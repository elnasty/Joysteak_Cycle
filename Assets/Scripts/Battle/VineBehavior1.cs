using UnityEngine;
using System.Collections;

public class VineBehavior1 : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        StartCoroutine(PierceOverTime(this.gameObject, 1f, 1f));
    }

    IEnumerator PierceOverTime(GameObject gameObj, float time, float newX)
    {
        Vector3 originalScale = GetComponent<Transform>().localScale;

        float currentTime = 0.0f;
        do
        {
            newX = Mathf.SmoothStep(originalScale.x, 1f, currentTime / time);
            gameObj.GetComponent<Transform>().localScale = new Vector3(newX, 1, 1);

            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }
}