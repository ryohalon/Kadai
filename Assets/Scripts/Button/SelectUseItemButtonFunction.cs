using UnityEngine;
using System.Collections;

public class SelectUseItemButtonFunction : MonoBehaviour
{
    private GameObject alchemy = null;
    private GameObject itemSelect = null;
    private GameObject useItem1 = null;
    private GameObject useItem2 = null;
    private GameObject useItem3 = null;
    private GameObject selectUseItem1 = null;
    private GameObject selectUseItem2 = null;
    private GameObject selectUseItem3 = null;
    private GameObject letsAlchemy = null;

    private SelectItemManager selectItemManager = null;
    private ItemBoxManager itemBoxManager = null;

    void Awake()
    {
        alchemy = GameObject.Find("Alchemy");
        itemSelect = GameObject.Find("ItemSelect");
        useItem1 = GameObject.Find("UseItem1");
        useItem2 = GameObject.Find("UseItem2");
        useItem3 = GameObject.Find("UseItem3");
        selectUseItem1 = GameObject.Find("SelectUseItem1");
        selectUseItem2 = GameObject.Find("SelectUseItem2");
        selectUseItem3 = GameObject.Find("SelectUseItem3");
        letsAlchemy = GameObject.Find("Let'sAlchemy");

        selectItemManager = GameObject.Find("SelectItemManager").GetComponent<SelectItemManager>();
        itemBoxManager = GameObject.Find("ItemBoxManager").GetComponent<ItemBoxManager>();
    }

    public void ChangeSelectWindow()
    {
        alchemy.SetActive(false);
        itemSelect.SetActive(true);
        useItem1.SetActive(false);
        useItem2.SetActive(false);
        useItem3.SetActive(false);
        selectUseItem1.SetActive(false);
        selectUseItem2.SetActive(false);
        selectUseItem3.SetActive(false);
        letsAlchemy.SetActive(false);

        itemBoxManager.SetActiveItemBoxs(true);

        selectItemManager.SelectUseItemBox = this.GetComponent<ButtonStatus>().id;
    }
}
