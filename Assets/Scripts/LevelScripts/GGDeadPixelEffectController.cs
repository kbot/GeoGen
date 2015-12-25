using UnityEngine;
using System.Collections;

public class GGDeadPixelEffectController : GGGroundEffect {

	public int spawnsPerMinute = 1;
	Vector3 spawnDir;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		//TODO - Make this a method in some kind of helper class
		float xDir = -1.0f * (float)(Random.Range(0,int.MaxValue) % 2);
		float zDir = xDir != 0.0f ? 0.0f : (Random.Range(0,int.MaxValue) % 2) > 0 ? -1.0f : 1.0f;
		spawnDir = new Vector3 (xDir, 0.0f, zDir);
		StartCoroutine (deadPixelSpawnCoroutine ());
	}


	IEnumerator deadPixelSpawnCoroutine()
	{
		while (lifetimeRemaining > 0.0f) {
			yield return new WaitForSeconds (60/spawnsPerMinute); 
			GameObject deadPixel = instantiateEffectPrefab();
			deadPixel.transform.parent = transform;
			deadPixel.transform.position = transform.position;
			yield return null;
		}
	}
}
