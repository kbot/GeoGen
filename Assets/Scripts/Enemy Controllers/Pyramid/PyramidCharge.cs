using UnityEngine;
using System.Collections;

public class PyramidCharge : MonoBehaviour {

	public GameObject chargeAnimation;

	public void beginCharging () {
		chargeAnimation.GetComponent<Animator> ().SetTrigger ("isCharging");
		StartCoroutine (ChargeCoroutine ());
	}

	IEnumerator ChargeCoroutine() {
		yield return new WaitForSeconds (7);
		chargeAnimation.GetComponent<Animator> ().ResetTrigger ("isCharging");
		yield return new WaitForSeconds (3);
		GetComponentInParent<PyramidController> ().changePyramidState (PyramidState.Attacking);
	}
}
