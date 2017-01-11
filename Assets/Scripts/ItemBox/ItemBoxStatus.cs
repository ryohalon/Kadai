using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemBoxStatus : MonoBehaviour
{
    public int itemId = 0;

    public void SetItemNumText(int levelNum, int haveNum)
    {
        var levelNumText = transform.GetChild(0).GetComponent<Text>();
        levelNumText.text = "Lv." + levelNum.ToString();
        var haveNumText = transform.GetChild(1).GetComponent<Text>();
        haveNumText.text = "x" + haveNum.ToString();
    }
}
