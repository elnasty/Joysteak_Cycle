using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	//public GameObject RingProjectile;

	public bool isLevelReadyToStart = false;


	public enum SpawnObjectEnum { thorn, vine1 };
	public List<List<GameObject>> pools = new List<List<GameObject>>();     //each list is an object pool for one
	//type of object.

	[Header("Pool Options")]
	public List<int> poolSizes;     //the size of each object pool.
	public List<bool> willGrow;     //whether or not the pools will grow to accomodate increasing demand for the object.
	public List<GameObject> objectPrefabs = new List<GameObject>();

	[Header("Pause/Cutscene Options")]

	private bool isPaused = false;
	private float PauseEndTime;

	void Awake()
	{
		if (instance == null)
			instance = this;

		for (int i = 0; i < Enum.GetNames(typeof(SpawnObjectEnum)).GetLength(0); i++)
		{
			pools.Add(new List<GameObject>());
		}
		
	}

	// Use this for initialization
	void Start ()
    {
		
		InitialiseBattle ();
		Heart.GetComponent<Heart> ().Initialise ();


		//populate pools
		int i = 0;

		foreach (List<GameObject> pool in pools)
		{
			for (int j = 0; j < poolSizes[i]; ++j)
			{
				GameObject obj = (GameObject)Instantiate(objectPrefabs[i]);
				obj.SetActive(false);
				pool.Add(obj);
			}
			i++;
		}
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

		//StartCoroutine(FadeIn(RingProjectile,5f,1f));
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

		if (gameObj.Equals (Elliot)) 
		{
			Debug.Log ("Ready");
			isLevelReadyToStart = true; //true after the level loads and mini-cutscene at the start is done, 
			//controls whether the actual battle can start or not
		}
	}

	public void AffectPlayerHealth(int value)
	{
		Heart.GetComponent<Heart> ().HeartAffectHealth (value);
	}

	public GameObject GetPooledObject(SpawnObjectEnum objType)
	{
		int typeIndex = (int)objType;   //gets the index (in the enum) of the object type.

		List<GameObject> pool = pools[typeIndex];

        if (willGrow[typeIndex])
        {
            GameObject obj = (GameObject)Instantiate(objectPrefabs[typeIndex]);
            obj.SetActive(false);
            pool.Add(obj);
        }

        for (int i = 0; i < pool.Count; i++)
		{
			if (!pool[i].activeInHierarchy)
			{
				return pool[i];
			}
		}

		return null;
	}

	public void ReturnPooledObject(GameObject obj)
	{
		obj.transform.position = this.transform.position;
		obj.SetActive (false);
	}

	/// <summary>
	/// Pause methods
	/// </summary>
	public void TimedPause(float delay)
	{
		Time.timeScale = 0;
		PauseEndTime = Time.realtimeSinceStartup + delay;

		while (Time.realtimeSinceStartup < PauseEndTime)
		{
			;
		}
		Unpause();
	}

	public void Unpause()
	{
		Time.timeScale = 1;
	}

	private IEnumerator Slideshow(GameObject gameobject)
	{
		gameobject.SetActive(true);
		yield return null;
		TimedPause(3);
		gameobject.SetActive(false);
	}
}
