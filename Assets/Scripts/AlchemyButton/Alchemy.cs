using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Alchemy : MonoBehaviour
{
    [SerializeField]
    private GameObject alchemy = null;
    [SerializeField]
    private GameObject useItem1 = null;
    [SerializeField]
    private GameObject useItem2 = null;
    [SerializeField]
    private GameObject useItem3 = null;
    [SerializeField]
    private GameObject selectUseItem1 = null;
    [SerializeField]
    private GameObject selectUseItem2 = null;
    [SerializeField]
    private GameObject selectUseItem3 = null;
    [SerializeField]
    private GameObject letsAlchemy = null;

    private ItemManager itemManager = null;

    [SerializeField]
    private SelectItemManager selectItemManager = null;
    private List<List<int>> recipes = new List<List<int>>();

    [SerializeField]
    private GameObject alchemyItem = null;

    void DebugRecipes()
    {
        List<int> recipes_ = new List<int>();
        recipes_.Add(1);
        recipes_.Add(2);
        recipes_.Add(3);
        recipes_.Add(4);

        recipes.Add(recipes_);
        recipes_ = new List<int>();

        recipes_.Add(4);
        recipes_.Add(5);
        recipes_.Add(6);
        recipes_.Add(7);

        recipes.Add(recipes_);
        recipes_ = new List<int>();

        recipes_.Add(6);
        recipes_.Add(7);
        recipes_.Add(8);
        recipes_.Add(9);

        recipes.Add(recipes_);
    }

    void Start()
    {
        FindGameObject();

        DebugRecipes();
    }

    private void FindGameObject()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
    }

    public void DoAlchemist()
    {
        if (selectItemManager.selectedItemId[0] == int.MaxValue ||
            selectItemManager.selectedItemId[1] == int.MaxValue ||
            selectItemManager.selectedItemId[2] == int.MaxValue)
            return;

        var alchemyItem_ = Instantiate(alchemyItem);
        alchemyItem_.transform.SetParent(GameObject.Find("Canvas").transform);
        alchemyItem_.GetComponent<RectTransform>().position =
            GameObject.Find("Canvas").transform.position;

        alchemy.SetActive(false);
        selectUseItem1.SetActive(false);
        selectUseItem2.SetActive(false);
        selectUseItem3.SetActive(false);
        letsAlchemy.SetActive(false);

        useItem1.GetComponent<Image>().sprite = null;
        useItem2.GetComponent<Image>().sprite = null;
        useItem3.GetComponent<Image>().sprite = null;
        useItem1.SetActive(false);
        useItem2.SetActive(false);
        useItem3.SetActive(false);


        foreach (var recipe in recipes)
        {
            bool[] success = new bool[3]
            {
                false,
                false,
                false
            };

            if (recipe[0] == selectItemManager.selectedItemId[0] ||
                recipe[1] == selectItemManager.selectedItemId[0] ||
                recipe[2] == selectItemManager.selectedItemId[0])
                success[0] = true;
            if (recipe[0] == selectItemManager.selectedItemId[1] ||
                recipe[1] == selectItemManager.selectedItemId[1] ||
                recipe[2] == selectItemManager.selectedItemId[1])
                success[1] = true;
            if (recipe[0] == selectItemManager.selectedItemId[2] ||
                recipe[1] == selectItemManager.selectedItemId[2] ||
                recipe[2] == selectItemManager.selectedItemId[2])
                success[2] = true;

            if (!success[0] || !success[1] || !success[2])
                continue;

            // 成功した場合
            // 錬金されたitemを一つ増やす
            itemManager.AcquisitionItem(recipe[3]);
            Debug.Log("レシピ増えた : " + recipe[3].ToString());
            // imageの入れ替え
            foreach (var items_ in itemManager.items)
            {
                foreach (var item in items_)
                {
                    var itemStatus = item.GetComponent<ItemStatus>();
                    if (itemStatus.id != recipe[3])
                        continue;

                    alchemyItem_.GetComponent<Image>().sprite =
                        itemStatus.sprite;

                    selectItemManager.selectedItemId[0] = int.MaxValue;
                    selectItemManager.selectedItemId[1] = int.MaxValue;
                    selectItemManager.selectedItemId[2] = int.MaxValue;

                    return;
                }
            }
        }

        // レシピに存在しない錬金の場合
        // 錬金に使用されたアイテムの中でLv.1～一番レベルが高い
        // ところの範囲でアイテムを指定して錬金する
        // 今のところは全アイテムランダムでやる
        int randomAlchemyItemId = UnityEngine.Random.Range(1, itemManager.itemMaxId);
        // 錬金されたitemを一つ増やす
        itemManager.AcquisitionItem(randomAlchemyItemId);
        foreach (var items_ in itemManager.items)
        {
            foreach (var item in items_)
            {
                var itemStatus = item.GetComponent<ItemStatus>();
                if (itemStatus.id != randomAlchemyItemId)
                    continue;
                
                Debug.Log("増えた : " + randomAlchemyItemId.ToString());

                alchemyItem_.GetComponent<Image>().sprite =
                    itemStatus.sprite;

                selectItemManager.selectedItemId[0] = int.MaxValue;
                selectItemManager.selectedItemId[1] = int.MaxValue;
                selectItemManager.selectedItemId[2] = int.MaxValue;

                return;
            }
        }

        Debug.Log("Not exist item : id");
    }
}
