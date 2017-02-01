using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public int heartEffectValue;
	public bool isDestroyOnImpact;
	public float velocity;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" && !BattleController.instance.isPlayerInvulnerable) 
		{
			//hurt/heal player accordingly
			BattleController.instance.AffectPlayerHealth(heartEffectValue);

			if (isDestroyOnImpact) 
			{
				BattleController.instance.ReturnPooledObject(this.gameObject);
			}
		}
	}

	void ReturnPool()
	{
		BattleController.instance.ReturnPooledObject(this.gameObject);
		CancelInvoke ();
	}

	void OnBecameInvisible() 
	{
		ReturnPool ();
	}
}
