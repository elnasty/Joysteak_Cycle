using UnityEngine;
using System.Collections;

public class TextPopBehavior : MonoBehaviour {

    public float textboxspeed;

    private Vector3 target;

    private bool onscreen = false;

    private float displacement;

    private Transform child;

    void Start()
    {
        child = transform.GetChild(0);
    }

    void FixedUpdate ()
    {         
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (onscreen == false && displacement == 0)
            {
                target = transform.position + new Vector3(0, 3, 0);
                StartCoroutine(Slide(target));
                onscreen = true;
                child.gameObject.SetActive(onscreen);
            }
            else if (onscreen == true && displacement == 0)
            {
                target = transform.position - new Vector3(0, 3, 0);
                StartCoroutine(Slide(target));
                onscreen = false;
                child.gameObject.SetActive(onscreen);
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
