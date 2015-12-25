using UnityEngine;
using System.Collections;

public class GGWeaponFactory : Singleton<GGWeaponFactory> {

	public GameObject[] arrWeaponPrefabs;

	public GameObject getWeaponAtTier(int nTier) {
		if (nTier >= arrWeaponPrefabs.Length) {
			Debug.LogError("GGWeaponFactory.getWeaponAtTier: Trying to get Weapon beyond arrayPrefab.length, returning last object");
			return (GameObject)Instantiate(arrWeaponPrefabs[arrWeaponPrefabs.Length-1]);
		}
		return (GameObject)Instantiate(arrWeaponPrefabs[nTier]);
	}
}
