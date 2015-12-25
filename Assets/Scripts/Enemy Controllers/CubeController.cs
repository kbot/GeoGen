using UnityEngine;
using System.Collections;

public class CubeController : GGEnemy {

	AI_Seeker seekerScript;
	
	Vector3 torqueVector;

	// Use this for initialization
	void Awake () {
		seekerScript = GetComponent<AI_Seeker> ();
		StartCoroutine (cubeFlipCoroutine ());
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (bMoveThisFrame) {
			rigidbody.AddForce (Vector3.up * MoveForce);
			rigidbody.AddForce (moveVector * MoveForce/1.5f);
			rigidbody.AddTorque (torqueVector.normalized * MoveForce * 2.0f);
			bMoveThisFrame = false;
		}
	}

	protected override Vector3 getMoveVector(Vector3 vectorToTarget)
	{
		return Vector3Util.cardinalDirectionClosestToVector (vectorToTarget);
	}

	IEnumerator cubeFlipCoroutine()
	{
		Vector3 vecToTarget;
		while ((vecToTarget = seekerScript.getVectorToTarget()).sqrMagnitude > 0.1f) {
			yield return new WaitForSeconds (60/MovesPerMinute); 
			moveVector = getMoveVector (vecToTarget);
			torqueVector = Vector3.Cross(Vector3.up,moveVector);
			bMoveThisFrame = true;
			yield return null;
		}
	}
}
