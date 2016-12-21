using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseBubbleBehaviour : MonoBehaviour {

	public GameObject playerObj;
	public float triggerRadius;
	public float timerTriggerValue;
	float timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float distance = (transform.position - playerObj.transform.position).magnitude;
		if (distance <= triggerRadius) 
		{
			timer += Time.deltaTime;
			if (timer >= timerTriggerValue) 
			{
				Debug.Log ("Rose bud activated");
			}
		} else 
		{
			timer = 0; //reset timer if no longer within range
		}
	}
}
