using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : Projectile {

	//ringvine reads the path object children and goes to each one down the list
	public GameObject pathObjParent;
	bool canMoveToNext = false;
	int index = 0;

	List<GameObject> pathObjList = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		if (pathObjParent != null) 
		{
			for (int i = 0; i < pathObjParent.transform.childCount; i++) 
			{
				GameObject nextPathObj = pathObjParent.transform.GetChild (i).gameObject;
				pathObjList.Add (nextPathObj);
				nextPathObj.GetComponent<SpriteRenderer> ().enabled = false;
			}
		} 
		else 
		{
			Debug.LogError ("No path for Ring Projectile is set!");
		}

		base.isDestroyOnImpact = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//TODO: check if battle intro is done before moving
		if (!BattleController.instance.isLevelReadyToStart)
			return;
		
		MoveToNextPathObj ();
	}

	void MoveToNextPathObj ()
	{
		if (pathObjList.Count > 0) 
		{
			GameObject pathObj = pathObjList [index];
			float distToPoint = ((transform.position - pathObj.transform.position).magnitude);
			if (distToPoint > 0) 
			{
				canMoveToNext = false;
				distToPoint = ((transform.position - pathObj.transform.position).magnitude);
				transform.position = Vector2.MoveTowards (transform.position, pathObj.transform.position, base.velocity * Time.deltaTime);
				if (distToPoint <= 0.05f) 
				{
					canMoveToNext = true;
				}
			}
			if (canMoveToNext) 
			{
				index = (index + 1) % (pathObjList.Count);
				pathObj = pathObjList [index];
			}
		}
	}
}
