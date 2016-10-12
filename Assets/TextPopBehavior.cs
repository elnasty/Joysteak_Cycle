using UnityEngine;
using System.Collections;

public class TextPopBehavior : MonoBehaviour {

    public float textboxspeed;

    private Vector3 target;

    private bool onscreen = false;

    private float displacement;

    public GameObject roomlit;
    private bool lit = false;

    public GameObject Player;
    public GameObject dialoguebox;
    private bool dialogue = false;
    private int repeat = -1;
    public GameObject neutral;
    public GameObject angry;
    public GameObject sad;

    // Update is called once per frame
    void FixedUpdate ()
    {         
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (onscreen == false && displacement == 0)
            {
                target = transform.position + new Vector3(0,3,0);
                StartCoroutine(Slide(target));
                onscreen = true;
            }
            else if (onscreen == true && displacement == 0)
            {
                target = transform.position - new Vector3(0, 3, 0);
                StartCoroutine(Slide(target));
                onscreen = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (lit == false)
            {
                roomlit.SetActive(true);
                lit = true;
            }
            else if (lit == true)
            {
                roomlit.SetActive(false);
                lit = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (dialogue == false)
            {
                dialoguebox.SetActive(true);
                dialogue = true;
                //repeat += 1;
            }
            else if (dialogue == true)
            {
                dialoguebox.SetActive(false);
                dialogue = false;
            }
        }

        switch (repeat)
        {
            case -1:
                angry.SetActive(false);
                sad.SetActive(false);
                break;
            case 0:
                angry.SetActive(false);
                sad.SetActive(false);
                break;
            case 1:
                neutral.SetActive(false);
                sad.SetActive(true);
                break;
            default:
                sad.SetActive(false);
                angry.SetActive(true);
                break;
        }
    }


    private IEnumerator Slide(Vector3 target)
    {
        displacement = Vector3.Distance(target, transform.position);
        while (displacement > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, textboxspeed * Time.fixedDeltaTime);
            displacement = Vector3.Distance(target, transform.position);
            yield return null;
        }
    }

}
