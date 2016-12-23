using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoomController : MonoBehaviour {

	public static RoomController instance;
	public Camera cameraObj;
	public GameObject playerObject;
	public GameObject transitionCircle;
	public bool isPlayerMoving = false;
	public Waypoint selectedInteractableWaypoint;
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
			// On mouse click, move player to mouse click position
			if (Input.GetMouseButtonDown (0)) 
			{
				Vector2 targetPos = cameraObj.ScreenToWorldPoint(Input.mousePosition);
				player.MovePlayerTo (targetPos);
			}

			// When player stops moving, check for following event(s)
			if (isPlayerMoving == false) 
			{
				// Event: If player is at selected waypoint attached to gameobject, starts dialogue and battle
				if (selectedInteractableWaypoint != null) 
				{
					Vector2 interactableWaypointPos = selectedInteractableWaypoint.gameObject.transform.position;
					Vector2 playerPos = player.gameObject.transform.position;
					if ((interactableWaypointPos - playerPos).magnitude <= 0.1) 
						StartCoroutine (LoadBattleScene (sceneToLoad));
				}

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
