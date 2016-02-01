using UnityEngine;
using System.Collections;

public class GGLevelManager : Singleton<GGLevelManager> {

	private int nCurrentLevel;

	public Transform[] walls;

	public GameObject floor;
	private FloorIlluminationController floorIllumController;

	private ArrayList fifoLastKnownPlayerPositions;
	private const int maxKnownPlayerPositions = 4;

	public GameObject floorPingPrefab; 

	// Use this for initialization
	void Start () {
		fifoLastKnownPlayerPositions = new ArrayList (maxKnownPlayerPositions);
		floorIllumController = floor.GetComponent<FloorIlluminationController> ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
		#if DEBUG
		if (Input.GetKeyDown(KeyCode.P)) {
			Vector3 mostRecentPlayerPos = (Vector3)fifoLastKnownPlayerPositions[0];
			Vector3 gridAlignedPos = gridAlignedPositionForPositionInLevel (mostRecentPlayerPos);
			spawnFloorPingEffectAtPositionWithColor (gridAlignedPos, Random.ColorHSV());
			GameObject pup = GetComponent<GGPowerupFactory>().instantiateRandomPowerup();
			pup.transform.position = gridAlignedPos + Vector3.up;
		}
		#endif
//		illuminateFloorAtPositionsWithColor (fifoLastKnownPlayerPositions.ToArray (), Color.cyan);
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
		foreach (Vector3 item in arrayPositions) {
			floorIllumController.illuminateGridAtPositionWithColor(item.x+25.0f,item.z+25.0f,illuminationColor);	
		}
	}

	public Vector3 gridAlignedPositionForPositionInLevel(Vector3 position) {
		Vector2 numGridTiles = floor.GetComponent<Renderer> ().material.mainTextureScale;
		float sizePerTileX = floor.transform.localScale.x/numGridTiles.x;
		float sizePerTileY = floor.transform.localScale.y/numGridTiles.y;
		int xGridPosition = (int)((position.x+25.0f)/sizePerTileX);
		int yGridPosition = (int)((position.z+25.0f)/sizePerTileY);
		return new Vector3 (sizePerTileX * xGridPosition - 25.0f + sizePerTileX * 0.5f, position.y, sizePerTileY * yGridPosition - 25.0f + sizePerTileY * 0.5f);
	}

	public void spawnFloorPingEffectAtPositionWithColor(Vector3 position,Color pingColor) {
		if (isInsideLevel(position)) {
			Vector3 gridAlignedPos = gridAlignedPositionForPositionInLevel (position);
			GameObject pingEffect = Instantiate (floorPingPrefab);
			//place ping effect .1f above floor
			pingEffect.transform.position = new Vector3 (gridAlignedPos.x, floor.transform.position.y + 0.1f, gridAlignedPos.z);
			Destroy(pingEffect,10.0f);
		}
	}
}
