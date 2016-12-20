using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HeartBehavior : MonoBehaviour
{
    private int darkening = 0;
    private int darkeningMax = 5;
    private float colorFloat;

    private int pulsingLevel;
    public int pulsingLevelReq;
    public int levelPassReq;

    public GameObject HeartBeat;


    void Start()
    {
        StartCoroutine(Blink(transform, false));
        StartCoroutine(MoveTo(1f));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (darkening < darkeningMax)
        {
            darkening += 5;
            StartCoroutine(Beat(HeartBeat));
        }
        Debug.Log(darkening);
    }

    void Update()
    {
        // Darkening

        colorFloat = (float)(darkeningMax - darkening) / (float)darkeningMax;

        if(Input.GetMouseButtonDown(2))
        {
            darkening = 0;
        }

        // Pulsing

    }

    IEnumerator MoveTo(float time)
    {
        float currentTime = 0.0f;
        Vector2 startPos = GetComponent<Transform>().position;
        Vector2 endPos = new Vector2(-7.5f, 0);
        float xPos;

        yield return new WaitForSeconds(0.75f);

        do
        {
            xPos = Mathf.SmoothStep(startPos.x, endPos.x, currentTime / time);
            GetComponent<Transform>().position = new Vector2(xPos, endPos.y);

            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }

    IEnumerator Blink(Transform transform, bool b00l)
    {
        float startTime = Time.timeSinceLevelLoad;
        float currentTime;

        do
        {
            b00l = !b00l;
            transform.GetComponent<Renderer>().enabled = b00l;
            transform.GetChild(0).GetComponent<Renderer>().enabled = b00l;
            currentTime = Time.timeSinceLevelLoad - startTime;
            yield return new WaitForSeconds(0.15f);
        } while (currentTime <= 0.6f);

		transform.GetComponent<Renderer>().enabled = true;
		transform.GetChild(0).GetComponent<Renderer>().enabled = true;
    }

    // Coroutine for 2 things, Pulsing and Pulsing Light

    IEnumerator Beat(GameObject HeartBeat)
    {
        float scale;
        float scaleStart = 0.3f;
        float scaleEnd = 0.32f;

        float opacity;
        float time = 0.5f;
        float currentTime = 0.0f;

        float color;

        do
        {
            color = Mathf.Lerp(1f, colorFloat, currentTime / (time * 2));

            if (currentTime < time)
            {
                opacity = Mathf.Lerp(0f, 1f, currentTime / time);
                scale = Mathf.SmoothStep(scaleStart, scaleEnd, currentTime / time);
            }
            else
            {
                opacity = Mathf.Lerp(1f, 0f, (currentTime - time) / time);
                scale = Mathf.SmoothStep(scaleEnd, scaleStart, (currentTime - time) / time);
            }

            HeartBeat.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, opacity);
            GetComponent<Transform>().localScale = new Vector3(scale, scale, 1);
            GetComponent<SpriteRenderer>().color = new Vector4(colorFloat, colorFloat, colorFloat, 1);

            currentTime += Time.deltaTime;

            yield return null;

        } while (currentTime <= time*2);
    }
}
