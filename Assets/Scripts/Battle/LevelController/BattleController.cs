using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class BattleController : MonoBehaviour {

	/// <summary>
	/// The Controller in battle scene.
	/// Has a reference to GameManager to update model
	/// Listens to player (heart), Elliot and projectiles 
	/// </summary>

	[Header("Basic/Player Options")]
	public static BattleController instance;
	public GameObject cameraObj;
	public GameObject backgroundFront;
	public GameObject backgroundBack;
	public GameObject elliot;
	public GameObject heart;
	public bool isPlayerInvulnerable = false;

	[Header("Pool Options")]
	public List<int> poolSizes;     //the size of each object pool.
	public List<bool> willGrow;     //whether or not the pools will grow to accomodate increasing demand for the object.
	public List<GameObject> objectPrefabs = new List<GameObject>();
	public List<List<GameObject>> pools = new List<List<GameObject>>(); //each list is an object pool for type of object.
	public enum SpawnObjectEnum { thorn, vine1, vine2, barb };

	[Header("Pause/Cutscene Options")]
	public bool isLevelReadyToStart = false;
	private float PauseEndTime;

	[Header("Special Effects Options")]
	public ParticleSystem bulletDisintegrate;
	private RippleEffect ripple;

	void Update()
	{
		// TODO: To be removed - Quick restart level for QA testing
		if (Input.GetKeyDown (KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void Awake()
	{
		if (instance == null)
			instance = this;

		for (int i = 0; i < Enum.GetNames(typeof(SpawnObjectEnum)).GetLength(0); i++)
			pools.Add(new List<GameObject>());

		ripple = cameraObj.GetComponent<RippleEffect> ();
	}

	void Start ()
    {
		heart.GetComponent<Heart> ().Initialise ();
		PopulatePools ();
	}

	void PopulatePools() 
	{
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

	public void AffectPlayerHealth(int value)
	{
		heart.GetComponent<Heart> ().HeartAffectHealth (value);
	}

	public GameObject GetPooledObject(SpawnObjectEnum objType)
	{
		int typeIndex = (int)objType;   //gets the index (in the enum) of the object type.

		List<GameObject> pool = pools[typeIndex];

        for (int i = 0; i < pool.Count; i++)
		{
			if (!pool[i].activeInHierarchy)
			{
				return pool[i];
			}
		}

		if (willGrow[typeIndex])
		{
			GameObject obj = (GameObject)Instantiate(objectPrefabs[typeIndex]);
			obj.SetActive(false);
			pool.Add(obj);
			return obj;
		}

		return null;
	}

	public void ReturnPooledObject(GameObject obj)
	{
		if (this != null) 
		{
			obj.transform.position = this.transform.position;
			obj.SetActive (false);
		}
	}
		
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
		
	public void ExecutePlayerDeathSequence()
	{
		DestroyAllProjectilesWithEffect (bulletDisintegrate);
		isLevelReadyToStart = false;
	}

	public void SetBulletSpeed(float bulletSpeed, SpawnObjectEnum objType) 
	{
		List<GameObject> projectiles = BattleController.instance.pools [(int)objType];
		foreach (GameObject projectile in projectiles) 
		{
			projectile.GetComponent<Projectile> ().velocity = bulletSpeed;
		}
	}

	void DestroyAllProjectilesWithEffect(ParticleSystem vfx) 
	{
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag ("Projectile");
		foreach (GameObject projectile in projectiles) {
			MakeParticleEffect (vfx, projectile.transform.position);
			projectile.GetComponent<Projectile> ().ReturnPool ();
			isLevelReadyToStart = false;
		}
	}

	ParticleSystem MakeParticleEffect(ParticleSystem vfx, Vector3 position) 
	{
		ParticleSystem effect = Instantiate(vfx) as ParticleSystem;
		effect.transform.position = position;
		Destroy(effect.gameObject, effect.main.startLifetime.constant);
		return effect;
	}

	//TODO: What is this for?
	private IEnumerator Slideshow(GameObject gameobject)
	{
		gameobject.SetActive(true);
		yield return null;
		TimedPause(3);
		gameobject.SetActive(false);
	}

	public void StartRippleEffect()
	{
		ripple.shouldStartRipple = true;
		ripple.ripplePosition = heart.transform.position;
	}
}
