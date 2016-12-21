using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

    public float movespeed;
	public Camera cameraObj;
	public List<GameObject> wayPoints;
    private float moveY;
    private float moveX;
	private bool isMoving;
	private bool isMouseInterrupt;
	private Vector2 mouseClickPos;
	private Vector2 diffToMousePos;

	enum Direction { None = 0, Down = 1, Up = 2, Right = 3, Left = 4 };

    void Update()
    {
		if (GameManager.instance.isMouseControl) 
		{
			ListenToMouseClick ();
		} 
		else 
		{
			ListenToWASD ();
		}
    }

	void ListenToMouseClick()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			mouseClickPos = cameraObj.ScreenToWorldPoint(Input.mousePosition);
			diffToMousePos = mouseClickPos - (Vector2) transform.position;
			if (Mathf.Abs (diffToMousePos.x) > Mathf.Abs (diffToMousePos.y)) 
			{
				if (diffToMousePos.x > 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Right);
				if (diffToMousePos.x < 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Left);
			}
			else if (Mathf.Abs (diffToMousePos.x) < Mathf.Abs (diffToMousePos.y)) 
			{
				if (diffToMousePos.y > 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Up);
				if (diffToMousePos.y < 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Down);
			}
			if (!isMoving) StartCoroutine ("MoveToMouseClick");
		}
	}

	IEnumerator MoveToMouseClick()
	{
		isMoving = true;
		float step = movespeed * Time.fixedDeltaTime;
		while (Vector2.Distance(mouseClickPos, transform.position) > 0.1f)
		{
			transform.position = Vector2.MoveTowards (transform.position, mouseClickPos, step);
			yield return new WaitForFixedUpdate();
		}
		GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.None);
		transform.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
		isMoving = false;
	}

	void ListenToWASD() 
	{
		moveX = Input.GetAxisRaw ("Horizontal");
		moveY = Input.GetAxisRaw ("Vertical");

		if (!UIController.instance.isShowingDialogueBox) 
		{
			if (moveY != 0 && moveX == 0) 
			{
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, moveY * movespeed);
				if (moveY > 0 && moveX == 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Up);
				if (moveY < 0 && moveX == 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Down);
			}
			else if (moveX != 0 && moveY == 0) 
			{
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveX * movespeed, 0);
				if (moveX > 0 && moveY == 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Right);
				if (moveX < 0 && moveY == 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Left);
			}
			else 
			{
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				if (moveX== 0 && moveY == 0) GetComponent<Animator> ().SetInteger ("Direction", 0);
			}
		} 
	}

}

