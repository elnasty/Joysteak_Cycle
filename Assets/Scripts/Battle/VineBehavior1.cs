using UnityEngine;
using System.Collections;

public class VineBehavior1 : Projectile
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightAlt))
            StartCoroutine(PierceOverTime(this.gameObject, 1f, 1f));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PierceOverTime(this.gameObject, 0.1f, 1f));
            Invoke("Fire", 0.25f);
            Invoke("Fire", 0.5f);
            Invoke("Fire", 0.75f);
        }
    }

    IEnumerator PierceOverTime(GameObject gameObj, float targetX, float time)
    {
        Vector3 originalScale = GetComponent<Transform>().localScale;

        float currentTime = 0.0f;
        float lerpT;

        do
        {
            lerpT = currentTime / time;
            lerpT = 1f - Mathf.Cos(lerpT * Mathf.PI * 0.5f);
            lerpT = lerpT * lerpT * lerpT * lerpT;

            var newX = Mathf.Lerp(originalScale.x, targetX, lerpT);
            gameObj.GetComponent<Transform>().localScale = new Vector3(newX, 1, 1);

            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }

    void Fire()
    {
        GameObject thorn = BattleController.instance.GetPooledObject(BattleController.SpawnObjectEnum.thorn);
        if (thorn == null) return;

        thorn.transform.position = transform.GetChild(0).position;
        thorn.SetActive(true);
    }
}