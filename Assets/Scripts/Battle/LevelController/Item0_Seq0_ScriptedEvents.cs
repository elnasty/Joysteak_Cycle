using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item0_Seq0_ScriptedEvents : MonoBehaviour 
{
	GameObject heart;

	GameObject firstBg;
	GameObject secondBg;
	GameObject elliot;
	GameObject[] ringShields;
	public GameObject rose;
	public GameObject corridorSpawn;

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
	float ringShieldStartPosX;
	float ringShieldRotOffset = 5;
	float nextCameraRotAngle = 0;
	int waveNo = 0;

	bool shouldBgScroll = false;
	bool isMovingElliot = false;
	bool isElliotMovingIn = true;
	bool isSequenceStarted = false;
	bool shouldPlayEndEvent = false;
	bool isPlayingEndEvent = false;

	void Start () 
	{
		heart = BattleController.instance.heart;

		elliot = BattleController.instance.elliot;
		elliotStartPos = elliot.transform.position;
		elliotEndPos = (Vector2) elliot.transform.position + new Vector2 (10, 0);

		horizontalSpawn = corridorSpawn.transform.GetComponent<SpawnStraightCorridor> ();
		diagonalSpawn = corridorSpawn.transform.GetComponent<SpawnDiagonalCorridor> ();
		noCorridorSpawn = corridorSpawn.transform.GetComponent<SpawnNoCorridor> ();
		hadoukenSpawn = corridorSpawn.transform.GetComponent<SpawnHadoukenWave> ();

		ringShields = noCorridorSpawn.ringShields;
		ringShieldStartPosX = ringShields [0].transform.position.x;

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
		ScrollBackground ();
		if (shouldStartSequence ()) StartSequence0 ();
		if (!shouldPlayEndEvent) ElliotSeqMovement ();
		if (isPlayerDead ()) EndSequence0 ();
		if (shouldPlayEndEvent && !isPlayingEndEvent) StartCoroutine (PlayEndEvent ());
	}

	bool shouldStartSequence()
	{
		return BattleController.instance.isLevelReadyToStart && !isSequenceStarted && !shouldPlayEndEvent;
	}

	bool isPlayerDead()
	{
		return !BattleController.instance.isLevelReadyToStart && isSequenceStarted && !shouldPlayEndEvent;
	}

	void StartSequence0()
	{
		isSequenceStarted = true;
		InvokeRepeating ("ExecuteNextSpawnType", 0, 10);
	}

	void EndSequence0()
	{
		CancelInvoke ();
		StopAllCoroutines ();
		HideAllRingShields ();
		horizontalSpawn.enabled = false;
		diagonalSpawn.enabled = false;
		noCorridorSpawn.enabled = false;
		hadoukenSpawn.enabled = false;
		StartCoroutine (RotateCamera ());
		StartCoroutine (MoveObject (elliot, elliotEndPos, elliotFastSpeed * 4));
		StartCoroutine (MoveObject (heart, new Vector2 (-5.5f, 0), elliotFastSpeed * 4));
	}
		
	void HideAllRingShields()
	{
		foreach (GameObject ringshield in noCorridorSpawn.ringShields)
		{
			float originalWitherDuration = ringshield.GetComponent<RingShield> ().witherDuration;
			ringshield.GetComponent<RingShield> ().witherDuration = 1;
			StartCoroutine (ringshield.GetComponent<RingShield> ().Wither ());
			ringshield.GetComponent<RingShield> ().witherDuration = originalWitherDuration;
		}
	}

	void ElliotSeqMovement()
	{
		// Elliot follows a basic movement pattern of moving in and out of screen
		if (!isMovingElliot && BattleController.instance.isLevelReadyToStart) 
		{
			if (isElliotMovingIn) {
				StartCoroutine (MoveObject (elliot, elliotStartPos, elliotSlowSpeed));
				isElliotMovingIn = false;
			} else {
				StartCoroutine (MoveObject (elliot, elliotEndPos, elliotSlowSpeed));
				isElliotMovingIn = true;
			}
		}
		// If player is hit, move Elliot out of screen quickly
		if (currentDarkening < heart.transform.GetComponent<Heart>().darkening || 
			currentDarkening >= heart.transform.GetComponent<Heart>().darkeningMax && 
			BattleController.instance.isLevelReadyToStart) 
		{
			currentDarkening = heart.transform.GetComponent<Heart>().darkening;
			StopCoroutine (MoveObject (elliot, new Vector2 (), 0));
			StartCoroutine (MoveObject (elliot, elliotEndPos, elliotFastSpeed * 4));
			isElliotMovingIn = true;
		}

		// CorridorSpawn gameobject sticks to Elliot
		corridorSpawn.transform.position = elliot.transform.position;
	}

	void ScrollBackground()
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
	}

	void ExecuteNextSpawnType()
	{
		if (waveNo >= 20) 
		{
			// Player too pro. Force the end of item 0 seq 0. Activate Hadouken Spawn
			noCorridorSpawn.enabled = false;
			horizontalSpawn.enabled = false;
			diagonalSpawn.enabled = false;
			hadoukenSpawn.enabled = true;
			BattleController.instance.SetBulletSpeed (5, BattleController.SpawnObjectEnum.barb);
		} 
		else // Just execute the next spawn while rotating camera
		{ 	
			if (waveNo != 0) 
			{
				nextCameraRotAngle = nextCameraRotAngle + 90;
				StartCoroutine (RotateCamera ());
				MoveRingShields (); // To move ringShields back into camera's view during camera rotation
			}

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
				Debug.Log ("Item0 Seq 0 SpawnType error");
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

	void MoveRingShields()
	{
		foreach (GameObject ringShield in ringShields) 
		{
			float nextPosX = waveNo % 2 != 0 ? ringShieldStartPosX + ringShieldRotOffset : ringShieldStartPosX;
			float posY = ringShield.transform.position.y;
			Vector2 target = new Vector2 (nextPosX, posY);
			float speed = elliotFastSpeed * 2;
			StartCoroutine (MoveObject (ringShield, target, speed));
		}
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
			StartCoroutine (MoveObject (elliot, elliotEndPos, elliotFastSpeed));
		}
	}

	IEnumerator RotateCamera()
	{
		while (Camera.main.transform.eulerAngles.z < nextCameraRotAngle - 1) 
		{
			heart.transform.Rotate (0, 0, 85 * Time.deltaTime); 
			elliot.transform.Rotate (0, 0, 50 * Time.deltaTime);
			Camera.main.transform.Rotate (0, 0, 50 * Time.deltaTime);
			yield return null;
		}
		nextCameraRotAngle = nextCameraRotAngle >= 360 ? 0 : nextCameraRotAngle;
		heart.transform.eulerAngles = new Vector3(0, 0, nextCameraRotAngle);
		elliot.transform.eulerAngles = new Vector3(0, 0, nextCameraRotAngle);
		Camera.main.transform.eulerAngles = new Vector3(0, 0, nextCameraRotAngle);
	}

	IEnumerator MoveObject(GameObject obj, Vector2 target, float speed)
	{
		if (obj.tag == "Player") BattleController.instance.isLevelReadyToStart = false;
		if (obj.tag == "Elliot") isMovingElliot = true;
		float step = speed * Time.deltaTime;
		while (Vector2.Distance (target, obj.transform.position) > 0.1f) 
		{
			obj.transform.position = Vector2.MoveTowards (obj.transform.position, target, step);
			yield return null;
		}
		if (obj.tag == "Player" && isPlayerDead ()) shouldPlayEndEvent = true;
		if (obj.tag == "Player") BattleController.instance.isLevelReadyToStart = true;
		if (obj.tag == "Elliot") isMovingElliot = false;
	}

	IEnumerator PlayEndEvent()
	{
		BattleController.instance.isLevelReadyToStart = false;
		isPlayingEndEvent = true;

		// Elliot moves into camera view
		isMovingElliot = true;
		Vector2 target = new Vector2 (5.5f, 0);
		while (Vector2.Distance (target, elliot.transform.position) > 0.1f) 
		{
			elliot.transform.position = Vector2.MoveTowards (elliot.transform.position, target, 4 * Time.deltaTime);
			yield return null;
		}
		isMovingElliot = false;

		// Elliot drops a rose projectile
		float timer = 0;
		float duration = 1;
		float currentColorVal = 0;
		rose.SetActive (true);
		SpriteRenderer roseSprite = rose.transform.GetComponent<SpriteRenderer> ();
		roseSprite.color = new Color (143/255f, 143/255f, 143/255f, 0);
		while (timer <= duration)
		{
			timer += Time.fixedDeltaTime;
			currentColorVal = Mathf.Lerp (0, 1, timer / duration);
			roseSprite.color = new Color (143/255f, 143/255f, 143/255f, currentColorVal);
			yield return null;
		}

		// Elliot circles around rose until it blooms
		timer = 0;
		duration = 3;
		while (timer <= duration)
		{
			timer += Time.fixedDeltaTime;
			currentColorVal = Mathf.Lerp (143/255f, 1, timer / duration);
			roseSprite.color = new Color (currentColorVal, currentColorVal, currentColorVal, 1);
			yield return null;
		}
		rose.transform.GetComponent<RoseBubbleBehaviour> ().ActivateRoseBud ();

		// Elliot leaves the stage
		isMovingElliot = true;
		target = elliotEndPos;
		while (Vector2.Distance (target, elliot.transform.position) > 0.1f) 
		{
			elliot.transform.position = Vector2.MoveTowards (elliot.transform.position, target, 4 * Time.deltaTime);
			yield return null;
		}
		isMovingElliot = false;

		BattleController.instance.isLevelReadyToStart = true;
	}
}
