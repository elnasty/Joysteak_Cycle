using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

	/// <summary>
	/// The Controller in battle scene.
	/// Has a reference to GameManager to update model
	/// Listens to player (heart) and projectiles 
	/// </summary>

	public static BattleController instance;

	public GameObject BackgroundFront;
	public GameObject BackgroundBack;
	public GameObject Elliot;
	public GameObject Heart;

	public bool isLevelReadyToStart = false;

	void Awake()
	{
		if (instance == null)
			instance = this;
		
	}

	// Use this for initialization
	void Start () {
		InitialiseBattle ();
		Heart.GetComponent<Heart> ().Initialise ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//InitialiseBattle //(mini-cutscenes, fade in bg and heart)
	void InitialiseBattle()
	{
		BackgroundFront.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
		BackgroundBack.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
		//Cursor.visible = false;

		StartCoroutine(FadeIn(BackgroundFront, 2f, 1f));
		StartCoroutine(FadeIn(BackgroundBack, 3f, 1f));
		StartCoroutine(FadeIn(Elliot, 4.5f, 1f));
	}

	IEnumerator FadeIn(GameObject gameObj, float delay, float time)
	{
		float currentTime = 0.0f;
		float opacity;

		yield return new WaitForSeconds(delay);

		do
		{
			opacity = Mathf.Lerp(0f, 1f, currentTime / time);
			gameObj.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, opacity);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= time);
		if (gameObj.Equals (Elliot)) {
			Debug.Log ("Ready");
			isLevelReadyToStart = true; //true after the level loads and mini-cutscene at the start is done, 
			//controls whether the actual battle can start or not
		}
	}

	//BackgroundScroll

	//MoveLevel //(move and rotate level accordingly)

	//Pause ??

	//GameObjectPooler

	//AffectHealth
}
