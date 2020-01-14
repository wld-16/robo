using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class WebCamClient<TState> : MonoBehaviour {
    public static readonly byte[] CMD_NEXT = {0x0};
    public static readonly byte[] CMD_QUIT = {0x1};
    public static readonly byte[] CMD_SHUTDOWN = {0x2};

    public static readonly int RESPONSE_SIZE = Marshal.SizeOf<TState> ();

    public ChangeDelegate OnChange;

    public delegate void ChangeDelegate (TState[] newState, TState[] oldState);

    public String Host = "127.0.0.1";

    public ushort Port = 1234;

    public uint SkipFrames = 1;

    private TState[] _currentStates = null;

    private Socket _socket;

    protected void Connect () {
        if (this._socket == null) {
            Debug.Log (String.Format ("Trying connecting to server {0}:{1}", this.Host, this.Port));
            Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect (this.Host, this.Port);
            if (socket.Connected) {
                Debug.Log (String.Format ("Connected to server {0}:{1}", this.Host, this.Port));
                this._socket = socket;
            }
        }
    }

    protected void Disconnect () {
        if (this._socket != null) {
            this._socket.Send (CMD_QUIT);
            this._socket.Close ();
            this._socket = null;
        }
    }

    protected void Reconnect () {
        this.Disconnect ();
        this.Connect ();
    }

    private void OnEnable () {
        try {
            this.Reconnect ();
        } catch (Exception e) {
            Debug.LogError (e);
        }

        this.StartCoroutine (nameof (this.UpdateCoroutine));
    }

    private void OnDisable () {
        this.Disconnect ();
    }

    private IEnumerator UpdateCoroutine () {
        while (this.enabled) {
            if (this._socket != null && this._socket.Connected) {
                try {
                    this._socket.Send (CMD_NEXT);
                    this.HandleResponse ();
                } catch (Exception e1) {
                    Debug.LogError (e1);
                    try {
                        this.Reconnect ();
                    } catch (Exception e2) {
                        Debug.LogError (e2);
                        this._socket = null;
                    }

                    continue;
                }
            } else {
                try {
                    this.Reconnect ();
                } catch (Exception e) {
                    Debug.LogError (e);
                }
            }

            for (int i = 0; i < Math.Max (1, this.SkipFrames); i++) {
                yield return new WaitForEndOfFrame ();
            }
        }
    }

    protected void HandleResponse () {
        byte[] numFacesBuffer = new byte[1];
        if (this._socket.Receive (numFacesBuffer, 0, numFacesBuffer.Length, SocketFlags.None) == numFacesBuffer.Length) {
            byte numFaces = numFacesBuffer [0];

            TState[] newStates = new TState[numFaces];

            byte[] responseBuffer = new byte[RESPONSE_SIZE];
            for (byte i = 0; i < numFaces; i++) {
                if (this._socket.Receive (responseBuffer, 0, responseBuffer.Length, SocketFlags.None) == responseBuffer.Length) {
                    GCHandle handle = GCHandle.Alloc (responseBuffer, GCHandleType.Pinned);

                    TState emotionState = (TState) Marshal.PtrToStructure (handle.AddrOfPinnedObject (), typeof (TState));

                    handle.Free ();
                    newStates [i] = emotionState;
                }
            }

            TState[] oldEmotionStates = this._currentStates;

            this._currentStates = newStates;

            if (this.OnChange != null) {
                try {
                    this.OnChange.Invoke (newStates, oldEmotionStates);
                } catch (Exception e) {
                    //Debug.LogError (e);
                }
            }
        }
    }
}
