using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRotatingChild : MonoBehaviour
{
    private float delay = 0.5f;
    private float time = 0.0f;
	
	void Update ()
    {
		foreach (Transform child in transform)
        {
            child.transform.RotateAround(transform.position, Vector3.forward, 20 * Time.deltaTime);
        }

        time += Time.deltaTime;
        if (time > delay)
        {
            time = 0.0f;
            foreach (Transform child in transform)
            {
                Fire(child);
            }
        }
        
	}

    void Fire(Transform child)
    {
        float angleZ;
        GameObject barb = BattleController.instance.GetPooledObject(BattleController.SpawnObjectEnum.barb);
        if (barb == null) return;

        barb.transform.position = child.position;
        angleZ = Mathf.Atan2(barb.transform.position.y - transform.position.y, barb.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        barb.transform.eulerAngles = new Vector3(0, 0, angleZ);
        barb.SetActive(true);
    }
}
