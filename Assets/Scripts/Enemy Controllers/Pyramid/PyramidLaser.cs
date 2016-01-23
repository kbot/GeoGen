using UnityEngine;
using System.Collections;

public class PyramidLaser : MonoBehaviour {

	public float targetLag;
	public float laserLifetime;
	public ParticleSystem laserCollisionSpark;

	private LineRenderer laserRenderer;
	private Vector3 laggedTargetPosition;
	private Vector3 currentReticlePosition;
	private AI_Seeker seekerComponent;

	// Use this for initialization
	void Start () {
		laserRenderer = GetComponentInParent<LineRenderer> ();
		seekerComponent = GetComponentInParent<AI_Seeker> ();
		currentReticlePosition = laggedTargetPosition = seekerComponent.target.transform.position;
		laserRenderer.enabled = false;
		ParticleSystem.EmissionModule emitter = laserCollisionSpark.emission;
		emitter.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (laserRenderer.enabled) {
			currentReticlePosition += (laggedTargetPosition - currentReticlePosition) * targetLag * Time.smoothDeltaTime;

			laserRenderer.SetPosition(0,transform.position);

			RaycastHit hitInfo;
			if (Physics.Raycast (transform.position, (currentReticlePosition - transform.position).normalized, out hitInfo, 100.0f, Physics.AllLayers)) {
				laserRenderer.SetPosition (1,  hitInfo.point);
				laserCollisionSpark.transform.position = hitInfo.point + Vector3.up * 0.25f;
			}
		}
	}

	public void FireLaser () {
		StartCoroutine (delayedTargetPositionCoRoutine ());
		laserRenderer.enabled = true;
		ParticleSystem.EmissionModule emitter = laserCollisionSpark.emission;
		emitter.enabled = true;
		StartCoroutine (laserKillCoRoutine ());
	}

	IEnumerator delayedTargetPositionCoRoutine () {
		while (true) {
			laggedTargetPosition = seekerComponent.target.transform.position;
			yield return new WaitForSeconds (targetLag);	
		}
	}

	IEnumerator laserKillCoRoutine () {
		yield return new WaitForSeconds (laserLifetime);
		ParticleSystem.EmissionModule emitter = laserCollisionSpark.emission;
		emitter.enabled = laserRenderer.enabled = false;
		GetComponentInParent<FlyingEnemyController> ().changeFlightState (EnemyFlightState.LiftingOff);
	}
}
