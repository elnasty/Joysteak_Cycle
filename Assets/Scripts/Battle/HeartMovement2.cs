using UnityEngine;
using System.Collections;

public class HeartMovement2 : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector2 direction;

    public float speed = 0;
    public float speedMax;
    public float acceleration;
    private float deceleration;
	public float knockback;
	bool isKnockedBack = false;
	Rigidbody2D rgbody;

    void Start()
    {
        deceleration = -acceleration * 5;
		rgbody = GetComponent<Rigidbody2D> ();
    }

	void OnCollisionEnter2D(Collision2D coll){
		//bounce heart back in the opposite direction
		//KnockedBack();
	
	}

	void KnockedBack(){
		//rgbody.velocity = -direction*speed;
		speed = 0;
		//isKnockedBack = true;

		//Invoke ("EndKnockBack", 0.5f);
	}

	void EndKnockBack(){
		isKnockedBack = false;
	}


    // Update is called once per frame
    void FixedUpdate()
    {
		if (isKnockedBack) {
			return;
		}
        if (Input.GetMouseButton(0) )
        {
            // If mouse is left clicked, get direction
            mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            direction = (mousePosition - transform.position);

            // If magnitude of direction if above 1, normalize, otherwise don't
            if (direction.magnitude > 1)
                direction = (mousePosition - transform.position).normalized;

            if (speed < speedMax)
                speed = speed + acceleration * Time.deltaTime;
        }
        else
        {
            if (speed > 0)
                speed = speed + deceleration * Time.deltaTime;
            else
                speed = 0;
        }


		speed = Mathf.Clamp (speed, 0, speedMax);
		GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + speed * direction * Time.deltaTime);

		
    }
}
