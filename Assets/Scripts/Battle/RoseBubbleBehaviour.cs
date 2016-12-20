using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseBubbleBehaviour : MonoBehaviour {

	public GameObject playerObj;
	public float triggerRadius;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float distance = (transform.position - playerObj.transform.position).magnitude;
		if (distance <= triggerRadius) {
			Debug.Log ("Rose bud activated");
		}
	}
}
