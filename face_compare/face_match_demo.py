# import tensorflow as tf
import tensorflow.compat.v1 as tf
tf.disable_v2_behavior()

import numpy as np
import facenet
from align import detect_face
import cv2
import argparse
import imutils
from imutils import paths
import os
# parser = argparse.ArgumentParser()
# parser.add_argument("--img1", type = str, required=True)
# parser.add_argument("--img2", type = str, required=True)
# args = parser.parse_args()

cur_path = os.path.dirname(__file__)    
print(" ---------------" + cur_path)

# some constants kept as default from facenet
minsize = 20
threshold = [0.6, 0.8, 0.9]
factor = 0.709
margin = 0
input_image_size = 160

sess = tf.Session()

# read pnet, rnet, onet models from align directory and files are det1.npy, det2.npy, det3.npy
pnet, rnet, onet = detect_face.create_mtcnn(sess, 'align')

# facenet.load_model("20180408-102900/20180408-102900.pb")
facenet.load_model("20170512-110547/20170512-110547.pb")



# Get input and output tensors
images_placeholder = tf.get_default_graph().get_tensor_by_name("input:0")
embeddings = tf.get_default_graph().get_tensor_by_name("embeddings:0")
phase_train_placeholder = tf.get_default_graph().get_tensor_by_name("phase_train:0")
embedding_size = embeddings.get_shape()[1]

def getFace(img):
    faces = []
    img_size = np.asarray(img.shape)[0:2]
    bounding_boxes, _ = detect_face.detect_face(img, minsize, pnet, rnet, onet, threshold, factor)
    if not len(bounding_boxes) == 0:
        for face in bounding_boxes:
            if face[4] > 0.95:
                det = np.squeeze(face[0:4])
                bb = np.zeros(4, dtype=np.int32)
                bb[0] = np.maximum(det[0] - margin / 2, 0)
                bb[1] = np.maximum(det[1] - margin / 2, 0)
                bb[2] = np.minimum(det[2] + margin / 2, img_size[1])
                bb[3] = np.minimum(det[3] + margin / 2, img_size[0])
                cropped = img[bb[1]:bb[3], bb[0]:bb[2], :]
                resized = cv2.resize(cropped, (input_image_size,input_image_size),interpolation=cv2.INTER_CUBIC)
                prewhitened = facenet.prewhiten(resized)
                faces.append({'face':resized,'rect':[bb[0],bb[1],bb[2],bb[3]],'embedding':getEmbedding(prewhitened)})
    return faces
def getEmbedding(resized):
    reshaped = resized.reshape(-1,input_image_size,input_image_size,3)
    feed_dict = {images_placeholder: reshaped, phase_train_placeholder: False}
    embedding = sess.run(embeddings, feed_dict=feed_dict)
    return embedding

def compare2face(img1,img2):
    face1 = getFace(img1)
    face2 = getFace(img2)
    # face1 = detect_alignface(img1)
    # face2 = detect_alignface(img2)
    
    if face1 and face2:
        # calculate Euclidean distance
        dist = np.sqrt(np.sum(np.square(np.subtract(face1[0]['embedding'], face2[0]['embedding']))))
        return dist
    return -1

# img1 = cv2.imread("D:\\temp\\\images\\daniel-radcliffe_2.jpg")
# img2 = cv2.imread("D:\\temp\\\images\\daniel-radcliffe_3.jpg")
# distance = compare2face(img1, img2)
# threshold = 1.00    # set yourself to meet your requirement
# print("distance = "+str(distance))
# print("Result = " + ("same person" if distance <= threshold else "not same person"))

crop_path = "D:\\temp\\images"
imagePaths = sorted(list(paths.list_images(crop_path)))

i = 0 
for imagePath in imagePaths:
    filename0 = os.path.basename(imagePath)
    for j in range(i+1, len(imagePaths)):
        imgPath = imagePaths[j]
        filename1 = os.path.basename(imgPath)
        if filename0 == filename1:
            continue
        
        img1 = cv2.imread(imagePath)
        img2 = cv2.imread(imgPath)
        distance = compare2face(img1, img2)
        thresh = 1.0   # set yourself to meet your requirement
        print("distance = "+str(distance))
        print("Result = " + ("same person" if distance <= thresh else "not same person"))

        if thresh > distance:
            cv2.imshow("img1",img1)
            cv2.imshow("img2",img2)
            cv2.waitKey()
    i += 1
