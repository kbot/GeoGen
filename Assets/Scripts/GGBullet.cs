using UnityEngine;
using System.Collections;

public class GGBullet : MonoBehaviour {

	public float fBulletStrength = 1.0f;
	public bool allowsSinWave = false;
	public int prefabIndex = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject otherObject) {

		if (otherObject.tag == "Enemy") {
			GGEnemy otherObjectEnemyComponent = otherObject.GetComponent<GGEnemy>();
			if (otherObjectEnemyComponent) {
				otherObjectEnemyComponent.takeDamage (fBulletStrength);	
			}
		}
		else if (otherObject.tag == "Wall") {
			//spawn wall decal
			//spawn explosion emitter
			GameObject bulletCollisionParticle = GameObject.Instantiate(GGLevelManager.Instance.getBulletCollisionEffect());
			//destroy bullet
			Destroy(gameObject);
		}
		else if (otherObject.tag == "Floor") {
			//spawn emitter
			//spawn explosion emitter
			//destroy bullet
			Destroy(gameObject);
		}
	}
}
