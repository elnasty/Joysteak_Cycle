using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoomController : MonoBehaviour {

	public static RoomController instance;
	public GameObject playerObject;
	public GameObject transitionCircle;
	public bool isPlayerMoving = false;
	public bool isInteractableSelected = false;
	public string sceneToLoad = "";

	private PlayerMovement player;

	void Start()
	{
		if (instance == null) instance = this;
		player = playerObject.transform.GetComponent<PlayerMovement> ();
	}

	void Update ()
	{
		if (GameManager.instance.isMouseControl) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				player.MoveToMouseClick ();
			}

			if (isPlayerMoving == false && isInteractableSelected == true) 
			{
				StartCoroutine(LoadBattleScene(sceneToLoad));
				isInteractableSelected = false;
			}
		}
		else
		{
			player.MoveByWASD ();
		}
	}


	IEnumerator LoadBattleScene(string sceneName)
	{
		StartCoroutine(ExpandTransitionCircle(transitionCircle, 3f, new Vector2(25f, 25f)));
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(sceneName);
	}


	IEnumerator ExpandTransitionCircle(GameObject gameObj, float time, Vector2 destinationScale)
	{
		Vector2 originalScale = GetComponent<Transform>().localScale;
		float currentTime = 0.0f;
		do
		{
			gameObj.GetComponent<Transform>().localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= time);
	}

}
