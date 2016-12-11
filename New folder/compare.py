import cv2
import numpy

img1 = cv2.imread('pic1.jpg', cv2.IMREAD_GRAYSCALE)
img2 = cv2.imread('pic2.jpg', cv2.IMREAD_GRAYSCALE)

diff = cv2.subtract(img1, img2)


result = not numpy.any(diff)

if result is True:
	print "Accepted"
else:
	print "Declined"
