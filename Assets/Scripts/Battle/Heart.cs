using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
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

	private int darkening = 0;
	private int darkeningMax = 5;
	private float colorFloat;

	private int pulsingLevel;
	public int pulsingLevelReq;
	public int levelPassReq;

	public GameObject HeartBeat;

    void Start()
    {
        deceleration = -acceleration * 5;
		rgbody = GetComponent<Rigidbody2D> ();
    }

	public void Initialise()
	{
		StartCoroutine(Blink(transform, false));
		StartCoroutine(MoveTo(1f));
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		//bounce heart back in the opposite direction
		//KnockedBack();
		speed = 0;

	}

	void Update()
	{
		// Darkening

		colorFloat = (float)(darkeningMax - darkening) / (float)darkeningMax;

		if(Input.GetMouseButtonDown(2))
		{
			darkening = 0;
		}

		// Pulsing

	}

	IEnumerator MoveTo(float time)
	{
		float currentTime = 0.0f;
		Vector2 startPos = GetComponent<Transform>().position;
		Vector2 endPos = new Vector2(-7.5f, 0);
		float xPos;

		yield return new WaitForSeconds(0.75f);

		do
		{
			xPos = Mathf.SmoothStep(startPos.x, endPos.x, currentTime / time);
			GetComponent<Transform>().position = new Vector2(xPos, endPos.y);

			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= time);
	}

	IEnumerator Blink(Transform transform, bool b00l)
	{
		float startTime = Time.timeSinceLevelLoad;
		float currentTime;

		do
		{
			b00l = !b00l;
			transform.GetComponent<Renderer>().enabled = b00l;
			transform.GetChild(0).GetComponent<Renderer>().enabled = b00l;
			currentTime = Time.timeSinceLevelLoad - startTime;
			yield return new WaitForSeconds(0.15f);
		} while (currentTime <= 0.6f);

		transform.GetComponent<Renderer>().enabled = true;
		transform.GetChild(0).GetComponent<Renderer>().enabled = true;
	}

	// Coroutine for 2 things, Pulsing and Pulsing Light

	IEnumerator Beat(GameObject HeartBeat)
	{
		float scale;
		float scaleStart = 0.3f;
		float scaleEnd = 0.32f;

		float opacity;
		float time = 0.5f;
		float currentTime = 0.0f;

		float color;

		do
		{
			color = Mathf.Lerp(1f, colorFloat, currentTime / (time * 2));

			if (currentTime < time)
			{
				opacity = Mathf.Lerp(0f, 1f, currentTime / time);
				scale = Mathf.SmoothStep(scaleStart, scaleEnd, currentTime / time);
			}
			else
			{
				opacity = Mathf.Lerp(1f, 0f, (currentTime - time) / time);
				scale = Mathf.SmoothStep(scaleEnd, scaleStart, (currentTime - time) / time);
			}

			HeartBeat.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, opacity);
			GetComponent<Transform>().localScale = new Vector3(scale, scale, 1);
			GetComponent<SpriteRenderer>().color = new Vector4(colorFloat, colorFloat, colorFloat, 1);

			currentTime += Time.deltaTime;

			yield return null;

		} while (currentTime <= time*2);
	}

//	void KnockedBack()
//	{
//		rgbody.velocity = (-direction*knockback);
//		speed = 0;
//		isKnockedBack = true;
//
//		Invoke ("EndKnockBack", 0.5f);
//	}

//	void EndKnockBack()
//	{
//		isKnockedBack = false;
//	}


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

		//uncomment for keyboard movement scheme
//		movex = Input.GetAxisRaw("Horizontal");
//		movey = Input.GetAxisRaw("Vertical");
//
//		GetComponent<Rigidbody2D>().velocity = new Vector2(movex * movespeed, movey * movespeed);

		
    }

	public void HeartAffectHealth(int value)
	{
		if (darkening < darkeningMax && value > 0 || darkening > 0 && value < 0)
		{
			darkening += value;
			StartCoroutine(Beat(HeartBeat));
		}
	}
}
