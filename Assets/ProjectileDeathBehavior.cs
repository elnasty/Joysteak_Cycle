using UnityEngine;
using System.Collections;

public class ProjectileDeathBehavior : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Heart")
        {
            Destroy(this.gameObject);
        }
    }
}
