using UnityEngine;
using System.Collections;

public class GGLevelManager : Singleton<GGLevelManager> {

//	public struct GGLevelProperties
//	{
//		float fGridX;
//		float fGridY;
//
//	}

	private int nCurrentLevel;

	public Transform[] walls;
	public Transform floor;

	public void initializeLevelDirectory(string filePath)
	{

	}
	public void loadLevel (int nLevel)
	{

	}

	public bool isInsideLevel(Vector3 pos) {
		Vector3 wallForward;
		Vector3 toOther;
		foreach (Transform wall in walls) {
			wallForward = wall.TransformDirection(Vector3.forward);
			toOther = pos - wall.position;
			float fDotVal = Vector3.Dot(wallForward,toOther);
			//walls face outward for collision, so actually check if the object is in front of the wall
			if (fDotVal > 0) {
				return false;
			}
		}
		return true;
	}

	public RaycastHit getTransformOnFloorForPostionInLevel(Vector3 pos) {
		//drop a raycast from above the level downward until it hits a level object
		RaycastHit hitInfo;
		if (!Physics.Raycast (pos, Vector3.down, out hitInfo, 100.0f, Physics.AllLayers ^ LayerMask.NameToLayer ("Environment"))) {
			Debug.Log("GGLevelManager.getTransformOnFloorForPostionInLevel didn't find anything to colide with...");
		}
		//assign that position, also get the quaternion rotation of the reflect
		return hitInfo;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
