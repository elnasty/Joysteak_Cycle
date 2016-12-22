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
	private bool isMouseInterrupt;
	private Vector2 mouseClickPos;
	private List<Vector2> path = new List<Vector2>();
	private int currentPathIndex = 0;

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
			// Stop moving everytime player hits an obstacle
			StopMoving ();

			// Finding direction of collision
			ContactPoint2D contact = other.contacts [0];
			Vector2 obstaclePos = other.transform.position;
			Vector2 contactNormal = contact.normal;

			// To prevent colliders from overlapping, add buffer distance between colliders
			if (contact.normal.y < 0) transform.position = (Vector2) transform.position - new Vector2 (0,0.1f);
			if (contact.normal.y > 0) transform.position = (Vector2) transform.position + new Vector2 (0,0.1f);
			if (contact.normal.x < 0) transform.position = (Vector2) transform.position - new Vector2 (0.1f,0);
			if (contact.normal.x > 0) transform.position = (Vector2) transform.position + new Vector2 (0.1f,0);

			path.Clear ();
			currentPathIndex = 0;
		}
	}

	void StopMoving()
	{
		StopCoroutine("MoveToPosition");
		transform.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
		GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.None);
	}

	void ListenToMouseClick()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			StopMoving ();
			path.Clear ();
			currentPathIndex = 0;
			mouseClickPos = cameraObj.ScreenToWorldPoint(Input.mousePosition);
			Vector2 diffToMousePos = mouseClickPos - (Vector2) transform.position;

			// Add the straight path with the smaller displacement into the path list first
			// TODO: Create right path from the start instead of dynamically changing list of paths
			if (Mathf.Abs (diffToMousePos.x) < Mathf.Abs (diffToMousePos.y))
			{
				path.Add (new Vector2 (mouseClickPos.x, transform.position.y));
				path.Add (new Vector2 (mouseClickPos.x, mouseClickPos.y));
			}
			else 
			{
				path.Add (new Vector2 (transform.position.x, mouseClickPos.y));
				path.Add (new Vector2 (mouseClickPos.x, mouseClickPos.y));
			}

			// Begin executing chain of paths
			StartCoroutine ("MoveToPosition");
		}
	}

	IEnumerator MoveToPosition()
	{
		float step = movespeed * Time.fixedDeltaTime;
		Vector2 targetPos = path[currentPathIndex];
		Vector2 diff = targetPos - (Vector2)transform.position;

		// Show walking animation for axis with the bigger displacement
		// i.e. More horizontal displacement = show Right/Left walking animation
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

		// Keep walking towards target until !isMoving or player reach it's destination
		while (Vector2.Distance(targetPos, transform.position) > 0.1f)
		{
			transform.position = Vector2.MoveTowards (transform.position, targetPos, step);
			yield return new WaitForFixedUpdate();
		}

		StopMoving ();

		// Begin walking towards next path in the chain/path-list, or stop walking
		if (++currentPathIndex < path.Count) 
		{
			StartCoroutine ("MoveToPosition");
		}
		else 
		{
			path.Clear();
			currentPathIndex = 0;
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

