using UnityEngine;
using System.Collections;

public enum PyramidState 
{
	Spawning = 1,
	Charging,
	Attacking,
}

public class PyramidController : GGEnemy {

	PyramidState currentState;

	void flightStateChangedCallback(EnemyFlightState newState) {
		switch (newState) {
		case EnemyFlightState.Landed:
			{
				changePyramidState (PyramidState.Charging);
			}
			break;
		default:
			break;
		}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		GetComponent<FlyingEnemyController> ().flightStateDelegate = new FlyingEnemyController.FlightStateChangedDelegate(flightStateChangedCallback);
		GetComponent<FlyingEnemyController> ().enabled = false;
		changePyramidState(PyramidState.Spawning);
	}

	public void changePyramidState(PyramidState newState)
	{
		if (currentState == newState)
			return;

		currentState = newState;
			
		switch (newState) {
		case PyramidState.Spawning:
			{
				//move to just above ground - then change state to moving
				StartCoroutine(spawningCoRoutine());
			}
			break;
		case PyramidState.Charging:
			{
				//Charge weapon. Fire. Change state to lifting off  
				GetComponent<PyramidCharge> ().beginCharging ();
				//animation material emmission effect on sin wave
				GetComponent<Animator>().SetTrigger("Pulsing");
			}
			break;
		case PyramidState.Attacking:
			{
				GetComponent<Animator>().ResetTrigger("Pulsing");
				GetComponentInChildren <PyramidLaser>().FireLaser();
			}
			break;
			default:
				break;
		}
	}

	IEnumerator spawningCoRoutine () {
		yield return new WaitForSeconds(1);
		GetComponent<FlyingEnemyController> ().changeFlightState (EnemyFlightState.LiftingOff);
	}
}
