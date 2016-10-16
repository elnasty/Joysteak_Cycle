using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    //Variables
    private float movey;
    private float movex;

    public GameObject Photo;

    // Update is called once per frame
    void Update()
    {
        movey = Input.GetAxisRaw("Horizontal");
        movex = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.F))
        {
            movey = -0.01f;
            Invoke("Photos", 1f);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            movex = 0.01f;
        }

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

    void Photos()
    {
        Photo.SetActive(true);
    }
}
