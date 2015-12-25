using UnityEngine;
using System.Collections;

public class DeadPixelController : GGEnemy {
	
	AI_Seeker seekerScript;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		seekerScript = GetComponent<AI_Seeker> ();
		if (seekerScript == null) {
			Debug.LogError("DeadPixelController has no AI_Seeker Component");
		}
		StartCoroutine (deadPixelSeekCoroutine ());
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		//shitty move kind of like a low rez blob
//		if (bMoveThisFrame) {
//			transform.position += moveVector * MoveForce * Time.deltaTime;
//			bMoveThisFrame = false;
//		}

		Vector3 vecToTarget = seekerScript.getVectorToTarget ();
	
		moveVector = getMoveVector (vecToTarget);

		if (bMoveThisFrame) {
			rigidbody.AddForce(moveVector * MoveForce);
			bMoveThisFrame = false;
		}
	}

	protected override Vector3 getMoveVector(Vector3 vectorToTarget)
	{
		return Vector3Util.cardinalDirectionClosestToVector(vectorToTarget);
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			//TODO - Sap Life
		}
		if (other.tag == "Enemy") {
			//TODO - Poison
		}
		Destroy(this.gameObject);
	}

	IEnumerator deadPixelSeekCoroutine()
	{
		Vector3 vecToTarget;
		while ((vecToTarget = seekerScript.getVectorToTarget()).sqrMagnitude > 0.1f) {
			yield return new WaitForSeconds (60/MovesPerMinute); 
			Vector3 prevMoveVector = moveVector;
			moveVector = getMoveVector (vecToTarget);
			if (prevMoveVector != moveVector) {
				//changing directions, zero vel so that we turn on a dime
				rigidbody.velocity = Vector3.zero;
				Debug.Log("DeadPixelController.deadPixelSeekCoroutine Switching Directions");
			}
			bMoveThisFrame = true;
			yield return null;
		}
	}
}
