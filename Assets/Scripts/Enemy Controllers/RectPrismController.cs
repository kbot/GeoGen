using UnityEngine;
using System.Collections;

public class RectPrismController : GGEnemy {
	
	public float FieldOfVisionInDegrees = 15.0f;

	AI_Seeker seekerScript;

	bool bCanJabThisFrame;

	Vector3 torqueVector;
	
	void Awake () {
		seekerScript = GetComponent<AI_Seeker> ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		StartCoroutine (rectFlipCoroutine ());
		bCanJabThisFrame = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		Vector3 vectorToTarget = seekerScript.getVectorToTarget ();
		if (rectCheckShouldJab(vectorToTarget, Vector3.right) || rectCheckShouldJab(vectorToTarget, Vector3.left)) {
			Debug.Log("Adding Force for JAB");
			GetComponent<Rigidbody>().AddForce (moveVector * MoveForce * 2.75f);
			bCanJabThisFrame = false;
		}
		else if (bMoveThisFrame) {
			GetComponent<Rigidbody>().AddForce (Vector3.up * MoveForce);
			GetComponent<Rigidbody>().AddForce (moveVector * MoveForce/1.75f);
			GetComponent<Rigidbody>().AddTorque (torqueVector.normalized * MoveForce * 2.75f);
			bMoveThisFrame = false;
		}
	}

	protected override Vector3 getMoveVector(Vector3 vectorToTarget)
	{
		float angleFromForwardVector = Vector3.Angle (Vector3.forward, vectorToTarget);
		if(angleFromForwardVector < 90.0f) {
			return Vector3.forward;
		}
		return Vector3.back;
	}

	IEnumerator rectFlipCoroutine () {
		Vector3 vecToTarget;
		while ((vecToTarget = seekerScript.getVectorToTarget()).sqrMagnitude > 0.1f) {
			yield return new WaitForSeconds (60/MovesPerMinute); 
			moveVector = getMoveVector (vecToTarget);
			torqueVector = Vector3.Cross(Vector3.up,moveVector);
			bMoveThisFrame = true;
			bCanJabThisFrame = true;
			yield return null;
		}
	}
	bool rectCheckShouldJab (Vector3 vectorToTarget, Vector3 jabVector) {
		if (bCanJabThisFrame) {
			float angleToJabVector = Vector3.Angle(vectorToTarget,jabVector);
			if (angleToJabVector < FieldOfVisionInDegrees){
				//Debug.Log ("in rectJabCoroutine");
				moveVector = jabVector;
				torqueVector = Vector3.zero;	
				return true;
			}
		}
		return false;
	}
}
