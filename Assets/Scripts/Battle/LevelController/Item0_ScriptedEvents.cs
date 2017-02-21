using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item0_ScriptedEvents : MonoBehaviour 
{
	GameObject firstBg;
	GameObject secondBg;
	Vector2 firstBgStartPos;
	Vector2 secondBgStartPos;
	float bgWidth;
	float startTime;
	float scrollSpeed = 4;
	bool shouldBgScroll = false;

	void Start () 
	{
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
		if (shouldBgScroll == true) 
		{
			float newPosition = Mathf.Repeat ((Time.time - startTime) * scrollSpeed, bgWidth);
			firstBg.transform.position = firstBgStartPos + Vector2.left * newPosition;
			secondBg.transform.position = secondBgStartPos + Vector2.left * newPosition;
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
			Debug.Log ("Item0 Seq0 Start Event End");
			BattleController.instance.isLevelReadyToStart = true;
			shouldBgScroll = true;
			startTime = Time.time;
		}
	}
}
