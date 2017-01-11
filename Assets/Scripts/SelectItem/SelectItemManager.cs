using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectItemManager : MonoBehaviour
{
    public int SelectUseItemBox = 0;
    public List<int> selectedItemId = new List<int>();

    void Awake()
    {
        for (int i = 0; i < 3; i++)
            selectedItemId.Add(int.MaxValue);
    }
}
