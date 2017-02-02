using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingShield : MonoBehaviour {

	public float witherDuration;
	private bool isWithering = false;

	private SpriteRenderer ringSprite;
	private float currentColorVal;
	private float initialColorVal;
	private float timer = 0;

	void Start ()
	{
		ringSprite = GetComponent<SpriteRenderer> ();
		initialColorVal = ringSprite.color.r;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			GameObject player = other.gameObject;

			float distanceToCenter = (this.transform.position - player.transform.position).magnitude;
			float halfWidthOfShield = this.GetComponent<CircleCollider2D> ().bounds.size.magnitude / 4;
			bool isShieldCoveringPlayer = distanceToCenter < halfWidthOfShield;

			if (isShieldCoveringPlayer) 
			{
				if (!BattleController.instance.isPlayerInvulnerable) 
				{
					BattleController.instance.isPlayerInvulnerable = true;
				}
				if (!isWithering)
				{
					StartCoroutine (Wither ());
				}
			} 
			else 
			{
				if (BattleController.instance.isPlayerInvulnerable) 
				{
					BattleController.instance.isPlayerInvulnerable = false;
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			BattleController.instance.isPlayerInvulnerable = false;
		}
	}

	void Update() 
	{
		// For spinning effect
		transform.Rotate(0, 0, 50 * Time.deltaTime);
	}


	IEnumerator Wither()
	{
		isWithering = true;
		while (timer <= witherDuration)
		{
			timer += Time.fixedDeltaTime;
			currentColorVal = Mathf.Lerp (initialColorVal, 0, timer / witherDuration);
			ringSprite.color = new Color (currentColorVal, currentColorVal, currentColorVal, currentColorVal);
			yield return new WaitForFixedUpdate();
		}
		if (BattleController.instance.isPlayerInvulnerable) 
		{
			BattleController.instance.isPlayerInvulnerable = false;
		}
		Destroy (this.gameObject);
	}
}
