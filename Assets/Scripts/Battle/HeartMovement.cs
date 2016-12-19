using UnityEngine;
using System.Collections;

public class HeartMovement : MonoBehaviour
{

    private float movex;
    private float movey;
    public float movespeed = 5.0f;

    void FixedUpdate()
    {

        movex = Input.GetAxisRaw("Horizontal");
        movey = Input.GetAxisRaw("Vertical");

        GetComponent<Rigidbody2D>().velocity = new Vector2(movex * movespeed, movey * movespeed);
    }
}
