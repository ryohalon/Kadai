using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    }

    void Update()
    {
        foreach(var selectUseItem in selectUseItems)
        {
            if(selectUseItem.activeInHierarchy)
                selectUseItem.GetComponent<ButtonManager>().UpdateButton();
        }
        if (letsAlchemy.activeInHierarchy)
            letsAlchemy.GetComponent<ButtonManager>().UpdateButton();
        foreach(var itemBox in itemBoxManager.itemBoxs[itemBoxManager.DisplayCategory])
        {
            if (itemBox.activeInHierarchy)
                itemBox.GetComponent<ButtonManager>().UpdateButton();
        }

        SelectAction();
        SelectUseItem();
        Alchemy();
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
            if(!buttonManager.isPushed)
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

            Debug.Log("selectNum" + i.ToString());

            return;
        }

        foreach(var id in selectItemId)
        {
            if (id == -1)
                return;
        }

        buttonManager = letsAlchemy.GetComponent<ButtonManager>();
        if(buttonManager.isPushed)
        {
            buttonManager.isPushed = false;
            status = ActionStatus.ALCHEMY;
        }
    }

    void SelectUseItem()
    {
        if (status != ActionStatus.SELECTUSEITEM)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;

        var itemBoxs = itemBoxManager.itemBoxs;
        foreach(var itemBox in itemBoxs[itemBoxManager.DisplayCategory])
        {
            var button = itemBox.GetComponent<ButtonManager>();
            if (!button.isPushed)
                continue;

            Debug.Log("select");

            button.isPushed = false;
            selectItemId[selectUseItemNum] = itemBox.GetComponent<ItemBoxStatus>().itemId;
            useItems[selectUseItemNum].GetComponent<Image>().sprite =
                itemManager.GetItem(selectItemId[selectUseItemNum]).sprite;

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
        if (status != ActionStatus.ALCHEMY)
            return;


    }
}