using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HeartBehavior : MonoBehaviour
{
    private float colourvalue = 1f;
    private float unTaint = 15f;

    void Start()
    {
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name != "flowercenter" && col.gameObject.name != "Boundary")
        {
            GetComponent<Transform>().localScale += new Vector3(0.02f, 0.02f, 1f);
            print(unTaint);      
        }
        else if(col.gameObject.name == "flowercenter" && col.gameObject.name != "Boundary")
        {
            unTaint += 5;
            if (unTaint >= 15)
            {
                unTaint = 15;
            }
        }
    }

    void Update()
    {
        colourvalue = unTaint / 15;

        if (colourvalue >= 1f)
        {
            colourvalue = 1f;
        }
        if (colourvalue <= 0f)
        {
            colourvalue = 0f;
        }

        GetComponent<SpriteRenderer>().color = new Color(colourvalue, colourvalue, colourvalue);

        if (colourvalue <= 0f)
        {
            print("failed bitch");
            SceneManager.LoadScene("Room");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Room");
        }
    }
}
