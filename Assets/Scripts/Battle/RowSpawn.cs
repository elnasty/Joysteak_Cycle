using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowSpawn : MonoBehaviour
{
    public int number;
    public int delay;


	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FireEven();
            Invoke("FireOdd", 3f);
        }
		
	}
    void FireEven()
    {
        Fire(new Vector2(0, -6.5f));
        Fire(new Vector2(6, -6.5f));
        Fire(new Vector2(12, -6.5f));
        Fire(new Vector2(-6, -6.5f));
        Fire(new Vector2(-12, -6.5f));
    }

    void FireOdd()
    {
        Fire(new Vector2(3, -6.5f));
        Fire(new Vector2(-3, -6.5f));
        Fire(new Vector2(9, -6.5f));
        Fire(new Vector2(-9, -6.5f));
    }

    void Fire(Vector2 position)
    {
        GameObject vine1 = BattleController.instance.GetPooledObject(BattleController.SpawnObjectEnum.vine1);
        if (vine1 == null) return;

        vine1.transform.position = position;
        vine1.SetActive(true);
    }
}
