using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Slider slider;
    private GameObject[] agents;
    private void Start()
    {
        Time.timeScale = 0;

        slider.onValueChanged.AddListener(TimeSliderChanged);
        agents = GameObject.FindGameObjectsWithTag("Player");
    }

    public void ToogleTrails()
    {
        foreach (GameObject agent in agents)
        {
            TrailRenderer tr = agent.GetComponentInChildren<TrailRenderer>();
            if (tr.enabled)
            {
                tr.enabled = false;
            }
         else
            {
                tr.enabled = true;
            }
        }
    }

    public void togglePause()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;

        }
        else
        {
            Time.timeScale = 0;

        }
    }

    public void TimeSliderChanged(float val)
    {
        Time.timeScale = val;

    }

    public void Reset()
    {
        //Time.timeScale = 0;
        Application.LoadLevel(Application.loadedLevel);
    }
}
