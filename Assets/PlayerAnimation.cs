using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    //Variables
    private float movey;
    private float movex;

    // Update is called once per frame
    void Update()
    {
        movey = Input.GetAxisRaw("Horizontal");
        movex = Input.GetAxisRaw("Vertical");

        // 1 = down, 2 = up, 3 = right, 4 = left
        if (movex > 0 && movey == 0)
        {
            GetComponent<Animator>().SetInteger("Direction", 2);
        }
        else if (movex < 0 && movey == 0)
        {
            GetComponent<Animator>().SetInteger("Direction", 1);
        }
        else if (movey > 0 && movex == 0)
        {
            GetComponent<Animator>().SetInteger("Direction", 3);
        }
        else if (movey < 0 && movex == 0)
        {
            GetComponent<Animator>().SetInteger("Direction", 4);
        }
        else
        {
            GetComponent<Animator>().SetInteger("Direction", 0);
        }
    }
}
