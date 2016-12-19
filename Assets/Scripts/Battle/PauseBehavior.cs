using UnityEngine;
using System.Collections;

public class PauseBehavior : MonoBehaviour
{
    public GameObject Mem1;
    public GameObject Mem2;
    public GameObject Mem3;

    private bool isPaused;
    private float PauseEndTime;

    // Use this for initialization
    void Start()
    {
        isPaused = false;
        Mem1.SetActive(false);
        Mem2.SetActive(false);
        Mem3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Pause Key Conditions
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused == true)
            {
                Unpause();
                isPaused = false;
                Mem1.SetActive(false);
                Mem2.SetActive(false);
                Mem3.SetActive(false);
            }
            else if (isPaused == false)
            {
                TimedPause(999);
                isPaused = true;
            }
        }
    }

    public void TimedPause(float delay)
    {
        Time.timeScale = 0;
        PauseEndTime = Time.realtimeSinceStartup + delay;

        while (Time.realtimeSinceStartup < PauseEndTime)
        {
            ;
        }
        Unpause();
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }

    private IEnumerator Slideshow(GameObject gameobject)
    {
        gameobject.SetActive(true);
        yield return null;
        TimedPause(3);
        gameobject.SetActive(false);
    }
}

