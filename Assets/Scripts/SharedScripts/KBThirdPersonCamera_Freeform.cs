using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

[AddComponentMenu("Camera-Control/Mouse Look")]
public class KBThirdPersonCamera_Freeform : MonoBehaviour {

	public Transform cameraTarget;
	public float fMinZoomDist = 5.0f;
	public float fMaxZoomDist = 10.0f;
	float fZoomDist = 5.0f;

	public float fMinPitch = 15.0f;
	public float fMaxPitch = 89.0f;
	float fPitch = 25.0f;
	bool bPitchLock = false;

	//Mouse sensitivity values 
	public float fMinSensitivyX = 5.0f;
	public float fMaxSensitivyX = 5.0f;
	float fSensitivyX = 5.0f;

	public float fMinSensitivyY = 3.0f;
	public float fMaxSensitivyY = 3.0f;
	float fSensitivyY = 1.25f;

	float rotationY = 0.0f;

	void Start ()
	{
		fPitch = fMinPitch;
		fZoomDist = fMaxZoomDist + fMinZoomDist / 2;
		bPitchLock = true;
	}
	void LateUpdate ()
	{
		//clamp Zoom
		fZoomDist -= Input.GetAxis("Mouse ScrollWheel");
		fZoomDist = Mathf.Clamp (fZoomDist, fMinZoomDist, fMaxZoomDist);

		//get the rotation around the Y-axis from the Mouse X movement
		rotationY += Input.GetAxis("Mouse X") * fSensitivyX;
		rotationY = Mathf.Clamp (rotationY, -360.0f, 360.0f);

		//Get the Pitch from the Mouse Y movement
		if(!bPitchLock) {
			fPitch += Input.GetAxis("Mouse Y") * fSensitivyY;
			fPitch = Mathf.Clamp (fPitch, fMinPitch, fMaxPitch);
		}

		Quaternion currentRotation = Quaternion.Euler (fPitch, -rotationY, 0);

		//move camera back from target based on the zoom
		transform.position = cameraTarget.position;
		transform.position += currentRotation * Vector3.back * fZoomDist;

		transform.LookAt (cameraTarget);
	}

	public Vector3 getCameraToTargetVector() {
		return cameraTarget.position - transform.position;
	}
}