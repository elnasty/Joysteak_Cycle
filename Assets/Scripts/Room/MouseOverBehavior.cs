using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MouseOverBehavior : MonoBehaviour
{
    public GameObject TransitionCircle;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    IEnumerator OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(PierceOverTime(TransitionCircle, 3f, new Vector3(25f, 25f, 1f)));
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("Battle1");
        }
    }

    IEnumerator PierceOverTime(GameObject gameObj, float time, Vector3 destinationScale)
    {
        Vector3 originalScale = GetComponent<Transform>().localScale;

        float currentTime = 0.0f;
        do
        {
            gameObj.GetComponent<Transform>().localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);

            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }
}
