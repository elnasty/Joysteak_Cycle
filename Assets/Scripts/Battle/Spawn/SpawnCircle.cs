using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircle : MonoBehaviour
{
    private int total = 36;
    private int count = 0;
    public int radius = 13;

    private float delay = 0.1f;
    private float time = 0.0f;
    private Vector3 circlePos;
    private Vector2 target = new Vector2(0, 0);

    void Update()
    {
        time += Time.deltaTime;
        if(time > delay)
        {
            time = 0.0f;
            circlePos = CircleClock(target, radius, count * 360/total);

            Fire(circlePos);
            count += 1;
            if (count == total) count = 0;

        }
    }

    // this shouldn't be here, to be moved to abstract class

    Vector3 CircleClock(Vector2 center, float radius, float angle)
    { // create random angle between 0 to 360 degrees 
        Vector2 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return pos;
    }

    void Fire(Vector2 position)
    {
        float angleZ;
        GameObject vine2 = BattleController.instance.GetPooledObject(BattleController.SpawnObjectEnum.vine2);
        if (vine2 == null) return;

        vine2.transform.position = position;

        angleZ = Mathf.Atan2(vine2.transform.position.y - target.y, vine2.transform.position.x - target.x) * Mathf.Rad2Deg;
        vine2.transform.eulerAngles = new Vector3(0, 0, angleZ);
       
        vine2.SetActive(true);
    }

}
