using UnityEngine;
using System.Collections;

public class GGEnemy : MonoBehaviour {

	protected float fHealth;
	protected float fMaxHealth;
	public float fBaseHealth;
	protected float fLifeTime;

	public float MovesPerMinute = 10;
	public float MoveForce = 100;
	protected bool bMoveThisFrame;

	protected Vector3 moveVector;

	//protected MonoBehaviour enemyAIScript;

	// Use this for initialization
	protected virtual void Start () {
		bMoveThisFrame = true;
		fHealth = fMaxHealth = generateInitialHealth ();
		if (GetComponent<AI_Seeker> ().target == null) {
			Debug.Log("GGEnemy target not set, attempting to default to GGGameManager.Player");
			setTarget(GGGameManager.Instance.Player);
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		fLifeTime += Time.deltaTime;
	}

	void OnParticleCollision(GameObject otherObject) {
		//Debug.Log ("OnParticleCollision");
		if (otherObject.tag == "Bullet") {
			//takeDamage (otherObject.GetComponent<GGBullet>().fBulletStrength);
			bMoveThisFrame = false;	
		}
	}

	void OnCollisionEnter(Collision collision) {
		bMoveThisFrame = false;
	}

	public virtual float takeDamage(float fDamage)
	{
		fHealth -= fDamage;
		Debug.Log ("takeDamage:" + fDamage + "RemainingHealth" + fHealth);
		if (fHealth <= 0.0f) {
			//spawn death emitter
			//spawn wireframe mesh obj
			//spawn point value
			Destroy(gameObject);
		}
		return fHealth;
	}

	protected virtual Vector3 getMoveVector(Vector3 vectorToTarget)
	{
		return Vector3.forward;
	}

	protected float generateInitialHealth()
	{
		return fBaseHealth * 2.0f;//replace with GameManager.healthModifierForWave
	}

	public void setTarget(GameObject target) {
		AI_Seeker seekerComponent = GetComponent<AI_Seeker> ();
		if (seekerComponent) {
			seekerComponent.target = target;
		}
	}
}
