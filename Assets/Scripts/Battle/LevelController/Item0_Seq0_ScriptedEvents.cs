using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item0_Seq0_ScriptedEvents : MonoBehaviour 
{
	Heart heart;

	GameObject firstBg;
	GameObject secondBg;
	GameObject elliot;

	SpawnStraightCorridor horizontalSpawn;
	SpawnDiagonalCorridor diagonalSpawn;
	SpawnNoCorridor noCorridorSpawn;
	SpawnHadoukenWave hadoukenSpawn;

	Vector2 firstBgStartPos;
	Vector2 secondBgStartPos;
	Vector2 elliotEndPos;
	Vector2 elliotStartPos;

	public float scrollSpeed = 4;
	public float bulletSpeed = 5;
	public float bulletMaxSpeed = 20;
	public float elliotFastSpeed = 1.3f;
	public float elliotSlowSpeed = 0.3f;

	float bgWidth;
	float startTime;
	float currentDarkening = 0f;
	int waveNo = 0;

	bool shouldBgScroll = false;
	bool isMovingElliot = false;
	bool isElliotMovingIn = true;
	bool isSequenceStarted = false;

	void Start () 
	{
		heart = BattleController.instance.heart.transform.GetComponent<Heart> ();

		elliot = BattleController.instance.elliot;
		elliotStartPos = elliot.transform.position;
		elliotEndPos = (Vector2) elliot.transform.position + new Vector2 (10, 0);

		GameObject corridorSpawn = elliot.transform.FindChild ("CorridorSpawn").gameObject; //Gameobject is specific only to this sequence
		horizontalSpawn = corridorSpawn.transform.GetComponent<SpawnStraightCorridor> ();
		diagonalSpawn = corridorSpawn.transform.GetComponent<SpawnDiagonalCorridor> ();
		noCorridorSpawn = corridorSpawn.transform.GetComponent<SpawnNoCorridor> ();
		hadoukenSpawn = corridorSpawn.transform.GetComponent<SpawnHadoukenWave> ();

		firstBg = BattleController.instance.backgroundFront;
		bgWidth = firstBg.transform.GetComponent<SpriteRenderer> ().bounds.size.x;
		firstBgStartPos = firstBg.transform.position;
		secondBg = GameObject.Instantiate (firstBg);
		secondBg.transform.position = new Vector2 (bgWidth + firstBgStartPos.x, firstBgStartPos.y);
		secondBgStartPos = secondBg.transform.position;

		InitialiseBattle ();
	}

	void Update() 
	{
		if (!BattleController.instance.isLevelReadyToStart) 
		{
			shouldBgScroll = false;
		}

		if (shouldBgScroll == true) 
		{
			float newPosition = Mathf.Repeat ((Time.time - startTime) * scrollSpeed, bgWidth);
			firstBg.transform.position = firstBgStartPos + Vector2.left * newPosition;
			secondBg.transform.position = secondBgStartPos + Vector2.left * newPosition;
		}
			
		// Elliot follows a basic movement pattern of moving in and out of screen
		if (!isMovingElliot && BattleController.instance.isLevelReadyToStart) 
		{
			if (isElliotMovingIn) {
				StartCoroutine (MoveElliot (elliotStartPos, elliotSlowSpeed));
				isElliotMovingIn = false;
			} else {
				StartCoroutine (MoveElliot (elliotEndPos, elliotSlowSpeed));
				isElliotMovingIn = true;
			}
		}

		// If player is hit, move Elliot out of screen quickly
		if (currentDarkening < heart.darkening || currentDarkening >= heart.darkeningMax && BattleController.instance.isLevelReadyToStart) 
		{
			currentDarkening = heart.darkening;
			StopAllCoroutines ();
			StartCoroutine (MoveElliot (elliotEndPos, elliotFastSpeed * 4));
			isElliotMovingIn = true;
			Debug.Log ("Item0 Seq0 Level Ending, Event Start");
		}
			
		if (BattleController.instance.isLevelReadyToStart && !isSequenceStarted) 
		{
			isSequenceStarted = true;
			InvokeRepeating ("ExecuteNextSpawnType", 0, 10);
		}

	}

	void ExecuteNextSpawnType()
	{
		if (waveNo >= 20) 
		{
			noCorridorSpawn.enabled = false;
			horizontalSpawn.enabled = false;
			diagonalSpawn.enabled = false;
			hadoukenSpawn.enabled = true;
			BattleController.instance.SetBulletSpeed (5, BattleController.SpawnObjectEnum.barb);
		} 
		else 
		{
			int spawnType = waveNo % 3;
			switch (spawnType) 
			{
			case 0:
				noCorridorSpawn.enabled = false;
				horizontalSpawn.enabled = true;
				if (waveNo != 0) 
				{
					bulletSpeed = bulletSpeed >= bulletMaxSpeed ? bulletSpeed : bulletSpeed + 1;
					BattleController.instance.SetBulletSpeed (bulletSpeed, BattleController.SpawnObjectEnum.barb);
				}
				break;
			case 1:
				horizontalSpawn.enabled = false;
				diagonalSpawn.enabled = true;
				break;
			case 2:
				diagonalSpawn.enabled = false;
				noCorridorSpawn.enabled = true;
				break;
			default:
				Debug.Log ("== Item0 Seq 0 SpawnType error ==");
				break;
			}
			waveNo++;
		}
	}
		
	void InitialiseBattle()
	{
		BattleController.instance.backgroundFront.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
		BattleController.instance.backgroundBack.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
		StartCoroutine(FadeIn(BattleController.instance.backgroundFront, 2f, 1f));
		StartCoroutine(FadeIn(BattleController.instance.backgroundBack, 3f, 1f));
		StartCoroutine(FadeIn(BattleController.instance.elliot, 4.5f, 1f));
	}

	IEnumerator FadeIn(GameObject gameObj, float delay, float time)
	{
		float currentTime = 0.0f;
		float opacity;

		yield return new WaitForSeconds(delay);
		if (!gameObj.activeSelf) 
		{
			gameObj.SetActive (true);
		}
		do
		{
			opacity = Mathf.Lerp(0f, 1f, currentTime / time);
			gameObj.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, opacity);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= time);

		if (gameObj.Equals (BattleController.instance.elliot)) 
		{
			Debug.Log ("Item0 Seq0 Level Beginning, Event End");
			BattleController.instance.isLevelReadyToStart = true;
			shouldBgScroll = true;
			startTime = Time.time;
			StartCoroutine (MoveElliot (elliotEndPos, elliotFastSpeed));
		}
	}

	IEnumerator MoveElliot(Vector2 target, float speed)
	{
		isMovingElliot = true;
		float step = speed * Time.deltaTime;
		while (Vector2.Distance(target, elliot.transform.position) > 0.1f)
		{
			elliot.transform.position = Vector2.MoveTowards (elliot.transform.position, target, step);
			yield return null;
		}
		isMovingElliot = false;
	}
}
