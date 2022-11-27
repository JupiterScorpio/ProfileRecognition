#!/usr/bin/env python3
import os
import numpy as np
import pickle5 as pickle

def compare(name1,name2):
    with open(name1, 'rb') as infile:
        emb1 = pickle.load(infile)
    
    with open(name2, 'rb') as infile:
        emb2 = pickle.load(infile)

    dist = np.linalg.norm(emb1 - emb2)

    print(f'Distance between two images is {dist}')   

    return float(dist)

if __name__ == '__main__':
    img1 = "D:\\temp\\images\\7_aligned.jpg"
    img2 = "D:\\temp\\images\\7_raw.jpg"   
    emd1 = os.path.splitext(img1)[0]
    emd2 = os.path.splitext(img2)[0]
  
    dist = compare(emd1,emd2)
    print(dist)
   
    # run(img1, img2)
