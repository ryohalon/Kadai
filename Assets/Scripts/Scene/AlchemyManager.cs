using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AlchemyManager : MonoBehaviour
{
    private ItemManager itemManager = null;
    [SerializeField]
    private List<GameObject> useItems = new List<GameObject>();
    [SerializeField]
    private List<GameObject> selectUseItems = new List<GameObject>();
    [SerializeField]
    private GameObject letsAlchemy = null;
    [SerializeField]
    private ItemBoxManager itemBoxManager = null;
    [SerializeField]
    private RecipeManager recipeManager = null;
    [SerializeField]
    private GameObject alchemyItem = null;
    private GameObject alchemyItem_ = null;

    private const int maxUseItemNum = 3;
    private int[] selectItemId = new int[maxUseItemNum];
    private int selectUseItemNum = -1;

    private enum ActionStatus
    {
        SELECTACTION,
        SELECTUSEITEM,
        ALCHEMY
    }
    ActionStatus status;

    void Awake()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        status = ActionStatus.SELECTACTION;
        for (int i = 0; i < maxUseItemNum; i++)
            selectItemId[i] = -1;
    }

    void Start()
    {
        itemBoxManager.SetAvtiveAllItemBoxs(false);
    }

    void Update()
    {
        foreach (var selectUseItem in selectUseItems)
        {
            if (selectUseItem.activeInHierarchy)
                selectUseItem.GetComponent<ButtonManager>().UpdateButton();
        }
        if (letsAlchemy.activeInHierarchy)
            letsAlchemy.GetComponent<ButtonManager>().UpdateButton();
        foreach (var itemBox in itemBoxManager.itemBoxs[itemBoxManager.DisplayCategory])
        {
            if (itemBox.activeInHierarchy)
                itemBox.GetComponent<ButtonManager>().UpdateButton();
        }

        SelectAction();
        SelectUseItem();
        DisplayAlchemyItem();
    }

    void SelectAction()
    {
        if (status != ActionStatus.SELECTACTION)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;

        ButtonManager buttonManager;

        for (int i = 0; i < selectUseItems.Count; i++)
        {
            buttonManager = selectUseItems[i].GetComponent<ButtonManager>();
            if (!buttonManager.isPushed)
                continue;

            buttonManager.isPushed = false;
            selectUseItemNum = i;
            status = ActionStatus.SELECTUSEITEM;

            foreach (var useItem in useItems)
                useItem.SetActive(false);
            foreach (var selectUseItem in selectUseItems)
                selectUseItem.SetActive(false);
            letsAlchemy.SetActive(false);

            itemBoxManager.SetActiveItemBoxs(true);
            itemBoxManager.LoadItem();

            return;
        }

        foreach (var id in selectItemId)
        {
            if (id == -1)
                return;
        }

        buttonManager = letsAlchemy.GetComponent<ButtonManager>();
        if (buttonManager.isPushed)
        {
            buttonManager.isPushed = false;
            status = ActionStatus.ALCHEMY;

            Alchemy();

            foreach (var useItem in useItems)
            {
                useItem.GetComponent<Image>().sprite = null;
                useItem.SetActive(false);
            }
            foreach (var selectUseItem in selectUseItems)
                selectUseItem.SetActive(false);
            letsAlchemy.SetActive(false);
        }
    }

    void SelectUseItem()
    {
        if (status != ActionStatus.SELECTUSEITEM)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;

        var itemBoxs = itemBoxManager.itemBoxs;
        foreach (var itemBox in itemBoxs[itemBoxManager.DisplayCategory])
        {
            var button = itemBox.GetComponent<ButtonManager>();
            if (!button.isPushed)
                continue;
            var itemBoxStatus = itemBox.GetComponent<ItemBoxStatus>();
            if (itemManager.GetItemNum(itemBoxStatus.itemId) <= 0)
                continue;

            button.isPushed = false;
            selectItemId[selectUseItemNum] = itemBoxStatus.itemId;
            useItems[selectUseItemNum].GetComponent<Image>().sprite =
                itemManager.GetItem(selectItemId[selectUseItemNum]).sprite;

            // Itemの消費
            itemManager.UsedItem(itemBoxStatus.itemId);
            
            foreach (var useItem in useItems)
                useItem.SetActive(true);
            foreach (var selectUseItem in selectUseItems)
                selectUseItem.SetActive(true);
            letsAlchemy.SetActive(true);

            itemBoxManager.SetActiveItemBoxs(false);

            status = ActionStatus.SELECTACTION;

            return;
        }
    }

    void Alchemy()
    {
        alchemyItem_ = Instantiate(alchemyItem);
        alchemyItem_.transform.SetParent(GameObject.Find("Canvas").transform);
        alchemyItem_.transform.localPosition = Vector3.zero;

        int alchemyItemId = IsSuccess();
        if (alchemyItemId != -1)
        {
            itemManager.AcquisitionItem(alchemyItemId);
            Debug.Log("レシピ増えた : " + alchemyItemId);

            for (int i = 0; i < maxUseItemNum; i++)
                selectItemId[i] = -1;

            foreach (var itemCategory in itemManager.items)
            {
                foreach (var item in itemCategory)
                {
                    var itemStatus = item.GetComponent<ItemStatus>();
                    if (itemStatus.id != alchemyItemId)
                        continue;

                    alchemyItem_.GetComponent<Image>().sprite =
                        itemStatus.sprite;

                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < maxUseItemNum; i++)
                selectItemId[i] = -1;

            int randomAlchemyItemId = UnityEngine.Random.Range(1, itemManager.itemMaxId);
            itemManager.AcquisitionItem(randomAlchemyItemId);
            Debug.Log("item : " + randomAlchemyItemId);
            foreach (var items_ in itemManager.items)
            {
                foreach (var item in items_)
                {
                    var itemStatus = item.GetComponent<ItemStatus>();
                    if (itemStatus.id != randomAlchemyItemId)
                        continue;

                    alchemyItem_.GetComponent<Image>().sprite =
                        itemStatus.sprite;

                    return;
                }
            }
        }
    }

    private int IsSuccess()
    {
        var recipes = recipeManager.recipes;
        for (int i = 0; i < recipes.Count; i++)
        {
            bool[] isSuccess = new bool[3]
            {
                false,
                false,
                false
            };

            foreach (var id in selectItemId)
            {
                if (recipes[i][0] == id && !isSuccess[0])
                    isSuccess[0] = true;
                else if (recipes[i][1] == id && !isSuccess[1])
                    isSuccess[1] = true;
                else if (recipes[i][2] == id && !isSuccess[2])
                    isSuccess[2] = true;
            }

            if (!isSuccess[0] ||
                !isSuccess[1] ||
                !isSuccess[2])
                continue;

            if (!recipeManager.isDiscovery[i])
                recipeManager.isDiscovery[i] = true;

            return recipes[i][3];
        }

        return -1;
    }

    void DisplayAlchemyItem()
    {
        if (status != ActionStatus.ALCHEMY)
            return;
        if (!alchemyItem_.GetComponent<AlchemyItemManager>().isEaseEnd)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;

        foreach (var useItem in useItems)
            useItem.SetActive(true);
        foreach (var selectUseItem in selectUseItems)
            selectUseItem.SetActive(true);
        letsAlchemy.SetActive(true);

        Destroy(alchemyItem_);

        status = ActionStatus.SELECTACTION;
    }
}