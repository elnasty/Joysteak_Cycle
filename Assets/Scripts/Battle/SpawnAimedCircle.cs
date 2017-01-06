using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAimedCircle : MonoBehaviour
{
    public int radius = 13;
    private int buffer = 30;
    private float angle;

    private float delay = 0.5f;
    private float time = 0.0f;
    private Vector3 circlePos;

    public Transform target;

    void Start()
    {
        angle = Random.value * 360;
    }

    // Use this for initialization
    void Update ()
    {
        time += Time.deltaTime;
        if (time > delay)
        {
            time = 0.0f;
            // calculate new angle with buffer to not be close to previous
            angle = Random.value * (360 - buffer*2) + angle + buffer;
            if (angle >= 360) angle -= 360;
            circlePos = CircleClock(new Vector2(0, 0), radius, angle);

            Fire(circlePos);
        }
    }

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
        float length;
        GameObject vine2 = BattleController.instance.GetPooledObject(BattleController.SpawnObjectEnum.vine2);
        if (vine2 == null) return;

        vine2.transform.position = position;

        angleZ = Mathf.Atan2(vine2.transform.position.y - target.transform.position.y, vine2.transform.position.x - target.transform.position.x) * Mathf.Rad2Deg;
        vine2.transform.eulerAngles = new Vector3(0, 0, angleZ);
        length = (new Vector2(target.transform.position.x, target.transform.position.y) - position).magnitude;
        vine2.GetComponent<Vine>().SetTargetXScale((length * 1.5f) / 12.16f);
        vine2.SetActive(true);
    }
}
