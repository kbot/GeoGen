using UnityEngine;
using System.Collections;

public class FloorEffectController : MonoBehaviour {

	float index;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		index += 0.01f;
		GetComponent<Renderer>().material.SetFloat("_index", index);
	}
}
