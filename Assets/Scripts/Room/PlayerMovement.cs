using UnityEngine;
using UnityEngine.AI;
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
	private List<Vector2> path = new List<Vector2>();
	private int currentPathStage = 0;

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

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Obstacle") 
		{
			StopMoving ();
			ContactPoint2D contact = other.contacts [0];
			Vector2 obstaclePos = other.transform.position;
			Vector2 contactNormal = contact.normal;
			if (contact.normal.y < 0) transform.position = (Vector2) transform.position - new Vector2 (0,0.1f);
			if (contact.normal.y > 0) transform.position = (Vector2) transform.position + new Vector2 (0,0.1f);
			if (contact.normal.x < 0) transform.position = (Vector2) transform.position - new Vector2 (0.1f,0);
			if (contact.normal.x > 0) transform.position = (Vector2) transform.position + new Vector2 (0.1f,0);
			path.Clear ();
			currentPathStage = 0;
		}
	}

	void StopMoving()
	{
		transform.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
		GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.None);
		isMoving = false;
	}

	void ListenToMouseClick()
	{
		if (Input.GetMouseButtonDown (0) && !isMoving) 
		{
			mouseClickPos = cameraObj.ScreenToWorldPoint(Input.mousePosition);
			diffToMousePos = mouseClickPos - (Vector2) transform.position;
			if (Mathf.Abs (diffToMousePos.x) < Mathf.Abs (diffToMousePos.y)) {
				path.Add (new Vector2 (mouseClickPos.x, transform.position.y));
				path.Add (new Vector2 (mouseClickPos.x, mouseClickPos.y));
			} else {
				path.Add (new Vector2 (transform.position.x, mouseClickPos.y));
				path.Add (new Vector2 (mouseClickPos.x, mouseClickPos.y));
			}
			StartCoroutine (MoveToPosition (path [currentPathStage]));
		}
	}

	IEnumerator MoveToPosition(Vector2 targetPos)
	{
		isMoving = true;
		float step = movespeed * Time.fixedDeltaTime;
		Vector2 diff = targetPos - (Vector2)transform.position;
		if (Mathf.Abs (diff.x) > Mathf.Abs (diff.y)) 
		{
			if (diff.x > 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Right);
			if (diff.x < 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Left);
		}
		else 
		{
			if (diff.y > 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Up);
			if (diff.y < 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.Down);
		}
		while (Vector2.Distance(targetPos, transform.position) > 0.1f)
		{
			transform.position = Vector2.MoveTowards (transform.position, targetPos, step);
			if (!isMoving) yield break;
			yield return new WaitForFixedUpdate();
		}
		StopMoving ();
		currentPathStage++;
		if (currentPathStage < path.Count) 
		{
			StartCoroutine (MoveToPosition(path[currentPathStage]));
		}
		else 
		{
			path.Clear();
			currentPathStage = 0;
		}
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
				if (moveX== 0 && moveY == 0) GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.None);
			}
		} 
	}

}

