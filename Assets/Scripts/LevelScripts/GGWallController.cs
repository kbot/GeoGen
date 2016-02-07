using UnityEngine;
using System.Collections;

public class GGWallController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnParticleCollision(GameObject otherObject) {
		//get the collision point
		if (otherObject.tag == "Enemy") {
			
		}
		else if (otherObject.tag == "Bullet") {
			ParticleCollisionEvent [] collisionEvents = new ParticleCollisionEvent[1];
			ParticlePhysicsExtensions.GetCollisionEvents (otherObject.GetComponent<ParticleSystem>(),this.gameObject, collisionEvents);
			//spawn wall decal
			//spawn explosion emitter
			if (collisionEvents.Length > 0) {
				ParticleCollisionEvent colEvent = collisionEvents [0];
				GameObject bulletCollisionParticle = GameObject.Instantiate(GGLevelManager.Instance.getBulletCollisionEffect());
				bulletCollisionParticle.transform.position = colEvent.intersection;
				bulletCollisionParticle.transform.rotation = Quaternion.FromToRotation (Vector3.zero, transform.rotation.eulerAngles);
				Destroy (bulletCollisionParticle, 0.5f);	
			}
		}
		else if (otherObject.tag == "Player") {
			//spawn wall decal
			//spawn any emitters
			//destroy bullet
		}
	}
}

