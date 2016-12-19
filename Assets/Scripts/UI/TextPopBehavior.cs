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

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            target = transform.position + new Vector3(0, 3, 0);
            StartCoroutine(Slide(target));
            child.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            target = transform.position - new Vector3(0, 3, 0);
            StartCoroutine(Slide(target));
            child.gameObject.SetActive(false);
        }
    }

    IEnumerator Slide(Vector3 target)
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
