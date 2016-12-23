using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour {

	public string sceneName;
	public Waypoint attachedWaypoint;

	void OnMouseDown () 
	{
		RoomController.instance.sceneToLoad = sceneName;
		RoomController.instance.selectedInteractableWaypoint = attachedWaypoint;
	}
		
}
