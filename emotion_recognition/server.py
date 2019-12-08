import cv2
import numpy as np
import tensorflow as tf
from functions import scale_rects, import_graph, prepare_face_for_emotion_net
from constants import FACE_SCALE_FACTOR, SRV_CMD_NEXT_FRAME, SRV_CMD_QUIT, SRV_CMD_SHUTDOWN, SRV_HOST, SRV_PORT
from os.path import realpath, normpath
import socket
import struct
import ctypes
import os


def main():
    # use context guard, otherwise memory might not be released until the system is rebooted
    with tf.compat.v1.Session(graph=import_graph(os.path.dirname(os.path.abspath(__file__)) + '/models/graph_emotion.pb', name='')) as session:
        # grab the output tensor of the net
        emotions_tensor = session.graph.get_tensor_by_name('predictions/Softmax:0')
        print(normpath(realpath(cv2.__file__)))
        #classifier = cv2.CascadeClassifier('/home/spacewave/opencv_build/opencv/data/haarcascades/haarcascade_frontalface_default.xml')
        classifier = cv2.CascadeClassifier('D:\Program Files (x86)\Anaconda3\Lib\site-packages\cv2\data\haarcascade_frontalface_default.xml')
        # create listening socket
        listen = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        # reuse address to prevent crash when in TIME_WAIT from previous start
        listen.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)

        # bind to localhost port 1234
        listen.bind((SRV_HOST, SRV_PORT))

        # start listening, no backlog is required, when intended for single user, otherwise 10 is a reasonable number
        listen.listen(0)

        while True:
            # waiting for incoming connection
            conn, addr = listen.accept()
            print('New Connection')

            do_quit = False

            try:
                cap = cv2.VideoCapture(0)
                while cap.isOpened():
                    # sync with client, shut down when receiving 0x1, command has length of 1 byte
                    raw_command = conn.recv(1)
                    command, = struct.unpack('B', raw_command)
                    if command != SRV_CMD_NEXT_FRAME:
                        if command == SRV_CMD_SHUTDOWN:
                            do_quit = True
                            break
                        elif command == SRV_CMD_QUIT:
                            break
                        else:
                            continue

                    success, frame = cap.read()

                    if success:
                        send_buffer = b''
                        # image is mirrored, in order to give a more natural interaction
                        frame = np.flip(frame, axis=1)
                        print(cap.get(3))
                        print(cap.get(4))

                        faces = classifier.detectMultiScale(frame, minSize=(80, 80))

                        faces = scale_rects(faces, FACE_SCALE_FACTOR, frame.shape)

                        num_faces = 0
                        if type(faces) is np.ndarray:
                            if faces.shape[0] <= 255:
                                num_faces = faces.shape[0]
                            else:
                                num_faces = 255

                        send_buffer += bytes(ctypes.c_ubyte(num_faces))
                        print(send_buffer)

                        for face_index in range(num_faces):
                            x, y, w, h = faces[face_index]
                            cropped_face = frame[y:y + h, x:x + w]

                            prepared_face = prepare_face_for_emotion_net(cropped_face)
                            out_emotions, = session.run([emotions_tensor], feed_dict={'image_array_input:0': prepared_face})

                            send_buffer += struct.pack('7f', *(out_emotions[0].tolist()))
                            print(send_buffer)

                        conn.send(send_buffer)

                cap.release()
            except ConnectionResetError:
                if cap is not None:
                    cap.release()
                print('Connection Reset')

            if do_quit:
                break


if __name__ == '__main__':
    main()
