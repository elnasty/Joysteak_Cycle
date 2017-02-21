using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNoCorridor : MonoBehaviour 
{
	public float delay;
	public float corridorLength;

	private float time = 0.0f;
	private int rowNumber = 0;

	void Update ()
	{
		time += Time.deltaTime;
		if (time > delay && BattleController.instance.isLevelReadyToStart)
		{
			time = 0.0f;
			rowNumber++;

			if (rowNumber % corridorLength != 0) 
			{
				for (int i = 0; i < transform.childCount; i++) 
				{
					Transform child = transform.GetChild (i);
					Fire (child);
				}
			} 
		}
	}

	void Fire(Transform child)
	{
		GameObject barb = BattleController.instance.GetPooledObject(BattleController.SpawnObjectEnum.barb);
		if (barb == null) return;

		barb.transform.position = child.position;
		Vector3 relative = transform.InverseTransformPoint(barb.transform.position);
		float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg * -1; //-1 for shooting left
		barb.transform.eulerAngles = new Vector3(0, 0, angle);
		barb.SetActive(true);
	}
}
