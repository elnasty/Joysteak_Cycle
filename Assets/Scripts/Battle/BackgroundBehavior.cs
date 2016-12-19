using UnityEngine;
using System.Collections;

public class BackgroundBehavior : MonoBehaviour
{
    public GameObject BackgroundFront;
    public GameObject BackgroundBack;
    public GameObject Elliot;

    // Use this for initialization
    void Start()
    {
        BackgroundFront.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
        BackgroundBack.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
        Cursor.visible = false;

        StartCoroutine(FadeIn(BackgroundFront, 2f, 1f));
        StartCoroutine(FadeIn(BackgroundBack, 3f, 1f));
        StartCoroutine(FadeIn(Elliot, 4.5f, 1f));
    }

    IEnumerator FadeIn(GameObject gameObj, float delay, float time)
    {
        float currentTime = 0.0f;
        float opacity;

        yield return new WaitForSeconds(delay);

        do
        {
            opacity = Mathf.Lerp(0f, 1f, currentTime / time);
            gameObj.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, opacity);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }

    // Update is called once per frame
    void Update()
    {
    }
}

