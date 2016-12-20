using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float movespeed;
    private float moveY;
    private float moveX;

    void Update()
    {
		if (GameManager.instance.isMouseControl) 
		{
			Debug.Log ("mouse control");
		}
		else 
		{
			moveX = Input.GetAxisRaw ("Horizontal");
			moveY = Input.GetAxisRaw ("Vertical");

			if (!UIController.instance.isShowingDialogueBox) 
			{
				if (moveY != 0 && moveX == 0) 
				{
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, moveY * movespeed);
				}
				else if (moveX != 0 && moveY == 0) 
				{
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveX * movespeed, 0);     
				}
				else 
				{
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				}
			} 
			else 
			{
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			}
		}
    }
}

