using UnityEngine;
using System.Collections;

public class Vine : Projectile
{
    // Use this for initialization
    void OnEnable()
    {
        StartCoroutine(PierceOverTime(this.gameObject, 1f, 1f));
        Invoke("Retract", 1.5f);
    }

    void Retract()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(PierceOverTime(this.gameObject, 0.1f, 1f));
            Invoke("Fire", 0.5f);
            Invoke("Fire", 0.75f);
            Invoke("Fire", 0.875f);
            Invoke("ReturnPool", 1f);
        }
    }

    void ReturnPool()
    {
        BattleController.instance.ReturnPooledObject(this.gameObject);
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