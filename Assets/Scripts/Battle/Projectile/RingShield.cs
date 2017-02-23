using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingShield : MonoBehaviour {

	public float witherDuration;
	private bool isWithering = false;

	private SpriteRenderer ringSprite;
	private float currentColorVal;
	private float timer = 0;

	void OnEnable ()
	{
		ringSprite = GetComponent<SpriteRenderer> ();
		ringSprite.color = new Color (0, 0, 0, 0);
		StartCoroutine (Spawn ());
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
		
	public IEnumerator Wither()
	{
		isWithering = true;
		while (timer <= witherDuration)
		{
			timer += Time.fixedDeltaTime;
			currentColorVal = Mathf.Lerp (1, 0, timer / witherDuration);
			if (ringSprite!=null) ringSprite.color = new Color (currentColorVal, currentColorVal, currentColorVal, currentColorVal);
			yield return new WaitForFixedUpdate();
		}
		if (BattleController.instance.isPlayerInvulnerable) 
		{
			BattleController.instance.isPlayerInvulnerable = false;
		}
		isWithering = false;
		timer = 0;
		this.gameObject.SetActive (false);
	}

	public IEnumerator Spawn()
	{
		while (timer <= 1f)
		{
			timer += Time.fixedDeltaTime;
			currentColorVal = Mathf.Lerp (0, 1, timer / 1);
			ringSprite.color = new Color (currentColorVal, currentColorVal, currentColorVal, currentColorVal);
			yield return new WaitForFixedUpdate();
		}
		timer = 0;
	}
}
