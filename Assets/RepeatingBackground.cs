using UnityEngine;
using System.Collections;

public class RepeatingBackground : MonoBehaviour
{
    public float distance;
    public Transform player;

    private Vector3 shiftahead;

    // Use this for initialization
    void Start ()
    {
        shiftahead = new Vector3(distance * 2, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x - transform.position.x > distance)
        {
            transform.position += shiftahead;
        }
    }
}

