using UnityEngine;
using System.Collections;

public class GGSandStormController : GGGroundEffect
{
	Vector3 spawnDir;
	Vector3 nextSpawnPos;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		StartCoroutine (spawnTick ());
		float xDir = -1.0f * (float)(Random.Range(0,int.MaxValue) % 2);
		float zDir = xDir != 0.0f ? 0.0f : (Random.Range(0,int.MaxValue) % 2) > 0 ? -1.0f : 1.0f;
		spawnDir = new Vector3 (xDir, 0.0f, zDir);
		nextSpawnPos = transform.position;
	}

	IEnumerator spawnTick() {
		GameObject newTile;
		while (GGLevelManager.Instance.isInsideLevel(nextSpawnPos)) {
			yield return new WaitForSeconds(Time.fixedDeltaTime);
			newTile = instantiateEffectPrefab();
			newTile.transform.RotateAround (transform.position,Vector3.up,Vector3.Angle(spawnDir,Vector3.right));
			newTile.transform.position = nextSpawnPos + transform.right * 1.5f;

			nextSpawnPos += spawnDir * newTile.GetComponent<Renderer>().bounds.size.magnitude * 0.5f;
			yield return null;
		}
	}
}

