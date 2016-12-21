using UnityEngine;
using System.Collections;

public class ThornBehavior : MonoBehaviour
{
    public Transform target;
    public float velocity;

    private float turnspeed;
    private float homingSensitivity = 0.05f;
    private bool fire = false;

	// Use this for initialization
	void OnEnable ()
    {
        turnspeed = Random.Range(180f, 360f);
        StartCoroutine(Spin(3f, turnspeed));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (fire)
        {
            if (target != null)
            {
                Vector2 relativePos = target.position - transform.position;
                Debug.Log(relativePos);
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, relativePos);
                rotation.x = 0.0f;
                rotation.y = 0.0f;

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, homingSensitivity);
            }

            transform.Translate(Vector3.up * velocity * Time.deltaTime, Space.Self);
        }
    }

    IEnumerator Spin(float time, float turnspeed)
    {
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + turnspeed * time;
        float rotationZ;

        float currentTime = 0.0f;
        float lerpT;

        do
        {
            lerpT = currentTime / time;
            lerpT = 1f - Mathf.Cos(lerpT * Mathf.PI * 0.5f);

            rotationZ = Mathf.Lerp(startRotation, endRotation, lerpT);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotationZ);

            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime < time);

        fire = true;
    }
}
