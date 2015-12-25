using UnityEngine;
using System.Collections;

public static class Vector3Util {
	
	public static Vector3 cardinalDirectionClosestToVector(Vector3 inVector){
		float angleFromForwardVector = Vector3.Angle (Vector3.forward, inVector);
		if (angleFromForwardVector < 45.0f) {
			return Vector3.forward;
		}
		float angleFromRightVector = Vector3.Angle (Vector3.right, inVector);
		if(angleFromRightVector < 45.0f) {
			return Vector3.right;
		}
		float angleFromLeftVector = Vector3.Angle (Vector3.left, inVector);
		if(angleFromLeftVector < 45.0f) {
			return Vector3.left;
		}
		return Vector3.back;
	}
}
