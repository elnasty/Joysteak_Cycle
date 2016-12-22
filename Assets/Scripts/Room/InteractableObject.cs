using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour {

	public string sceneName;

	void OnMouseDown () {
		RoomController.instance.isInteractableSelected = true;
		RoomController.instance.sceneToLoad = sceneName;
	}
		
}
