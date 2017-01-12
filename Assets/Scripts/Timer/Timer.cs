using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;


public class Timer : MonoBehaviour
{
    [System.Serializable]
    public struct Date
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public float second;
    }
    public Date startDate;

    public int expeditionStageNum = -1;
    public bool isReturn = false;

    static private Timer instance = null;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        LoadTime();
    }

    private void LoadTime()
    {
        TextAsset csvFile = Resources.Load("Data/Time") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        string line = reader.ReadLine();
        string[] timeData = line.Split(',');

        startDate.year = int.Parse(timeData[0]);
        startDate.month = int.Parse(timeData[1]);
        startDate.day = int.Parse(timeData[2]);
        startDate.hour = int.Parse(timeData[3]);
        startDate.minute = int.Parse(timeData[4]);
        startDate.second = int.Parse(timeData[5]);
        expeditionStageNum = int.Parse(timeData[6]);
    }

    public void StartEspedition()
    {
        startDate.year = DateTime.Now.Year;
        startDate.month = DateTime.Now.Month;
        startDate.day = DateTime.Now.Day;
        startDate.hour = DateTime.Now.Hour;
        startDate.minute = DateTime.Now.Minute;
        startDate.second = DateTime.Now.Second;

        isReturn = false;
    }

    void Start()
    {
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return null;
            if (expeditionStageNum == -1)
                continue;
            if (!isReturn)
                isReturn = IsReturn();
        }
    }

    bool IsReturn()
    {
        if (Mathf.Abs(startDate.year - DateTime.Now.Year) > 0)
            return true;
        if (Mathf.Abs(startDate.month - DateTime.Now.Month) > 0)
            return true;
        if (Mathf.Abs(startDate.day - DateTime.Now.Day) > 1)
            return true;
        else if (Mathf.Abs(startDate.day - DateTime.Now.Day) == 1)
        {
            if (24 - startDate.hour + DateTime.Now.Hour >= 2)
                return true;
        }
        else if ((startDate.day - DateTime.Now.Day) == 0)
        {
            if (Mathf.Abs(startDate.hour - DateTime.Now.Hour) >= 2)
                return true;
        }

        return false;
    }


    public void OnApplicationQuit()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/Data/Time.csv", false);
        string txt = startDate.year.ToString() + ','
            + startDate.month.ToString() + ','
            + startDate.day.ToString() + ','
            + startDate.hour.ToString() + ','
            + startDate.minute.ToString() + ','
            + startDate.second.ToString();
        sw.WriteLine(txt);

        sw.Flush();
        sw.Close();
    }
}
