using UnityEngine;
using System.Collections;

public class GGPlayerWeapon : MonoBehaviour {

	public GameObject[] bulletPrefabs;

	static float playerWeaponUpgradeStep = 0.25f;

	GameObject[] arrBulletParticleSystems;

	public Vector3 fireVector;
	public float fFireRate;
	public float fMaxFireRate;

	public bool bAutoFire = false;

	//weapon modifiers
	public bool bEnableSinWave = false;
	ParticleSystem.Particle[] particles;

	// Use this for initialization
	void Start () {
		//build the emmitters
		GGBullet[] arrBulletEmitters = this.GetComponentsInChildren<GGBullet>();
		arrBulletParticleSystems = new GameObject[arrBulletEmitters.Length];
		for (int i = 0; i < arrBulletEmitters.Length; i++) {
			arrBulletParticleSystems[i] = (GameObject)Instantiate(bulletPrefabs[arrBulletEmitters[i].prefabIndex],arrBulletEmitters[i].transform.position,Quaternion.identity);
			arrBulletParticleSystems[i].transform.parent = arrBulletEmitters[i].transform;
		}

		if(bEnableSinWave) {
			particles = new ParticleSystem.Particle[1000];
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation (fireVector);
		if (bAutoFire || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Q)) {
			for (int i = 0; i < arrBulletParticleSystems.Length; i++) {
				foreach(ParticleSystem ps in arrBulletParticleSystems[i].GetComponents<ParticleSystem>()) {
					ps.startLifetime = float.MaxValue;
				}	
			}
		}

		updateSinEnableEmitters();

		//DEBUG - testing upgrades
		if (Input.GetKeyDown (KeyCode.U)) {
			upgradeWeapon ();
		}
	}

	void updateSinEnableEmitters ()
	{
		GGBullet[] arrBulletEmitters = this.GetComponentsInChildren<GGBullet>();
		for (int i = 0; i < arrBulletEmitters.Length; i++) {
			if (arrBulletEmitters[i].allowsSinWave) {
				float fSinOffset = Mathf.Sin (Time.time) * Time.deltaTime;
				arrBulletEmitters[i].transform.position += new Vector3(0.0f,0.0f,0.5f) * fSinOffset;
			}
		}
	}

	public void upgradeWeapon() {
		fFireRate += playerWeaponUpgradeStep;
		for (int i = 0; i < arrBulletParticleSystems.Length; i++) {
			foreach(ParticleSystem ps in arrBulletParticleSystems[i].GetComponents<ParticleSystem>()) {
				ps.emissionRate = fFireRate;
			}	
		}
	}

	public bool weaponRequiresTierUpgrade() {
		return fFireRate > fMaxFireRate;
	}
}
