using UnityEngine;
using System.Collections;

public class BackgroundBehavior : MonoBehaviour
{
    public GameObject BackgroundFront;
    public GameObject BackgroundBack;
    public GameObject Elliot;

	public bool isLevelReadyToStart = false;

    // Use this for initialization
    void Start()
    {
        BackgroundFront.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
        BackgroundBack.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
        Cursor.visible = false;

        StartCoroutine(FadeIn(BackgroundFront, 2f, 1f));
        StartCoroutine(FadeIn(BackgroundBack, 3f, 1f));
        StartCoroutine(FadeIn(Elliot, 4.5f, 1f));


		if (GameManager.instance.isMouseControl) {
		}

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
		if (gameObj.Equals (Elliot)) {
			Debug.Log ("Ready");
			isLevelReadyToStart = true; //true after the level loads and mini-cutscene at the start is done, 
			//controls whether the actual battle can start or not
		}
    }

    // Update is called once per frame
    void Update()
    {
    }
}

