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
	private bool isKnockedBack = false;

	public int darkening = 0;
	public int darkeningMax = 5;
	private float colorFloat;

	private int pulsingLevel;
	public int pulsingLevelReq;
	public int levelPassReq;

	private bool isHitBefore = false;
	private float prevBeatTime = 1f;
	private float currentBeatTime = 1f;

	public GameObject HeartBeat;
	public GameObject HeartOverlay;

    void Start()
    {
        deceleration = -acceleration * 5;
    }


	public void Initialise()
	{
		StartCoroutine(Blink(transform, false));
		StartCoroutine(MoveTo(1f));
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		speed = 0;
	}


	void Update()
	{
		// Darkening
		colorFloat = (float)(darkeningMax - darkening) / (float)darkeningMax;

		// Faster pulsing
		if (isHitBefore && currentBeatTime != prevBeatTime) 
		{
			prevBeatTime = currentBeatTime;
			CancelInvoke ("InvokeBeatCoroutine");
			InvokeRepeating ("InvokeBeatCoroutine", 0f, currentBeatTime * 2);
		}
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


	// Coroutine for 2 things, Pulsing Heart and Pulsing Light
	IEnumerator Beat()
	{
		float scale;
		float scaleStart = 0.3f;
		float scaleEnd = 0.32f;

		float opacity;
		float time = currentBeatTime;
		float currentTime = 0.0f;

		do
		{
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


	IEnumerator BeatOverlay(float maxScale)
	{
		float currentTime = 0f;
		float endTime = 0.7f;
		float initialAlpha = 0.2f;
		SpriteRenderer heartOverlaySprite = HeartOverlay.GetComponent<SpriteRenderer> ();
		heartOverlaySprite.color = new Vector4 (1, 1, 1, initialAlpha);

		while (currentTime <= endTime)
		{
			currentTime += Time.fixedDeltaTime;
			float currentAlpha = Mathf.Lerp (initialAlpha, 0, currentTime / endTime);
			heartOverlaySprite.color = new Color (1, 1, 1, currentAlpha);
			float scale = Mathf.Lerp (1, maxScale, currentTime / endTime);
			HeartOverlay.transform.localScale = new Vector3 (scale, scale, 1);
			yield return new WaitForFixedUpdate();
		}

		HeartOverlay.transform.localScale = new Vector3 (1, 1, 1);
		heartOverlaySprite.color = new Vector4 (1, 1, 1, 0);
	}


    // Update is called once per frame
    void FixedUpdate()
    {
		if (isKnockedBack) {
			return;
		}
		if (Input.GetMouseButton(0) && BattleController.instance.isLevelReadyToStart)
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


	// Testing function to move Casey by keyboard controls
	void MoveByWASD()
	{
		float movex = Input.GetAxisRaw("Horizontal");
		float movey = Input.GetAxisRaw("Vertical");
		Vector2 movement = new Vector2 (movex, movey);
		GetComponent<Rigidbody2D>().AddForce (movement * 500);
	}


	public void HeartAffectHealth(int value)
	{
		if (darkening < darkeningMax && value > 0 || darkening > 0 && value < 0) 
			darkening += value;

		// When player is taking damage
		if (value > 0) 
		{
			if (!isHitBefore) isHitBefore = true;
			InvokeBeatCoroutine ();
			StartCoroutine (Blink (transform, false));
			if (currentBeatTime <= 0.1f) {
				currentBeatTime = 0.1f;
			} else {
				currentBeatTime = currentBeatTime - 0.1f;
				StartCoroutine (BeatOverlay (8));
				BattleController.instance.StartRippleEffect ();
			}
		}

		if (darkening >= darkeningMax)
			BattleController.instance.ExecutePlayerDeathSequence ();
	}


	void InvokeBeatCoroutine()
	{
		StartCoroutine (Beat ());
	}
}
