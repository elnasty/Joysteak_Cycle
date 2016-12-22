using UnityEngine;
using System.Collections;

public class ThornBehavior : MonoBehaviour
{
    private GameObject target;
    public float velocity;

    private float turnspeed;
    private float homingSensitivity = 0.05f;
    private bool clockwise;
    private bool fire = false;

	// Use this for initialization
	void OnEnable ()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        clockwise = (Random.value > 0.5f);

        turnspeed = Random.Range(180f, 360f);

        if (clockwise) turnspeed = -turnspeed;

        StartCoroutine(Spin(3f, turnspeed));
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (fire)
        {
            if (target != null)
            {
                Vector2 relativePos = target.GetComponent<Transform>().position - transform.position;
                float angle = Vector2.Angle(transform.up, relativePos);
                float halfAngle;
                Vector3 cross = Vector3.Cross(transform.up, relativePos);

                if (cross.z < 0) angle = -angle;

                if ((angle > 0) == clockwise) halfAngle = 180 + (angle) / 2;
                else halfAngle = (angle) / 2;

                Vector3 halfPos = RotateVector2d(transform.up, halfAngle);
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, halfPos);
                rotation.x = 0.0f;
                rotation.y = 0.0f;

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, homingSensitivity);
            }

            transform.Translate(Vector2.up * velocity * Time.deltaTime, Space.Self);
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

    Vector2 RotateVector2d(Vector2 vec2, float degrees)
    {
        degrees *= Mathf.Deg2Rad;
        
        var x = vec2.x * Mathf.Cos(degrees) - vec2.y * Mathf.Sin(degrees);
        var y = vec2.x * Mathf.Sin(degrees) + vec2.y * Mathf.Cos(degrees);
        return new Vector2(x,y);
    }
}
