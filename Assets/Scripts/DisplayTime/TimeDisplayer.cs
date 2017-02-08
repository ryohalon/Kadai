using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System;

public class TimeDisplayer : MonoBehaviour
{
    private Timer timer = null;
    private Text text = null;

    void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        TimeSpan time = timer.GetTime();
        int[] stageTime = timer.GetStageTime();

        int hour = stageTime[0] + time.Hours;
        int minute = stageTime[1] + time.Minutes;
        int second = stageTime[2] + time.Seconds;
        if(second < 0)
        {
            second = 60 + second;
            minute--;
        }
        if(minute < 0)
        {
            minute = 60 + minute;
            hour--;
        }

        var s = new StringBuilder();
        s.Append((hour).ToString());
        s.Append(':');
        s.Append((minute).ToString());
        s.Append(':');
        s.Append((second).ToString());

        text.text = s.ToString();
    }
}
