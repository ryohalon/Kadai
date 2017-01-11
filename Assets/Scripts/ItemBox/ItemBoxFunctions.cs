using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemBoxFunctions : MonoBehaviour
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
    private ItemManager itemManager = null;

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
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
    }

    public void ChangeAlchemyWindow()
    {
        if (itemManager.GetItemNum(
            this.GetComponent<ButtonStatus>().itemId) <= 0)
            return;

        // 表示, 非表示の変更
        alchemy.SetActive(true);
        itemSelect.SetActive(false);
        useItem1.SetActive(true);
        useItem2.SetActive(true);
        useItem3.SetActive(true);
        selectUseItem1.SetActive(true);
        selectUseItem2.SetActive(true);
        selectUseItem3.SetActive(true);
        letsAlchemy.SetActive(true);
        itemBoxManager.SetActiveItemBoxs(false);

        // imageの入れ替え
        switch (selectItemManager.SelectUseItemBox)
        {
            case 0:

                var image = this.transform.GetChild(0);
                useItem1.GetComponent<Image>().sprite =
                    image.gameObject.GetComponent<Image>().sprite;

                break;
            case 1:

                var image2 = this.transform.GetChild(0);
                useItem2.GetComponent<Image>().sprite =
                    image2.gameObject.GetComponent<Image>().sprite;

                break;
            case 2:

                var image3 = this.transform.GetChild(0);
                useItem3.GetComponent<Image>().sprite =
                    image3.gameObject.GetComponent<Image>().sprite;

                break;
            default:
                break;
        }

        if (selectItemManager.SelectUseItemBox >= selectItemManager.selectedItemId.Count)
            return;

        // 前に違うitemがはいっていた場合itemの数を戻す
        if (selectItemManager.selectedItemId[selectItemManager.SelectUseItemBox] != int.MaxValue)
        {
            itemManager.AcquisitionItem(
            selectItemManager.selectedItemId[selectItemManager.SelectUseItemBox]);
        }

        // idを入れ替える
        selectItemManager.selectedItemId[selectItemManager.SelectUseItemBox] =
            this.GetComponent<ButtonStatus>().itemId;
        // 今回使うitemを一つ減らす
        itemManager.UsedItem(
            selectItemManager.selectedItemId[selectItemManager.SelectUseItemBox]);
    }
}
