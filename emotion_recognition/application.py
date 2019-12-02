import os
from os.path import realpath, normpath
import cv2
import numpy as np
import tensorflow as tf
from emotion_recognition.functions import scale_rects, import_graph, prepare_face_for_emotion_net
from emotion_recognition.constants import FACE_SCALE_FACTOR, EMOTION_MAP


def main():
    # use context guard, otherwise memory might not be released until the system is rebooted
    with tf.Session(graph=import_graph(os.path.dirname(os.path.abspath(__file__)) + '/models/graph_emotion.pb', name='')) as session:
        # grab the output tensor of the net
        emotions_tensor = session.graph.get_tensor_by_name('predictions/Softmax:0')

        cap = cv2.VideoCapture(cv2.CAP_DSHOW)

        classifier = cv2.CascadeClassifier(normpath(realpath(cv2.__file__)) + '../../../../../share/OpenCV/haarcascades/haarcascade_frontalface_default.xml')

        while cap.isOpened():
            success, frame = cap.read()

            if success:
                # image is mirrored, in order to give a more natural interaction
                frame = np.flip(frame, axis=1)

                # copy to draw on
                out = np.copy(frame)

                faces = classifier.detectMultiScale(frame, minSize=(80, 80))

                faces = scale_rects(faces, FACE_SCALE_FACTOR, frame.shape)

                for x, y, w, h in faces:
                    cv2.rectangle(
                        out,
                        pt1=(x, y),  # upper left corner
                        pt2=(x + w, y + h),  # lower right corner
                        color=(0, 255, 0),
                        thickness=2
                    )

                    cropped_face = frame[y:y + h, x:x + w]

                    prepared_face = prepare_face_for_emotion_net(cropped_face)
                    out_emotions, = session.run([emotions_tensor], feed_dict={'image_array_input:0': prepared_face})

                    # for emotion_name, emotion_index in EMOTION_MAP.items():
                    #     print('{}: {:.3}'.format(emotion_name, out_emotions[0, emotion_index]))

                    y_offset = 0
                    fontFace = cv2.FONT_HERSHEY_SIMPLEX
                    fontScale = 0.5
                    thickness = 2
                    for emotion in ['happy', 'sad', 'angry']:
                        text = '{}: {:.2%}'.format(emotion, out_emotions[0, EMOTION_MAP[emotion]])

                        offset, _ = cv2.getTextSize(
                            text, fontFace=fontFace,
                            fontScale=fontScale,
                            thickness=thickness
                        )

                        y_offset += offset[1] + thickness

                        cv2.putText(
                            out,
                            text,
                            (x, y + h + y_offset),
                            fontFace=fontFace,
                            fontScale=fontScale,
                            thickness=thickness,
                            color=(0, 0, 255)
                        )

                cv2.imshow('Live', out)

                # gives the preview window some execution time, so that it does not freeze
                cv2.waitKey(10)

        cap.release()

    # close all remaining OpenCV windows
    cv2.destroyAllWindows()


if __name__ == '__main__':
    main()
