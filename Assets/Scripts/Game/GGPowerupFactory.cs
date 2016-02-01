using UnityEngine;
using System.Collections;

public enum PowerupType {
	Health,
	Shield,
	BulletModifier,
	Point
}

public class GGPowerupFactory : MonoBehaviour
{
	public GameObject[] powerUpPrefabs;

	// Use this for initialization
	void Start ()
	{
	
	}

	public GameObject instantiateRandomPowerup () {
		return GameObject.Instantiate (powerUpPrefabs [Random.Range (0, powerUpPrefabs.Length)]);
	}

}

