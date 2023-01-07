using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour
{
    public Text timeElapsedDisplay;

    private float elapsedTime;

    private Dictionary<string, float> timeCounters = new Dictionary<string, float>();


    void Update()
    {
        timeElapsedDisplay.text = elapsedTime.ToString("f2");
        elapsedTime += worldTime * Time.deltaTime;
        List<string> keys = new List<string>(timeCounters.Keys);
        foreach (string key in keys)
        {
            if (timeCounters[key] > 0)
            {
                timeCounters[key] -= Time.deltaTime * worldTime;
            }
        }
    }

    private float worldTime = 1;
    public float WorldTime
    {
        get
        {
            return worldTime;
        }
        set
        {
            worldTime = value;
        }
    }

    public float getTimeSpeed(float speed, float delta)
    {
        return worldTime * delta * speed;
    }

    public void setCounter(string name, float time)
    {
        timeCounters[name] = time;
    }

    public bool hasElapsed(string name)
    {
        if (!timeCounters.ContainsKey(name))
        {
            return true;
        }
        return timeCounters[name] <= 0;
    }

}