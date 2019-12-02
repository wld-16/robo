import cv2
import numpy as np
import tensorflow as tf

__all__ = [
    'import_graph',
    'scale_rects',
    'prepare_face_for_emotion_net'
]


def import_graph(file_name: str, name: str = None) -> tf.Graph:
    graph = tf.Graph()
    with graph.as_default():
        graph_def = tf.compat.v1.GraphDef()
        with tf.io.gfile.GFile(file_name, 'rb') as f:
            graph_def.ParseFromString(f.read())
            tf.import_graph_def(graph_def, name=name)
    return graph


def scale_rects(rects, scale, frame_size=None):
    """
    Scales rectangles, such as faces, by scale, while limiting it to the bounds of the frame
    :param rects: a numpy array of shape (?, 4)
    :param scale: either a number or a numpy array of shape (2,)
    :param frame_size: tuple of the frame size (height, width)
    :return:
    """
    assert type(rects) is np.ndarray or type(rects) is tuple
    assert type(scale) is float or type(scale) is int or (
            type(scale) is np.ndarray and len(scale.shape) == 1 and scale.shape[0] == 2
    )
    assert frame_size is None or (type(frame_size) is tuple and len(frame_size) >= 2)

    if len(rects) > 0:
        scaled_rects = np.copy(rects)
        size_increased_by = rects[:, 2:] * scale
        scaled_rects[:, 2:] += size_increased_by.astype(scaled_rects.dtype)
        scaled_rects[:, :2] -= (size_increased_by / 2).astype(scaled_rects.dtype)

        # ensure that the coordinates are still inside the image
        scaled_rects[:, :2] = np.maximum(scaled_rects[:, :2], [0, 0])

        if frame_size is not None:
            # ensure that the face rect does not extend beyond the image
            frame_dimensions = np.flip(np.array(frame_size[:2]))

            # dimensions expanded to end of frame
            max_dimensions = np.maximum(np.zeros((2,)), frame_dimensions - scaled_rects[:, :2])

            # mask where face rectangles would exceed the frame dimensions
            mask = frame_dimensions < scaled_rects[:, :2] + scaled_rects[:, 2:]

            # overwrite values with max_dimensions where mask applies
            scaled_rects[:, 2:][mask] = max_dimensions[mask]

        return scaled_rects
    else:
        return rects


def prepare_face_for_emotion_net(face):
    # bring the color channels in the correct order and cast to float
    face = cv2.cvtColor(face, cv2.COLOR_BGR2RGB).astype(np.float)

    # normalize values to range [0.0, 1.0]
    face = (face - 127.5) / 127.5

    # resize to size used by the net
    face = cv2.resize(
        face,
        (48, 48),
        interpolation=cv2.INTER_LINEAR
    )

    face_min = np.min(face)
    face_max = np.max(face)

    face_normalized = face - (face_min / (face_max - face_min))
    face_mean = np.mean(face_normalized, axis=-1)

    return face_mean[np.newaxis, :, :, np.newaxis]
