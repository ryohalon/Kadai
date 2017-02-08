using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;


public class Timer : MonoBehaviour
{
    public DateTime startDate;

    public int expeditionStageNum = -1;
    public bool isReturn = false;

    static private Timer instance = null;

    List<int[]> stageTimeList = new List<int[]>();

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

        expeditionStageNum = -1;
        LoadTime();
        LoadStageTime();
    }

    private void LoadTime()
    {
        if (File.Exists(Application.persistentDataPath + "/Time.csv"))
        {
            StreamReader sr = new StreamReader(Application.persistentDataPath + "/Time.csv", false);
            string line = sr.ReadLine();
            string[] timeData = line.Split(',');

            startDate = new DateTime(int.Parse(timeData[0]),
            int.Parse(timeData[1]),
            int.Parse(timeData[2]),
            int.Parse(timeData[3]),
            int.Parse(timeData[4]),
            int.Parse(timeData[5]));
            expeditionStageNum = int.Parse(timeData[6]);

            Debug.Log(startDate);
        }
    }

    private void LoadStageTime()
    {
        TextAsset csvFile = Resources.Load("Data/StageTime") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] stageTime = line.Split(',');

            int[] stageTime_ = new int[3];
            for (int i = 0; i < 3; i++)
                stageTime_[i] = int.Parse(stageTime[i]);

            stageTimeList.Add(stageTime_);
        }
    }

    public void StartEspedition()
    {
        startDate = DateTime.Now;

        isReturn = false;
    }

    void Start()
    {

    }

    void Update()
    {
        if (expeditionStageNum == -1)
            return;

        if (!isReturn)
            isReturn = IsReturn();

        // Debug
        if (Input.GetKeyDown(KeyCode.F1))
            isReturn = true;
    }

     public bool IsReturn()
    {
        if (expeditionStageNum == -1)
            return false;
        TimeSpan delta = DateTime.Now - startDate;
        int[] stageTime_ = stageTimeList[expeditionStageNum];
        if (delta.Hours >= stageTime_[0] &&
            delta.Minutes >= stageTime_[1] &&
            delta.Seconds >= stageTime_[2])
            return true;

        return false;
    }

    public TimeSpan GetTime()
    {
        TimeSpan delta = startDate - DateTime.Now;

        return delta;
    }

    public int[] GetStageTime()
    {
        if (expeditionStageNum >= 0)
            return stageTimeList[expeditionStageNum];

        int[] noStage = { 0, 0, 0 };
        return noStage;
    }

    public void OnApplicationQuit()
    {

        FileStream f = (File.Exists(Application.persistentDataPath + "/Time.csv") == true) ?
            new FileStream(Application.persistentDataPath + "/Time.csv", FileMode.Truncate, FileAccess.Write) :
            new FileStream(Application.persistentDataPath + "/Time.csv", FileMode.Create, FileAccess.Write);

        StreamWriter sw = new StreamWriter(f);
        string txt = startDate.Year.ToString() + ','
            + startDate.Month.ToString() + ','
            + startDate.Day.ToString() + ','
            + startDate.Hour.ToString() + ','
            + startDate.Minute.ToString() + ','
            + startDate.Second.ToString() + ','
            + expeditionStageNum.ToString();
        sw.WriteLine(txt);

        sw.Flush();
        sw.Close();
    }
}
