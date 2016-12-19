using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    //Variables
    public float movespeed;
    private float movey;
    private float movex;

    void Update()
    {
        movey = Input.GetAxisRaw("Horizontal");
        movex = Input.GetAxisRaw("Vertical");
    }
    
    
    
    // Update is called once per frame
    void FixedUpdate()
    {

        if (movex != 0 && movey == 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, movex * movespeed);
        }
        else if (movey != 0 && movex == 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movey * movespeed, 0);     
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}

