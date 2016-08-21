using UnityEngine;
using System.Collections;

public class BoundaryBehavior : MonoBehaviour {

    private Vector3 camerapos;
    public Transform spawn;

    private Vector2 direction;
    private Vector3 dotproduct;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        camerapos = Camera.main.transform.position;

        direction = spawn.position - transform.position;
        dotproduct = Vector3.Dot(camerapos, direction)*direction;
        dotproduct = Vector3.Normalize(dotproduct);

        transform.position = dotproduct;

        //Debug.Log(dotproduct);
    }
}
