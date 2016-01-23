using UnityEngine;
using System.Collections;

public class CylinderController : GGEnemy {

	public float TurnSpeed = 1.97f;
	public float SpeedMultiplerPerLevel;

	AI_Seeker seekerScript;

	Vector3 rotateToVector;
	int cylinderLevel = 1;

	Vector3 initialLocalScale;

	// Use this for initialization
	void Awake () {
		seekerScript = GetComponent<AI_Seeker> ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		initialLocalScale = transform.localScale;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		rotateToVector = seekerScript.getVectorToTarget ();
		//Quaternion lookAtRotation = Quaternion.LookRotation(newDir,Vector3.up);
		Quaternion lookAtRotation = Quaternion.FromToRotation(Vector3.forward,rotateToVector);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, lookAtRotation, TurnSpeed * (float)cylinderLevel * Time.deltaTime);
	}
	void FixedUpdate () {
		Vector3 normalizedTargetVector = rotateToVector.normalized;
		GetComponent<Rigidbody>().AddForce (normalizedTargetVector * (MoveForce * (SpeedMultiplerPerLevel * (float)cylinderLevel)));
	}

	public override float takeDamage (float fDamage)
	{
		float fRemainingHealth = base.takeDamage (fDamage);
		Debug.Log ("Cylinder.RemainingHealth = " + fRemainingHealth);
		switch ((int)(fRemainingHealth / (fMaxHealth/3))) {
		case 1:{
			//TODO - Replace with spawning new prefab that is smaller
			transform.localScale = initialLocalScale * 0.5f;
			cylinderLevel = 3;
		}
			break;
		case 2:{
			//TODO - Replace with spawning new prefab that is smaller
			transform.localScale = initialLocalScale * 0.75f;
			cylinderLevel = 2;
		}
			break;
			default:
				break;
		}
		return fRemainingHealth; 
	}

	// The angle between dirA and dirB around axis
	float AngleAroundAxis (Vector3 dirA, Vector3 dirB, Vector3 axis) {
		// Project A and B onto the plane orthogonal target axis
		dirA = dirA - Vector3.Project (dirA, axis);
		dirB = dirB - Vector3.Project (dirB, axis);
		
		// Find (positive) angle between A and B
		float angle = Vector3.Angle (dirA, dirB);
		
		// Return angle multiplied with 1 or -1
		return angle * (Vector3.Dot (axis, Vector3.Cross (dirA, dirB)) < 0 ? -1 : 1);
	}
}
