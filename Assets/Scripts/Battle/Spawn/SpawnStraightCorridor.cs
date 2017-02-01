using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStraightCorridor : MonoBehaviour 
{
	public float delay;
	public float corridorLength;

	private float time = 0.0f;
	private int rowNumber = 0;
	private int noChildSpawnIndex = 5;

	void Update ()
	{
		time += Time.deltaTime;
		if (time > delay)
		{
			time = 0.0f;
			rowNumber++;

			if (rowNumber % corridorLength != 0) 
			{
				for (int i = 0; i < transform.childCount; i++) 
				{
					if (i != noChildSpawnIndex && i != noChildSpawnIndex + 1) 
					{
						Transform child = transform.GetChild (i);
						Fire (child);
					}
				}
			} 
			else 
			{
				noChildSpawnIndex = Random.Range (1, transform.childCount-1);
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
