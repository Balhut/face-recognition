#pragma once
#include <opencv2/core/core.hpp>                        //Sposób 
#include <opencv2/highgui/highgui.hpp>                    //dok³adny 
#include "opencv2/objdetect/objdetect.hpp" 
#include "opencv2/imgproc/imgproc.hpp" 
#include <string> 
#include <iostream> 


using namespace cv;
using namespace std;

string face_cascade_name = "haarcascade_frontalface_alt.xml";    
CascadeClassifier face_cascade;                                


Mat FaceDetection(Mat img)
{
	vector<Rect> faces;
	Mat img_gray;

	cvtColor(img, img_gray, CV_BGR2GRAY);


	face_cascade.detectMultiScale(img_gray, faces, 1.1, 3, 0 | CV_HAAR_SCALE_IMAGE, Size(50, 50));
	for (unsigned i = 0; i < faces.size(); i++)
	{
		Rect rect_face(faces[i]);

		rectangle(img, rect_face, Scalar(120, 5, 86), 2, 2, 0);
	}
	return img;
}


