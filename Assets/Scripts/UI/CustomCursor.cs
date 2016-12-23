using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour {

	public Camera cameraObj;
	private SpriteRenderer sprite;

	void Awake ()
	{
		Cursor.visible = false;
		sprite = transform.GetComponentInChildren <SpriteRenderer> ();
	}

	void Update () 
	{
		transform.position = (Vector2) cameraObj.ScreenToWorldPoint (Input.mousePosition);

		if (Input.GetMouseButtonDown (0)) 
		{
			sprite.color = new Color (158/255f, 158/255f, 158/255f);
			sprite.transform.localScale = new Vector2 (0.9f, 0.9f);
		}
		if (Input.GetMouseButtonUp (0)) 
		{
			sprite.color = new Color (255, 255, 255);
			sprite.transform.localScale = new Vector2 (1, 1);
		}
	}
}
