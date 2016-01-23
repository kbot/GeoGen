using UnityEngine;
using System.Collections;

public class ConeController : GGEnemy {

	public float MoveSpeed = 50.0f;
	public float OrbitMoveSpeed = 35.0f;
	public float fMaxOrbitDist = 16.0f;
	public float fMinOrbitDist = 8.0f;

	public float fMinDeltaForDrillBehavior = 10.0f;
	float fElapsedDrillTime = 0.0f;

	AI_Seeker seekerScript;

	float fRotationDir = -1.0f;

	Vector3 positionAtDrillBegin;

	//Ground Effects
	public GameObject[] arrEffectPrefabs;

	// Use this for initialization
	void Awake () {
		seekerScript = GetComponent<AI_Seeker> ();
	}
//	// Use this for initialization
//	void Start () {
//	
//	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (fMinDeltaForDrillBehavior < fLifeTime) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			fElapsedDrillTime += Time.deltaTime * 0.5f;
			float lerpVal = Mathf.Lerp(0.0f,360.0f,fElapsedDrillTime);
			transform.Rotate(Vector3.up,lerpVal * Time.deltaTime);
			if (lerpVal > 360.0f * 0.9f ) {
				GetComponent<Rigidbody>().GetComponent<Collider>().enabled = false;
				transform.Translate(-Vector3.up * Mathf.Lerp(0.0f,2.0f,fElapsedDrillTime * 0.5f) * Time.deltaTime);	
			}
			if ((positionAtDrillBegin - transform.position).sqrMagnitude > GetComponent<MeshRenderer>().bounds.size.sqrMagnitude) {
				RaycastHit floorCollisionPoint = GGLevelManager.Instance.getTransformOnFloorForPostionInLevel(positionAtDrillBegin);
				int randPrefabIndex = Random.Range(0,arrEffectPrefabs.Length-1);
				//DEBUG hard code 
				randPrefabIndex = 3;
				Instantiate(arrEffectPrefabs[randPrefabIndex],floorCollisionPoint.point + floorCollisionPoint.normal * 0.1f,Quaternion.identity);
				Destroy(this.gameObject);
			}

		} else { //seek behavior
			positionAtDrillBegin = transform.position;
			Vector3 toTargetVector = seekerScript.getVectorToTarget ();
			float distToTarget = toTargetVector.magnitude;
			if (distToTarget < fMinOrbitDist) {
				fRotationDir = getRotationDirection();
				Vector3 awayVector = (transform.position - seekerScript.target.transform.position);
				awayVector.y = 0.0f;
				GetComponent<Rigidbody>().AddForce(awayVector.normalized * MoveSpeed);
			}
			else if(distToTarget > fMaxOrbitDist) {
				//seek
				//transform.position = Vector3.Slerp(transform.position,seekerScript.target.transform.position,MoveSpeed * Time.deltaTime);
				GetComponent<Rigidbody>().AddForce(toTargetVector.normalized * MoveSpeed);
			}
			
			if (toTargetVector.magnitude > fMinOrbitDist && toTargetVector.magnitude < fMaxOrbitDist ) {
				//transform.RotateAround (seekerScript.target.transform.position, Vector3.up, OrbitMoveSpeed * Time.deltaTime * fRotationDir);
				GetComponent<Rigidbody>().AddForce(Vector3.Cross(toTargetVector.normalized,Vector3.up) * OrbitMoveSpeed * fRotationDir);
			}
		}
	}

//	IEnumerator drillStateUpdate() {
//		while () {
//		
//		}
//	}
//	IEnumerator defaultStateUpdate() {
//
//	}

	float getRotationDirection ()
	{
		//if the velocit of our target is moving a similar direction, switch rotation
		return Vector3.Dot (seekerScript.target.GetComponent<Rigidbody>().velocity.normalized,GetComponent<Rigidbody>().velocity.normalized) < 0.0f ? -1.0f : 1.0f;
	}
}
