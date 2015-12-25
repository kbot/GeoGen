using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GGIceEffectController : GGGroundEffect {
	
	float nextSpawnRadius;
	List<Vector3> flakePatternList;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		flakePatternList = FlakePatternFactory.Instance.generate(8,12);
		Vector3 vertInEffectSpace;
		foreach (Vector3 vert in flakePatternList) {
			vertInEffectSpace = transform.position + vert;
			if (GGLevelManager.Instance.isInsideLevel(vertInEffectSpace)) {
				GameObject iceEffect = instantiateEffectPrefab();
				iceEffect.transform.position = vertInEffectSpace;	
			}
		}
	}
	
//	IEnumerator spawnTick() {
//		GameObject newTile;
//		while (nextSpawnRadius < 0) {
//			yield return new WaitForSeconds(Time.fixedDeltaTime);
//
//			yield return null;
//		}
//	}
}
