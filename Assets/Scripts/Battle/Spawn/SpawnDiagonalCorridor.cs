using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDiagonalCorridor : MonoBehaviour 
{	
	public float delay;
	public int corridorWidth;

	private float time = 0.0f;
	private int rowNumber = 0;
	private int noChildSpawnIndex = 5;

	void Update ()
	{
		time += Time.deltaTime;
		if (time > delay && BattleController.instance.isLevelReadyToStart)
		{
			time = 0.0f;
			rowNumber++;
			noChildSpawnIndex = rowNumber % transform.childCount;
			bool isEvenSegment = (rowNumber / transform.childCount) % 2 == 0;
			if (isEvenSegment) noChildSpawnIndex = transform.childCount - noChildSpawnIndex;
			for (int i = 0; i < transform.childCount; i++) 
			{
				bool isWithinEmptyArea = i < noChildSpawnIndex + corridorWidth && i > noChildSpawnIndex - corridorWidth + 1;
				if (!isWithinEmptyArea) {
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
