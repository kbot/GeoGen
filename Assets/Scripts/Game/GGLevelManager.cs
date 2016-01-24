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

	public GameObject floor;
	private Texture2D floorGridTex;
	private Color[] colors;

	private ArrayList fifoLastKnownPlayerPositions;
	private const int maxKnownPlayerPositions = 4;

	// Use this for initialization
	void Start () {
		fifoLastKnownPlayerPositions = new ArrayList (maxKnownPlayerPositions);
		floorGridTex = new Texture2D (20, 20, TextureFormat.ARGB32, true, true);
		floorGridTex.filterMode = FilterMode.Point;
		floor.GetComponent<Renderer>().material.SetTexture("_GridTex",floorGridTex);
		colors = new Color[floorGridTex.width * floorGridTex.height];
		for (int i = 0; i < floorGridTex.width * floorGridTex.height; ++i) {
			colors [i] = Color.white;
		}
		floorGridTex.SetPixels(0,0,floorGridTex.width,floorGridTex.height,colors);
		floorGridTex.Apply ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
		floorGridTex.SetPixels(0,0,floorGridTex.width,floorGridTex.height,colors);

		illuminateFloorAtPositionsWithColor (fifoLastKnownPlayerPositions.ToArray (), Color.blue);
	}

	public void initializeLevelDirectory(string filePath)
	{

	}
	public void loadLevel (int nLevel)
	{

	}

	public void beginLevelWithCountdown (int countdownFrom)
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

	public Vector3 getRandomLevelPosition() {
		
		float minX = float.MaxValue, maxX = float.MinValue, minZ = float.MaxValue, maxZ = float.MinValue;
		foreach (Transform wall in walls) {
			minX = Mathf.Min (minX,wall.transform.position.x);
			minZ = Mathf.Min (minZ,wall.transform.position.z);
			maxX = Mathf.Max (maxX,wall.transform.position.x);
			maxZ = Mathf.Max (maxZ,wall.transform.position.z);
		}
		//we've got the min and max values for the level space, calc randoms in those ranges
		float randXPos = Random.Range(minX,maxX);
		float randZPos = Random.Range(minZ,maxZ);

		Vector3 returnPosition = new Vector3(randXPos,floor.transform.position.y,randZPos);

		return returnPosition;
	}

	public void pushKnownPlayerPosition(Vector3 position) {
		if (fifoLastKnownPlayerPositions == null) {
			fifoLastKnownPlayerPositions = new ArrayList (maxKnownPlayerPositions);
		}
		if (fifoLastKnownPlayerPositions.Count + 1 > maxKnownPlayerPositions) {
			fifoLastKnownPlayerPositions.RemoveAt (0);
		}
		fifoLastKnownPlayerPositions.Add (position);
	}

	public void illuminateFloorAtPositionsWithColor(object [] arrayPositions, Color illuminationColor) {
		int xOffset;
		int yOffset;
		foreach (Vector3 item in arrayPositions) {
			xOffset = (int)((item.x+25) * 20/50);
			yOffset = (int)((item.z+25) * 20/50);
			floorGridTex.SetPixel(xOffset,yOffset,illuminationColor);	
		}

		floorGridTex.Apply ();
	}
}
