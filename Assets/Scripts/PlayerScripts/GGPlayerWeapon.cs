using UnityEngine;
using System.Collections;

public class GGPlayerWeapon : MonoBehaviour {

	static float playerWeaponUpgradeStep = 0.25f;

	public Vector3 fireVector;
	public float fFireRate;
	public float fMaxFireRate;

	public bool bAutoFire = false;

	//weapon modifiers
	public bool bEnableSinWave = false;

	public float fBulletStrength = 1.0f;

	public Transform[] sinEnabledEmitters;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation (fireVector);
		if (bAutoFire || Input.GetKeyDown(KeyCode.Mouse0)) {
			foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>()) {
				ps.startLifetime = float.MaxValue;
			}	
		}

		updateSinEnableEmitters();
	}

	void updateSinEnableEmitters ()
	{
		float fSinOffset = Mathf.Sin (Time.time) * Time.deltaTime;
		Vector3 positionStep = new Vector3 (0.2f, 0.0f, 0.0f);//TODO - make this a public var
		foreach (Transform emitterTransform in sinEnabledEmitters) {
			emitterTransform.transform.localPosition += positionStep * fSinOffset;
			fSinOffset *= -1.0f;//inverse the next emitter offset
		}
	}

	public void upgradeWeapon() {
		fFireRate += playerWeaponUpgradeStep;
		ParticleSystem.EmissionModule emission;
		foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>()) {
			emission = ps.emission;
			emission.rate = new ParticleSystem.MinMaxCurve(fFireRate);
		}	
	}

	public bool weaponRequiresTierUpgrade() {
		return fFireRate > fMaxFireRate;
	}
}
