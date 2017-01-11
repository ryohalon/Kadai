using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ItemBoxManager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemBox = null;
    public List<List<GameObject>> itemBoxs = new List<List<GameObject>>();

    private ItemManager itemManager = null;
    [SerializeField]
    private Vector2 boxDixtance = new Vector2(10.0f, 9.0f);

    public int DisplayCategory = 0;

    void Awake()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        var items = itemManager.items;
        for (int i = 0; i < items.Count; i++)
        {
            List<GameObject> itemBoxs_ = new List<GameObject>();
            for (int k = 0; k < items[i].Count; k++)
                itemBoxs_.Add(Instantiate(itemBox));

            itemBoxs.Add(itemBoxs_);
        }

        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
            {
                var itemStatus = items[i][k].GetComponent<ItemStatus>();

                itemBoxs[i][k].transform.SetParent(GameObject.Find("Canvas").transform);
                itemBoxs[i][k].transform.localScale = Vector3.one;
                itemBoxs[i][k].GetComponent<RectTransform>().localPosition =
                    new Vector3(
                        -boxDixtance.x + boxDixtance.x * (k % 3) + GameObject.Find("Canvas").transform.position.x,
                        boxDixtance.y - boxDixtance.y * (k / 3) + GameObject.Find("Canvas").transform.position.y,
                        0.0f);
                itemBoxs[i][k].GetComponent<ItemBoxStatus>().itemId = itemStatus.id;

                itemBoxs[i][k].GetComponent<Image>().sprite = itemStatus.sprite;
                itemBoxs[i][k].GetComponent<ItemBoxStatus>().SetItemNumText(itemStatus.level, itemStatus.num);
            }
        }
    }

    void Start()
    {
        StartCoroutine(updateItemBoxs());
    }

    private IEnumerator updateItemBoxs()
    {
        while (true)
        {
            //CheckActive();

            yield return null;
        }
    }

    private void CheckActive()
    {
        var items = itemManager.items;
        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
            {
                var itemStatus = items[i][k].GetComponent<ItemStatus>();

                var levelText = itemBoxs[i][k].transform.GetChild(0);
                var numText = itemBoxs[i][k].transform.GetChild(1);
                numText.GetComponent<Text>().text = "x" + itemStatus.num.ToString();

                if (itemStatus.num > 0)
                {
                    itemBoxs[i][k].gameObject.SetActive(true);
                    levelText.gameObject.SetActive(true);
                }
                else
                {
                    itemBoxs[i][k].gameObject.SetActive(false);
                    levelText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetAvtiveAllItemBoxs(bool flag)
    {
        foreach(var itemCategory in itemBoxs)
        {
            foreach (var itemBox_ in itemCategory)
                itemBox_.SetActive(flag);
        }
    }

    // itemBoxを表示するかしないか変える
    // flag : ture ⇒ 表示, false ⇒ 非表示
    public void SetActiveItemBoxs(bool flag)
    {
        for (int k = 0; k < itemBoxs[DisplayCategory].Count; k++)
            itemBoxs[DisplayCategory][k].SetActive(flag);
    }

    // 表示するitemのカテゴリーを変更する
    // value : - ⇒ 一つ戻る, + ⇒ 一つ進む, ±0 ⇒ 一つ進む
    public void ChangeDisplayItemCategory(int value)
    {
        for (int i = 0; i < itemBoxs[DisplayCategory].Count; i++)
            itemBoxs[DisplayCategory][i].SetActive(false);

        if (value >= 0)
            DisplayCategory = Mathf.Min(DisplayCategory + 1, itemBoxs.Count - 1);
        else
            DisplayCategory = Mathf.Max(DisplayCategory - 1, 0);

        for (int i = 0; i < itemBoxs[DisplayCategory].Count; i++)
            itemBoxs[DisplayCategory][i].SetActive(true);
    }

    public void LoadItem()
    {
        var items = itemManager.items;
        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
            {
                var itemStatus = items[i][k].GetComponent<ItemStatus>();
                itemBoxs[i][k].GetComponent<ItemBoxStatus>().SetItemHaveNum(itemStatus.num);
            }
        }
    }
}
