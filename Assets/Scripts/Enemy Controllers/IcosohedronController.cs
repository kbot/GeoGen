using UnityEngine;
using System.Collections;

public class IcosohedronController : GGEnemy {

	void flightStateChangedCallback(EnemyFlightState newState) {
		switch (newState) {
		case EnemyFlightState.Landed:
			{
				
			}
			break;
		default:
			break;
		}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
		GetComponent<FlyingEnemyController> ().flightStateDelegate = new FlyingEnemyController.FlightStateChangedDelegate(flightStateChangedCallback);
		GetComponent<FlyingEnemyController> ().changeFlightState (EnemyFlightState.LiftingOff);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	
	}
}
