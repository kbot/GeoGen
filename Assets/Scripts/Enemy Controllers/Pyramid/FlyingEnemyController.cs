using UnityEngine;
using System.Collections;

public enum EnemyFlightState 
{
	Flying = 1,
	Landing,
	Landed,
	LiftingOff
}

public class FlyingEnemyController : MonoBehaviour
{
	EnemyFlightState flightState;

	protected bool bIsMoving;
	protected Vector3 originFlightPosition;
	protected Vector3 finalFlightDestination;
	protected float fDestinationLerp;

	public float minFlightDistanceFromFloor;

	public delegate void FlightStateChangedDelegate(EnemyFlightState flightState);
	public FlightStateChangedDelegate flightStateDelegate;

	// Use this for initialization
	void Start ()
	{
		bIsMoving = false;
	}

	public void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer("Environment")) {
			changeFlightState (EnemyFlightState.Landed);
		}
	}

	// Update is called once per frame
	void Update () {

		switch (flightState) {
		case EnemyFlightState.Flying:
			{
				if (!bIsMoving) {
					bIsMoving = true;
					fDestinationLerp = 0.0f;
					originFlightPosition = transform.position;
					//pick a random spot in the level
					finalFlightDestination = GGLevelManager.Instance.getRandomLevelPosition ();	
					finalFlightDestination.y = transform.position.y;
					Debug.Log ("position to move to " + finalFlightDestination);
				} else {
					fDestinationLerp += Time.smoothDeltaTime * 0.5f;
					//move to destination
					transform.position = Vector3.Lerp(originFlightPosition,finalFlightDestination,fDestinationLerp);
					if (Vector3.Distance(transform.position,finalFlightDestination) < 3.0f) {
						//check below for enemies? or maybe just always blast all objects below to clear some space
						RaycastHit hitInfo;
						bool bIsCollidingWithEnemy = false;
						if (Physics.Raycast (GetComponentInParent<MeshCollider>().bounds.min, Vector3.down, out hitInfo, 100.0f, LayerMask.NameToLayer ("Enemies"))) {
							Debug.Log ("Enemy below! " + hitInfo.collider.ToString());
							bIsCollidingWithEnemy = true;
						}
						if (bIsCollidingWithEnemy) {
							//pick another location?
							bIsMoving = false;
							//nah...blast everything out of the way!!!!

						} else {
							//change state to landing
							bIsMoving = false;
							changeFlightState (EnemyFlightState.Landing);
						}
					}
				}		
			}
			break;
		case EnemyFlightState.LiftingOff:
		case EnemyFlightState.Landing:
			{
				//always check for height and stay above x distance to the floor
				float distanceToDestination = Vector3.Distance(finalFlightDestination,transform.position);
				bool bShouldMove = distanceToDestination > 1;

				if (bShouldMove) {
					fDestinationLerp += Time.smoothDeltaTime;

					Vector3 translate = Vector3.Lerp (originFlightPosition, finalFlightDestination, fDestinationLerp);

					//transform.Translate (translate);
					transform.position = translate;
						
				} else if (flightState == EnemyFlightState.Landing) {
					changeFlightState (EnemyFlightState.Landed);
				} else {
					changeFlightState (EnemyFlightState.Flying);
				}
			}
			break;
		default:
			break;
		}
	}

	public void changeFlightState(EnemyFlightState newState)
	{
		if (flightState == newState)
			return;

		flightState = newState;

		switch (newState) {
		case EnemyFlightState.Flying:
			{
				bIsMoving = false;
				fDestinationLerp = 0.0f;
			}
			break;
		case EnemyFlightState.LiftingOff:
			{
				fDestinationLerp = 0.0f;
				originFlightPosition = transform.position;
				RaycastHit floorCollisionPoint = GGLevelManager.Instance.getTransformOnFloorForPostionInLevel(transform.position);
				finalFlightDestination = floorCollisionPoint.point + Vector3.up * minFlightDistanceFromFloor;
			
				enabled = true;
				GetComponentInParent<Rigidbody> ().useGravity = false;
			}
			break;
		case EnemyFlightState.Landing:
			{
				fDestinationLerp = 0.0f;
				originFlightPosition = transform.position;
				RaycastHit floorCollisionPoint = GGLevelManager.Instance.getTransformOnFloorForPostionInLevel(transform.position);
				finalFlightDestination = floorCollisionPoint.point;
			}
			break;
		case EnemyFlightState.Landed:
			{
				enabled = false;
				GetComponentInParent<Rigidbody> ().useGravity = true;
			}
			break;
		default:
			break;
		}

		//notify the delegate
		if (flightStateDelegate != null) {
			flightStateDelegate (flightState);
		}
	}
}

