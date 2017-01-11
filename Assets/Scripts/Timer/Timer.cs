using UnityEngine;
using System.Collections;
using System.IO;
using System;


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
    public Date startDayDate;
    public Date lastDate;
    public Date nowDate;

    static private Timer instance = null;

    public void SetStartDayTime()
    {
        startDayDate.year = DateTime.Now.Year;
        startDayDate.month = DateTime.Now.Month;
        startDayDate.day = DateTime.Now.Day;
        startDayDate.hour = DateTime.Now.Hour;
        startDayDate.minute = DateTime.Now.Minute;
        startDayDate.second = DateTime.Now.Second;
        nowDate.year = DateTime.Now.Year;
        nowDate.month = DateTime.Now.Month;
        nowDate.day = DateTime.Now.Day;
        nowDate.hour = DateTime.Now.Hour;
        nowDate.minute = DateTime.Now.Minute;
        nowDate.second = DateTime.Now.Second;
    }

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

        LoadLastApplicationEndTime();
        LoadNowTime();
    }

    private void LoadNowTime()
    {
        nowDate.year = DateTime.Now.Year;
        nowDate.month = DateTime.Now.Month;
        nowDate.day = DateTime.Now.Day;
        nowDate.hour = DateTime.Now.Hour;
        nowDate.minute = DateTime.Now.Minute;
        nowDate.second = DateTime.Now.Second;
    }

    private void LoadLastApplicationEndTime()
    {
        string txt = "";

        FileInfo fi = new FileInfo(Application.dataPath + "/Resources/Data/TimerData.csv");
        try
        {
            using (StreamReader sr = new StreamReader(fi.Open(FileMode.Open, FileAccess.Read)))
            {
                txt += sr.ReadToEnd();

                sr.Close();
            }
        }
        catch (Exception e)
        {

        }

        string[] lastDate_ = txt.Split(',');
        startDayDate.year = int.Parse(lastDate_[0]);
        startDayDate.month = int.Parse(lastDate_[1]);
        startDayDate.day = int.Parse(lastDate_[2]);
        startDayDate.hour = int.Parse(lastDate_[3]);
        startDayDate.minute = int.Parse(lastDate_[4]);
        startDayDate.second = int.Parse(lastDate_[5]);
        lastDate.year = int.Parse(lastDate_[6]);
        lastDate.month = int.Parse(lastDate_[7]);
        lastDate.day = int.Parse(lastDate_[8]);
        lastDate.hour = int.Parse(lastDate_[9]);
        lastDate.minute = int.Parse(lastDate_[10]);
        lastDate.second = float.Parse(lastDate_[11]);
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
        }
    }


    // アプリ終了時に時間を書き込みをしておく
    public void OnApplicationQuit()
    {
        StreamWriter writer;
        writer = new StreamWriter(Application.dataPath + "/Resources/Data/TimerData.csv", false);
        writer.WriteLine(startDayDate.year +
            "," + startDayDate.month +
            "," + startDayDate.day +
            "," + startDayDate.hour +
            "," + startDayDate.minute +
            "," + startDayDate.second +
            "," + DateTime.Now.Year +
            "," + DateTime.Now.Month +
            "," + DateTime.Now.Day +
            "," + DateTime.Now.Hour +
            "," + DateTime.Now.Minute +
            "," + DateTime.Now.Second +
            ",");
        writer.Flush();
        writer.Close();
    }
}
