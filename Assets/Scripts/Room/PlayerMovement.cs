using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

    public float movespeed;
	public GameObject waypointsCollection;

    private float moveY;
    private float moveX;
	private Vector2 targetPos;
	private List<Vector2> straightPaths = new List<Vector2>();
	private int currentPathIndex = 0;
	private List<GameObject> waypointObjects = new List<GameObject>();

	enum Direction { None = 0, Down = 1, Up = 2, Right = 3, Left = 4 };

	void Start()
	{
		// Initialize waypointObjects list, which will be used for pathfinding
		foreach (Transform child in waypointsCollection.transform) 
			waypointObjects.Add (child.gameObject);
	}

	Stack<Waypoint> Dijkstra()
	{
		List<Waypoint> waypoints = new List<Waypoint> ();
		float nearestDistFromPlayer = int.MaxValue;
		float nearestDistFromTarget = int.MaxValue;
		Waypoint source = null;
		Waypoint target = null;

		foreach (GameObject waypointObj in waypointObjects) 
		{
			Waypoint waypoint = waypointObj.transform.GetComponent<Waypoint> ();
			waypoint.distToSource = int.MaxValue;
			waypoint.prevOptimalWaypoint = null;
			waypoints.Add (waypoint);

			// Find source: Waypoint that is nearest to player
			float distFromPlayer = ((Vector2)waypoint.transform.position - (Vector2)transform.position).magnitude;
			if (distFromPlayer < nearestDistFromPlayer) 
			{
				nearestDistFromPlayer = distFromPlayer;
				source = waypoint;
			}

			// Find target: Waypoint that is nearest to target position
			float distFromTarget = ((Vector2)waypoint.transform.position - (Vector2)targetPos).magnitude;
			if (distFromTarget < nearestDistFromTarget) 
			{
				nearestDistFromTarget = distFromTarget;
				target = waypoint;
			}
		}

		source.distToSource = 0; // Distance of source to itself is 0
		source.prevOptimalWaypoint = null; // Not possible to have waypoint before source
			
		while (waypoints.Count > 0)
		{
			// Get waypoint that has minimum dist in the list
			int minDist = int.MaxValue;
			Waypoint currentWaypoint = null;
			foreach (Waypoint waypoint in waypoints) 
			{
				if (waypoint.distToSource < minDist) 
				{
					minDist = waypoint.distToSource;
					currentWaypoint = waypoint;
				}
			}

			waypoints.Remove (currentWaypoint);

			List<GameObject> neighbourWaypointObjects = currentWaypoint.waypoints;
			foreach (GameObject waypointObj in neighbourWaypointObjects)
			{
				Waypoint neighbourWaypoint = waypointObj.transform.GetComponent<Waypoint> ();
				int alternateDist = currentWaypoint.distToSource + 1;
				if (alternateDist < neighbourWaypoint.distToSource) 
				{
					neighbourWaypoint.distToSource = alternateDist;
					neighbourWaypoint.prevOptimalWaypoint = currentWaypoint;
				}
			}

			// Stop searching: we have found our target
			if (currentWaypoint.Equals (target)) break;

		}

		// Create a stack of waypoints that will be path from source to target
		Stack<Waypoint> waypointStack = new Stack<Waypoint>();
		waypointStack.Push (target);
		Waypoint nextWaypoint = target.prevOptimalWaypoint;
		while (nextWaypoint != null) 
		{
			waypointStack.Push (nextWaypoint);
			nextWaypoint = nextWaypoint.prevOptimalWaypoint;
		}
			
		return waypointStack;
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

			straightPaths.Clear ();
			currentPathIndex = 0;
		}
	}

	void StopMoving()
	{
		StopCoroutine("MoveToPosition");
		transform.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
		GetComponent<Animator> ().SetInteger ("Direction", (int)Direction.None);
	}

	public void MovePlayerTo(Vector2 targetCoords)
	{
		RoomController.instance.isPlayerMoving = true;

		StopMoving ();
		straightPaths.Clear ();
		currentPathIndex = 0;
		targetPos = targetCoords;

		// Execute dijstra to return a stack of waypoints leading from playerPos to mousceClickPos
		Stack<Waypoint> waypointStack = Dijkstra ();
		Vector2 prevWaypointPos = (Vector2)transform.position;

		// Unroll waypoints stack and add them to path to traverse
		while (waypointStack.Count > 0) 
		{
			Waypoint currentWaypoint = waypointStack.Pop ();
			Vector2 waypointPos = currentWaypoint.gameObject.transform.position;
			Vector2 diffFromPrev = waypointPos - (Vector2) prevWaypointPos;

			// Add the straight path with the smaller displacement into the straightPaths list first
			// (Avoid making Casey walk diagonally as no animation for it yet)
			if (Mathf.Abs (diffFromPrev.x) < Mathf.Abs (diffFromPrev.y)) 
			{
				straightPaths.Add (new Vector2 (waypointPos.x, prevWaypointPos.y));
				straightPaths.Add (new Vector2 (waypointPos.x, waypointPos.y));
			}
			else 
			{
				straightPaths.Add (new Vector2 (prevWaypointPos.x, waypointPos.y));
				straightPaths.Add (new Vector2 (waypointPos.x, waypointPos.y));
			}

			prevWaypointPos = waypointPos;
		}
			
		// Begin executing chain of paths
		StartCoroutine ("MoveToPosition");
	}

	IEnumerator MoveToPosition()
	{
		float step = movespeed * Time.fixedDeltaTime;
		Vector2 targetPos = straightPaths[currentPathIndex];
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

		// Keep walking towards target until player reach it's destination
		while (Vector2.Distance(targetPos, transform.position) > 0.1f)
		{
			transform.position = Vector2.MoveTowards (transform.position, targetPos, step);
			yield return new WaitForFixedUpdate();
		}

		StopMoving ();

		// Begin walking towards next path in the chain/path-list, or stop walking
		if (++currentPathIndex < straightPaths.Count) 
		{
			StartCoroutine ("MoveToPosition");
		}
		else 
		{
			RoomController.instance.isPlayerMoving = false;
			straightPaths.Clear();
			currentPathIndex = 0;
		}
	}

	public void MoveByWASD() 
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

