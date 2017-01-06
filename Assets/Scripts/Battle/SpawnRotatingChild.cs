using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRotatingChild : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		foreach (Transform child in transform)
        {
            child.transform.RotateAround(transform.position, Vector3.forward, 20 * Time.deltaTime);
        }
	}
}
