using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barb : Projectile
{
    private float lifeTime = 10f;

    // Use this for initialization
    void OnEnable ()
    {
        Invoke("ReturnPool", lifeTime);
    }

    void ReturnPool()
    {
        BattleController.instance.ReturnPooledObject(this.gameObject);
    }
    // Update is called once per frame
    void Update ()
    {
        transform.Translate(Vector2.up * base.velocity * Time.deltaTime, Space.Self);
    }
}
