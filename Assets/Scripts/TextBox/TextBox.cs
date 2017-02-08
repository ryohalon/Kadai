using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextBox : MonoBehaviour
{
    private List<string> texts = new List<string>();
    private string text = null;
    public bool isEnd = false;

    void Start()
    {

    }

    void Update()
    {
        if (isEnd)
            return;

        if (Input.GetMouseButtonDown(0))
            UpdateText();
    }

    public void UpdateText()
    {
        Debug.Log(texts.Count);
        text = "";

        for(int i = 0; i < 3; i++)
        {
            if (isEnd)
                break;

            text += texts[0];
            texts.RemoveAt(0);
            if (i != 2)
                text += "\n";
            if (texts.Count == 0)
            {
                isEnd = true;
                break;
            }
        }

        transform.GetChild(0).GetComponent<Text>().text = text;
    }

    public void AddGetItem(List<string> itemName)
    {
        texts.Clear();
        texts = itemName;
    }
}
