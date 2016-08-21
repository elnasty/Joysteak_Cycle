using UnityEngine;
using System.Collections;

public class LevelBehavior : MonoBehaviour
{
    public Transform From;
    public Transform To;

    public float speed;

    private Vector2 Direction;
    private int degreesLeft;
	
	// Update is called once per frame
	void Update ()
    {
        Direction = (Vector2)(To.position - From.position);
        Direction.Normalize();

        transform.GetComponent<Rigidbody2D>().velocity = Direction * speed;

        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(Rotate(15));
	}

    private IEnumerator Rotate(int degrees)
    {
        degreesLeft = (int)(transform.rotation.z) / degrees;

        while (degreesLeft != 1)
        {
            print(degreesLeft);
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);
            degreesLeft = (int)(transform.rotation.z) / degrees;
            yield return null;
        }
    }
}
    