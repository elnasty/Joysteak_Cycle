using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHadoukenWave : MonoBehaviour 
{
	private bool startFiring = false;
	private float diff = 0.01f;
	private int rowNumber = 0;
	private float velocity = 10;
	public float corridorLength;

	void OnEnable() 
	{
		startFiring = true;
	}

	void Update ()
	{
		if (startFiring == true && BattleController.instance.isLevelReadyToStart) {
			if (rowNumber <= corridorLength && velocity > 5) {
				for (int i = 0; i < transform.childCount; i++) {
					Transform child = transform.GetChild (i);
					Fire (child);
				}
				rowNumber++;
				velocity -= diff;
				diff += 0.05f;
			}
		}
	}

	void Fire(Transform child)
	{
		GameObject barb = BattleController.instance.GetPooledObject(BattleController.SpawnObjectEnum.barb);
		barb.GetComponent<Projectile> ().velocity = velocity;
		if (barb == null) return;

		barb.transform.position = child.position;
		Vector3 relative = transform.InverseTransformPoint(barb.transform.position);
		float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg * -1; //-1 for shooting left
		barb.transform.eulerAngles = new Vector3(0, 0, angle);
		barb.SetActive(true);
	}
}
