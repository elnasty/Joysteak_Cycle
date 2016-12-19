using UnityEngine;
using System.Collections;

public class SpawnBehavior : MonoBehaviour {

    public Transform boundary;
    public GameObject prefab;
    public float distance;
    public float time;
    public float rate;

    public PauseBehavior Pause;

    private float yvalue = -6.0f;
    private float timesince = 0.0f;
    private float timesinceshot = 0.0f;

    private float realtimesincestart;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        timesince += Time.fixedDeltaTime;
        timesinceshot += Time.fixedDeltaTime;

        if(timesinceshot > rate)
        {
            timesinceshot = 0.0f;
            Instantiate(prefab, transform.position, Quaternion.identity);
        }

        if (timesince < time)
        {
            yvalue += distance * Time.fixedDeltaTime;
        }
        if (timesince > time)
        {
            timesince = 0.0f;
            distance = -distance;
        }

        transform.position = new Vector3(boundary.transform.position.x + 20, yvalue, 0);

	}
}
