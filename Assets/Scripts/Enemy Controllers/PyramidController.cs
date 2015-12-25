using UnityEngine;
using System.Collections;

public class PyramidController : GGEnemy {

	enum PyramidState
	{
		Grounded = 0,
		Landing,
		LiftingOff,
		Moving
	}
	private PyramidState pyramidState;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		changePyramidState (PyramidState.Grounded, PyramidState.Grounded);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

	}

	void changePyramidState(PyramidState currState,PyramidState newState)
	{
//		switch (newState) {
//			case 
//			default:
//				break;
//		}
	}

//	IEnumerable groundedCoRoutine()
//	{
//	}
//	IEnumerable groundedCoRoutine()
//	{
//	}
}
