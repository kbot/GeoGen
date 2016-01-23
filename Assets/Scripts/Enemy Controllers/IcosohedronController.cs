using UnityEngine;
using System.Collections;

public class IcosohedronController : GGEnemy {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		GetComponent<FlyingEnemyController> ().changeFlightState (EnemyFlightState.LiftingOff);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	
	}
}
