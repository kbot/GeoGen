﻿using UnityEngine;
using System.Collections;
using System.Xml;

public class GGWaveManager : Singleton<GGWaveManager> {

	public GGEnemy [] SpawnablePrefabs;

	public float fDeltaBetweenSpawns;
	private float fRunningTime;

	private int nWaveCount = 0;
	private int nObjectsSpawned = 0;

	bool bEnableSpawning = false;

	XmlReader waveDescriptor;

	// Use this for initialization
	void Start () {
		KBFileManager.Instance.Initialize ();
		waveDescriptor = KBFileManager.Instance.readerForXMLDoc ("Scripts/waves.xml");
	}
	
	// Update is called once per frame
	void Update () {

		#if DEBUG
		if (Input.GetKeyDown(KeyCode.L)) {
			Debug.Log ("enabled: " + enabled);
			bEnableSpawning = !bEnableSpawning;
		}
		int indexForDebugSpawn = -1;
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			indexForDebugSpawn = 1;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			indexForDebugSpawn = 2;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			indexForDebugSpawn = 3;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			indexForDebugSpawn = 4;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			indexForDebugSpawn = 5;
		}
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			indexForDebugSpawn = 6;
		}
		if (indexForDebugSpawn > 0) {
			Vector3 randPos = GGLevelManager.Instance.getRandomLevelPosition() + Vector3.up * 5;
			GGEnemy newEnemy = (GGEnemy)Instantiate(SpawnablePrefabs[indexForDebugSpawn-1],randPos,Quaternion.identity);
			newEnemy.setTarget(GGGameManager.Instance.Player);
		}
		#endif
		if (bEnableSpawning) {
			fRunningTime += Time.deltaTime;
			if (fRunningTime / fDeltaBetweenSpawns > nWaveCount) {
				//Spawn Objects
				nWaveCount++;
				SpawnWave(nWaveCount);
			}
		}

	}
	 		
	void SpawnWave(int wave){
		if (waveDescriptor.ReadToFollowing ("Wave") || waveDescriptor.Name == "Wave") {
			//int waveContent = XmlConvert.ToInt32(waveDescriptor.GetAttribute("count"));
			int numEnemies, indexPrefabToSpawn;
			Vector3 enemyPositionMod = Vector3.zero;
			waveDescriptor.ReadToDescendant("Enemy");
			do {
				numEnemies = XmlConvert.ToInt32(waveDescriptor.GetAttribute("count"));
				indexPrefabToSpawn = getIndexForPrefabFromEnemyType(waveDescriptor.GetAttribute("type"));
				Debug.Log ("Spawning " + numEnemies + " Enemies at index: " + indexPrefabToSpawn);	
				for (int i = 0; i < numEnemies; i++) {
					//TODO radnomize location based on the level size
					enemyPositionMod.x = Random.Range(-20.0f,20.0f);
					enemyPositionMod.z = Random.Range(-20.0f,20.0f);
					enemyPositionMod.y = Random.Range(transform.position.y,transform.position.y + 5.0f);
					GGEnemy newEnemy = (GGEnemy)Instantiate(SpawnablePrefabs[indexPrefabToSpawn],transform.position + enemyPositionMod,Quaternion.identity);
					newEnemy.setTarget(GGGameManager.Instance.Player);
					nObjectsSpawned++;
				}
			} while (waveDescriptor.ReadToNextSibling("Enemy"));
		}

	}

	int getIndexForPrefabFromEnemyType(string type) {
		int returnIndex = 0;
		switch (type) {
		case "Cube": {returnIndex = 0;}break;
		case "Rect": {returnIndex = 1;}break;
		case "Cyl": {returnIndex = 2;}break;
		case "Cone": {returnIndex = 3;}break;
		case "Pyr": {returnIndex = 4;}break;
		case "d20": {returnIndex = 5;}break;
		default:
					break;
		}

		return returnIndex;
	}
}
