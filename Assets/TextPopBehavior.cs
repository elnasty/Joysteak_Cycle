using UnityEngine;
using System.Collections;

public class TextPopBehavior : MonoBehaviour {

    public float textboxspeed;

    private Vector3 offscreenpos = new Vector3(0f, -7.5f, -2f);
    private Vector3 onscreenpos = new Vector3(0f, -5.5f, -2f);

    private bool onscreen = false;

    private float displacement;
    


    // Update is called once per frame
    void FixedUpdate ()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (onscreen == false && displacement == 0)
            {
                StartCoroutine(Slide(onscreenpos));
                onscreen = true;
            }
            else if (onscreen == true && displacement == 0)
            {
                StartCoroutine(Slide(offscreenpos));
                onscreen = false;
            }
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
