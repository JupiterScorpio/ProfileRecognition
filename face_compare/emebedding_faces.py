# import tensorflow as tf
import tensorflow.compat.v1 as tf
tf.disable_v2_behavior()

from align import detect_face
import facenet
import cv2
import imutils
import numpy as np
import pickle5 as pickle
import os
from imutils import paths

cur_path = os.path.dirname(__file__)    
# some constants kept as default from facenet
minsize = 20
threshold = [0.6, 0.7, 0.7]
factor = 0.709
margin = 44
input_image_size = 160

sess = tf.Session()
# read pnet, rnet, onet models from align directory and files are det1.npy, det2.npy, det3.npy
pnet, rnet, onet = detect_face.create_mtcnn(sess, cur_path + '/align')
facenet.load_model(cur_path+"/20170512-110547/20170512-110547.pb")

# Get input and output tensors
images_placeholder = tf.get_default_graph().get_tensor_by_name("input:0")
embeddings = tf.get_default_graph().get_tensor_by_name("embeddings:0")
phase_train_placeholder = tf.get_default_graph().get_tensor_by_name("phase_train:0")
embedding_size = embeddings.get_shape()[1]

def getFace(img):
    faces = []
    img_size = np.asarray(img.shape)[0:2]
    bounding_boxes, points = detect_face.detect_face(img, minsize, pnet, rnet, onet, threshold, factor)
    if not len(bounding_boxes) == 0:
        for face in bounding_boxes:
            if face[4] > 0.50:
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
    # print(feed_dict)
    embedding = sess.run(embeddings, feed_dict=feed_dict)
    return embedding

def get_embedding(image,savename):
    img = cv2.imread(image)
    img = imutils.resize(img,width=1000)
    faces = getFace(img)
       
    if len(faces) == 0:
        return False    
    
    face = faces[0]    
    embedding = face['embedding']
    with open(savename, 'wb') as outfile:
        pickle.dump(embedding, outfile)

    return True 

def compare(name1,name2):
    with open(name1, 'rb') as infile:
        emb1 = pickle.load(infile)
    
    with open(name2, 'rb') as infile:
        emb2 = pickle.load(infile)

    dist = np.linalg.norm(emb1 - emb2)

    print(f'Distance between two images is {dist}')   

    return float(dist)

def compare_face(image_one, image_two):
    # Load images
    image_one = cv2.imread(image_one)
    image_two = cv2.imread(image_two)

    if image_one is None or image_two is None:
        return -1

    face_one = getFace(image_one)
    face_two = getFace(image_two)

    # Calculate embedding vectors
    if len(face_one) == 0 or len(face_one) == 0:
        return -1

    embedding_one = face_one[0]['embedding']
    embedding_two = face_two[0]['embedding']

    dist = np.linalg.norm(embedding_one - embedding_two)
    print(f'Distance between two images is {dist}')
    if dist > 0.7:
        print('These images are of two different people!')
    else:
        print('These images are of the same person!-------------------------------')

    if dist < 0.7:
        cv2.imshow('face_one.png', face_one[0]['face'])
        cv2.imshow('face_two.png', face_two[0]['face'])
        cv2.waitKey()

    return dist

if __name__ == '__main__':
    img1 = "D:\\temp\\images\\7_aligned.jpg"
    img2 = "D:\\temp\\images\\7_raw.jpg"   
    emd1 = os.path.splitext(img1)[0]
    emd2 = os.path.splitext(img2)[0]
    # image_demo(img2,"D:\\temp\\crop\\2-crop.jpg")

    # # detect_alignface(img2)
    # get_embedding(img1,emd1)
    # get_embedding(img2,emd2)
    # dist = compare(emd1,emd2)
    # print(dist)
   
    # run(img1, img2)

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
            
            compare_face(imagePath,imgPath)
        i += 1
        