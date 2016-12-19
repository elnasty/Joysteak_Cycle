using UnityEngine;
using System.Collections;

public class PetalMovement1 : MonoBehaviour
{
    public float duration;
    public float maxspeed;
    private float speed;
    private float timesince = 0.0f;
    private float distance;
    private float maxdistance;

    private Vector2 parent;
    private Vector2 self;

    void Start()
    {
        parent = (Vector2)transform.parent.transform.position;
        self = (Vector2)transform.position;
        distance = (self - parent).magnitude;
        maxdistance = -distance;
    }

    void FixedUpdate()
    {
        parent = (Vector2)transform.parent.transform.position;
        self = (Vector2)transform.position;

        timesince += Time.fixedDeltaTime;
        speed = (float)Mathf.SmoothStep(0.0f, maxspeed, timesince / duration);
        distance = (float)Mathf.SmoothStep(0.0f, maxdistance, timesince / duration);

        if (timesince <= duration)
        {
            transform.position = Vector3.MoveTowards(transform.position, parent, speed * distance * Time.fixedDeltaTime);

        }
        else if (timesince > duration)
        {
            timesince = 0.0f;
            maxdistance = -maxdistance;
        }
    }
}