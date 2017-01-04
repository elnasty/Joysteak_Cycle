using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseBubbleBehaviour : MonoBehaviour {

	public float triggerRadius;
	public float timerTriggerValue;

	private float timer;
	private float initialColorVal;
	private float currentColorVal;
	private bool roseBudActivated;
	private bool roseBudActivating;
	private bool roseBudWilted;
	private GameObject playerObj;
	private SpriteRenderer sprite;

	void Start () 
	{
		sprite = GetComponent<SpriteRenderer> ();
		initialColorVal = sprite.color.r;
		playerObj = BattleController.instance.Heart;
		roseBudActivated = false;
	}
	
	void Update () 
	{
		float distance = (transform.position - playerObj.transform.position).magnitude;
		if (distance <= triggerRadius && !roseBudActivated && !roseBudWilted) {
			if (timer >= timerTriggerValue) {
				timer = 0;
				roseBudActivated = true;
				roseBudActivating = false;
			} else {
				roseBudActivating = true;
				timer += Time.deltaTime;
				currentColorVal = Mathf.Lerp (initialColorVal, 1, timer / timerTriggerValue);
				sprite.color = new Color (currentColorVal, currentColorVal, currentColorVal);
			}
		}
		if (distance > triggerRadius && roseBudActivating && !roseBudWilted) {
			if (timer <= 0) {
				roseBudActivated = false;
			} else {
				timer -= Time.deltaTime;
				currentColorVal = Mathf.Lerp (initialColorVal, 1, timer / timerTriggerValue);
				sprite.color = new Color (currentColorVal, currentColorVal, currentColorVal);
			}
		}
	}
}
