using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public int heartEffectValue;
	public bool isDestroyOnImpact;
	// possible variables that can be inherited:
	//velocity

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player") {
			//hurt player accordingly
			BattleController.instance.AffectPlayerHealth(heartEffectValue);

			if (isDestroyOnImpact) {
				Destroy (this.gameObject); //change this to pool
			}

		}
	}
}
