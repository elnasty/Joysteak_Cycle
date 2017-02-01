using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingShield : MonoBehaviour {
	
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

}
