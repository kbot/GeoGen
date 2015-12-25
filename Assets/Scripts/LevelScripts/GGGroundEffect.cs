using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GGGroundEffect : MonoBehaviour {
	public GameObject effectPrefab;
	public float lifetimeRemaining;

	protected List<GameObject> spawnedTiles;

	protected virtual void Start() {
		spawnedTiles = new List<GameObject> (50);
	}
	protected virtual void Update () {
		lifetimeRemaining -= Time.deltaTime;
		if (lifetimeRemaining < 0) {
			spawnedTiles.Clear();
			Destroy(this.gameObject);
		}
	}
	protected GameObject instantiateEffectPrefab() {
		GameObject newPrefab = (GameObject)Instantiate(effectPrefab);
		spawnedTiles.Add (newPrefab);
		newPrefab.transform.parent = transform;
		return newPrefab;
	}
}
