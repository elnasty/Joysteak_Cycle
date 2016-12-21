using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingVineMovement : MonoBehaviour {

	//ringvine reads the path object children and goes to each one down the list
	public GameObject pathObjParent;
	public float speed;
	bool canMoveToNext = false;
	int index = 0;

	List<GameObject> pathObjList = new List<GameObject>();
	Rigidbody2D rgbody;

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < pathObjParent.transform.childCount; i++)
		{
			pathObjList.Add (pathObjParent.transform.GetChild (i).gameObject);
		}

		rgbody = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveToNextPathObj ();

	}

	IEnumerator FadeInAndStart()
	{
		yield return null;
	}


	void MoveToNextPathObj ()
	{
		GameObject pathObj = pathObjList [index];
		float distToPoint = ((transform.position - pathObj.transform.position).magnitude);
		Vector2 direction = pathObj.transform.position - transform.position;
		if (distToPoint > 0) 
		{
			canMoveToNext = false;
			distToPoint = ((transform.position - pathObj.transform.position).magnitude);
			direction = pathObj.transform.position - transform.position;
			transform.position = Vector2.MoveTowards (transform.position, pathObj.transform.position, speed * Time.deltaTime);
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
