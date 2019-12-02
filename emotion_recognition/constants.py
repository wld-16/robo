import numpy as np

__all__ = [
    'FACE_SCALE_FACTOR_X',
    'FACE_SCALE_FACTOR_Y',
    'FACE_SCALE_FACTOR',
    'EMOTION_MAP',
    'SRV_HOST',
    'SRV_PORT',
    'SRV_CMD_NEXT_FRAME',
    'SRV_CMD_QUIT',
    'SRV_CMD_SHUTDOWN',
]

FACE_SCALE_FACTOR_X = 0.0
FACE_SCALE_FACTOR_Y = 0.4

FACE_SCALE_FACTOR = np.array([FACE_SCALE_FACTOR_X, FACE_SCALE_FACTOR_Y])
FACE_SCALE_FACTOR.flags.writeable = False

EMOTION_MAP = {
    'angry': 0,
    'disgust': 1,
    'fear': 2,
    'happy': 3,
    'sad': 4,
    'surprise': 5,
    'neutral': 6
}

SRV_HOST = '127.0.0.1'
SRV_PORT = 1234

SRV_CMD_NEXT_FRAME = 0x0
SRV_CMD_QUIT = 0x1
SRV_CMD_SHUTDOWN = 0x2
