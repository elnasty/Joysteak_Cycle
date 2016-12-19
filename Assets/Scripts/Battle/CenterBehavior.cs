using UnityEngine;
using System.Collections;

public class CenterBehavior : MonoBehaviour
{
    public Transform From;
    public Transform To;

    public float speed;
    // Update is called once per frame
    void Start ()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(speed , 0 , 0);
    }
}
