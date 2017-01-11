using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemBase = null;
    public List<List<GameObject>> items = new List<List<GameObject>>();

    [SerializeField]
    public List<Sprite> sprites = new List<Sprite>();

    public int itemMaxId = 0;

    private ItemManager instance = null;

    void DebugItems()
    {
        int i = 0;
        List<GameObject> items_ = new List<GameObject>();
        ItemStatus itemStatus;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 1;
        itemStatus.level = 1;
        itemStatus.num = 1;
        itemStatus.sprite = sprites[i];
        i++;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 2;
        itemStatus.level = 2;
        itemStatus.num = 2;
        itemStatus.sprite = sprites[i];
        i++;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 3;
        itemStatus.level = 3;
        itemStatus.num = 3;
        itemStatus.sprite = sprites[i];
        i++;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 4;
        itemStatus.level = 4;
        itemStatus.num = 5;
        itemStatus.sprite = sprites[i];
        i++;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 5;
        itemStatus.level = 5;
        itemStatus.num = 5;
        itemStatus.sprite = sprites[i];
        i++;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 6;
        itemStatus.level = 6;
        itemStatus.num = 5;
        itemStatus.sprite = sprites[i];
        i++;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 7;
        itemStatus.level = 7;
        itemStatus.num = 5;
        itemStatus.sprite = sprites[i];
        i++;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 8;
        itemStatus.level = 8;
        itemStatus.num = 5;
        itemStatus.sprite = sprites[i];
        i++;

        items_.Add(Instantiate(itemBase));
        itemStatus = items_[i].GetComponent<ItemStatus>();
        itemStatus.id = 9;
        itemStatus.level = 9;
        itemStatus.num = 5;
        itemStatus.sprite = sprites[i];

        items.Add(items_);
        itemMaxId = 9;

        foreach(var itemCategory in items)
        {
            foreach(var item in itemCategory)
            {
                item.transform.SetParent(transform);
            }
        }
    }

    void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DebugItems();
    }

    void Start()
    {
        
    }

    // item獲得時
    // id : 獲得したitemのid
    // acquisitionItemNum : 獲得した個数
    public void AcquisitionItem(int id, int acquisitionItemNum = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
            {
                var itemStatus = items[i][k].GetComponent<ItemStatus>();
                if (itemStatus.id != id)
                    continue;

                if (!itemStatus.isFirstGet)
                    itemStatus.isFirstGet = true;
                itemStatus.num = Mathf.Min(itemStatus.num + acquisitionItemNum, 99);
                return;
            }
        }

        Debug.Log("Not exist item : id");
    }

    // item使用時
    // id : 使用したitemのid
    // useNum : 使用した個数
    public void UsedItem(int id, int useNum = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
            {
                var itemStatus = items[i][k].GetComponent<ItemStatus>();
                if (itemStatus.id != id)
                    continue;

                itemStatus.num = Mathf.Max(itemStatus.num - useNum, 0);
                return;
            }
        }

        Debug.Log("Not exist item : id");
    }

    // itemの個数を知りたい時
    // id : 個数を知りたいitemのid
    public int GetItemNum(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
            {
                var itemStatus = items[i][k].GetComponent<ItemStatus>();
                if (itemStatus.id != id)
                    continue;

                return itemStatus.num;
            }
        }

        Debug.Log("Not exist item : id");
        return int.MinValue;
    }

    // itemの名前からidを知りたい時
    // name : idを知りたいitemのname
    public int GetItemId(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
            {
                var itemStatus = items[i][k].GetComponent<ItemStatus>();
                if (itemStatus.name != name)
                    continue;

                return itemStatus.id;
            }
        }

        Debug.Log("Not exist item : id");
        return int.MinValue;
    }

    // idからitemを返す
    public ItemStatus GetItem(int id)
    {
        foreach(var itemCategory in items)
        {
            foreach(var item in itemCategory)
            {
                var itemStatus = item.GetComponent<ItemStatus>();
                if (id != itemStatus.id)
                    continue;

                return itemStatus;
            }
        }

        return null;
    }
}
