using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingVineMovement : MonoBehaviour {

	//ringvine reads the path object children and goes to each one down the list
	public GameObject pathObjParent;
	public float speed;

	List<GameObject> pathObjList = new List<GameObject>();
	Rigidbody2D rgbody;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < pathObjParent.transform.childCount; i++){
			pathObjList.Add (pathObjParent.transform.GetChild (i).gameObject);
		}

		rgbody = GetComponent<Rigidbody2D> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		//go to each pathobj in sequence (for some reason it goes backwards, i.e 3 path objects, goes from 3,2,1 instead of 1,2,3)
		for (int i = 0; i <pathObjList.Count; i++) {
			StartCoroutine(MoveToPathObj (pathObjList [i]));
		}
	}

	IEnumerator FadeInAndStart(){
		yield return null;
	}

	IEnumerator MoveToPathObj(GameObject currPathObj){
		float distToPoint = ((transform.position - currPathObj.transform.position).magnitude);
		Vector2 direction = currPathObj.transform.position - transform.position;

		while (distToPoint > 0 ) {
			distToPoint = ((transform.position - currPathObj.transform.position).magnitude);
			direction = currPathObj.transform.position - transform.position;
			rgbody.velocity = direction.normalized * speed;
			if (distToPoint <= 0.05f) {
				break;
			}
			yield return null;
		}
	}
}
