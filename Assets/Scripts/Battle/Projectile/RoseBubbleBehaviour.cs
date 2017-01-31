using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseBubbleBehaviour : Projectile {

	[Header("Activate and Wilt Settings")]
	public float triggerRadius;
	public float activateAfterTime;
	public float wiltAfterTime;
	public float wiltDuration;
	public GameObject circle;

	[Header("Movement Settings")]
	public GameObject waypoint;

	private float timer;
	private float initialColorVal;
	private float currentColorVal;
	private bool roseBudActivated;
	private bool roseBudActivating;
	private bool roseBudWilted;
	private bool startWiltingInvoked;
	private bool roseBudWilting;
	private GameObject playerObj;
	private SpriteRenderer flowerSprite;
	private SpriteRenderer circleSprite;

	//TODO: Just for movement test. Movement could (should?) be determined by wave class?
	private bool isMoving = false;


	void Start ()
	{
		flowerSprite = GetComponent<SpriteRenderer> ();
		circleSprite = circle.GetComponent<SpriteRenderer> ();
		initialColorVal = flowerSprite.color.r;
		playerObj = BattleController.instance.Heart;
		roseBudActivating = false;
		roseBudActivated = false;
		roseBudWilted = false;
		roseBudWilting = false;
		startWiltingInvoked = false;
	}


	void Update () 
	{
		if (BattleController.instance.isLevelReadyToStart && !isMoving) 
		{	//TODO: Just for movement test. Movement could (should?) be determined by wave class?
			isMoving = true;
			StartCoroutine (MoveStraightToPos (waypoint.transform.position));
		}
		
		if (!BattleController.instance.isLevelReadyToStart) return;

		if (!roseBudActivated) ActivateOrWiltRose ();
		else DetectAndHealPlayer ();

		// For spinning effect
		transform.Rotate(0, 0, 50 * Time.deltaTime);
	}


	void DetectAndHealPlayer() 
	{
		if (base.heartEffectValue >= 0) base.heartEffectValue = base.heartEffectValue * -1;
		float distance = (transform.position - playerObj.transform.position).magnitude;
		if (distance <= triggerRadius) BattleController.instance.AffectPlayerHealth (base.heartEffectValue);
	}


	void ActivateOrWiltRose() 
	{
		float distance = (transform.position - playerObj.transform.position).magnitude;

		if (distance <= triggerRadius && !roseBudActivated && !roseBudWilting && !roseBudWilted) 
		{
			CancelInvoke ("StartWilting");
			startWiltingInvoked = false;
			if (timer >= activateAfterTime) ActivateRoseBud ();
			else ActivatingRoseBud ();
		}

		if (distance > triggerRadius && roseBudActivating && !roseBudWilting && !roseBudWilted) 
		{
			if (timer <= 0) DeactivateRoseBud ();
			else DeactivatingRoseBud ();
		}

		if (distance > triggerRadius && !roseBudActivated && !roseBudActivating) 
		{
			if (!roseBudWilting && !roseBudWilted && !startWiltingInvoked) 
			{
				Invoke ("StartWilting", wiltAfterTime);
				startWiltingInvoked = true;
			}
		}
	}


	void ActivateRoseBud() 
	{
		timer = 0;
		roseBudActivated = true;
		roseBudActivating = false;
		transform.localScale = new Vector2 (1.1f, 1.1f);
		circleSprite.color = new Color (1, 1, 0, 66/255f);
	}


	void ActivatingRoseBud() 
	{
		roseBudActivating = true;
		timer += Time.deltaTime;
		currentColorVal = Mathf.Lerp (initialColorVal, 1, timer / activateAfterTime);
		flowerSprite.color = new Color (currentColorVal, currentColorVal, currentColorVal);
	}


	void DeactivateRoseBud() 
	{
		roseBudActivated = false;
		roseBudActivating = false;
	}


	void DeactivatingRoseBud() 
	{
		timer -= Time.deltaTime;
		currentColorVal = Mathf.Lerp (initialColorVal, 1, timer / activateAfterTime);
		flowerSprite.color = new Color (currentColorVal, currentColorVal, currentColorVal);
	}


	void StartWilting() 
	{
		roseBudWilting = true;
		StartCoroutine (RoseWilt ());
	}


	IEnumerator RoseWilt()
	{
		roseBudWilting = true;
		timer = 0;
		while (timer <= wiltDuration)
		{
			timer += Time.fixedDeltaTime;
			currentColorVal = Mathf.Lerp (initialColorVal, 0, timer / wiltDuration);
			flowerSprite.color = new Color (currentColorVal, currentColorVal, currentColorVal, currentColorVal);
			circleSprite.color = new Color (currentColorVal, currentColorVal, currentColorVal, currentColorVal);
			yield return new WaitForFixedUpdate();
		}
		roseBudWilting = false;
		roseBudWilted = true;
		gameObject.SetActive(false);
	}


	//TODO: Just for movement test. Movement could (should?) be determined by wave class?
	IEnumerator MoveStraightToPos(Vector3 target)
	{
		float step = base.velocity * Time.fixedDeltaTime;
		while (Vector3.Distance(target, transform.position) > 0.1f)
		{
			transform.position = Vector3.MoveTowards (transform.position, target, step);
			yield return new WaitForFixedUpdate();
		}
	}
}
