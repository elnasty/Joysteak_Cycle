using UnityEngine;
using System.Collections;

public class PetalMovement2 : MonoBehaviour
{
    public float duration;
    public float maxspeed;
    private float speed;
    private float timesince = 0.0f;
    private float distance;
    private float maxdistance;

    private Vector2 parent;
    private Vector2 self;

    private Vector3 direction;
    public int right;

    void Start()
    {
        parent = (Vector2)transform.parent.transform.position;
        self = (Vector2)transform.position;
        distance = (self - parent).magnitude * 2;


        if (right == 1)
            direction = Vector3.forward;
        else
            direction = Vector3.back;

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
            transform.RotateAround(parent, direction, speed);
            transform.position = Vector3.MoveTowards(transform.position, parent, distance * Time.fixedDeltaTime);

        }
        else if (timesince > duration)
        {
            timesince = 0.0f;
            direction = -direction;
            maxdistance = -maxdistance;
        }
    }
}