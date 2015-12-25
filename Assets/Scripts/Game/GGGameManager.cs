using UnityEngine;
using System.Collections;

public class GGGameManager : Singleton<GGGameManager> {

	public GameObject Player;
	public GameObject mainCamera;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug
		if (Input.GetKeyDown(KeyCode.U)) {
			Player.GetComponent<GeoGenCharacterController>().upgradeWeapon();
		}
		if (Input.GetKeyUp(KeyCode.Tab)) {
			Player.GetComponent<GeoGenCharacterController>().playerWeaponComponent.bAutoFire = !Player.GetComponent<GeoGenCharacterController>().playerWeaponComponent.bAutoFire;
		}
	}

}
