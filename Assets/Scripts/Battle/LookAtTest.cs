using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTest : MonoBehaviour
{
    public Transform target;
    float angle;
    
    void Update()
    {
        var newRotation = Quaternion.LookRotation(transform.position - target.position, transform.up);
        newRotation.x = 0.0f;
        newRotation.y = 0.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 1);

        
        angle = Mathf.Atan2(transform.position.y - target.transform.position.y, transform.position.x - target.transform.position.x) * Mathf.Rad2Deg;
    }
    /*void Update()
    {
        transform.LookAt(target, transform.forwar);
        transform.rotation = new Quaternion(0, 0, transform.rotation.z,transform.rotation.w);
    }*/
}
