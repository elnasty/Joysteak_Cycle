using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public int heartEffectValue;
	public bool isDestroyOnImpact;
	public float velocity;

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			//hurt/heal player accordingly
			BattleController.instance.AffectPlayerHealth(heartEffectValue);

			if (isDestroyOnImpact) 
			{
				BattleController.instance.ReturnPooledObject(this.gameObject);
			}

		}
	}
}
