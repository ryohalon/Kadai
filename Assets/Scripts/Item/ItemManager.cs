using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine.UI;
using System.Text;


public class ItemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemBase = null;
    public List<List<GameObject>> items = new List<List<GameObject>>();

    [SerializeField]
    public List<Sprite> sprites = new List<Sprite>();

    public int itemMaxId = 0;

    private ItemManager instance = null;

    private IEnumerator LoadItem2()
    {
        TextAsset csvFile = Resources.Load("Data/Item") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        List<string[]> itemDatas = new List<string[]>();
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            itemDatas.Add(line.Split(','));
        }

        foreach (var itemCategory in itemDatas)
        {
            List<GameObject> itemCategory_ = new List<GameObject>();
            for (int i = 0; i < 9; i++)
                itemCategory_.Add(Instantiate(itemBase));

            for (int i = 0; i < 9; i++)
            {
                var itemStatus = itemCategory_[i].GetComponent<ItemStatus>();

                itemStatus.id = int.Parse(itemCategory[2 * i + 0]);
                itemStatus.level = int.Parse(itemCategory[2 * i + 1]);
            }

            items.Add(itemCategory_);
        }

        string url = "http://localhost:57223/api/Sample";
        WWW www = new WWW(url);

        yield return www;
        
        List<List<int>> nums = Json.Deserialize(www.text) as List<List<int>>;

        Debug.Log(nums);
        Debug.Log("i : " + nums.Count);
        Debug.Log("k : " + nums[0].Count);

        for (int i = 0; i < items.Count; i++)
        {
            for(int k = 0; k < items[i].Count; k++)
            {
                Debug.Log("i : " + i);
                Debug.Log("k : " + k);
                items[i][k].GetComponent<ItemStatus>().num = nums[i][k];
            }
        }
    }

    void LoadItem()
    {
        TextAsset csvFile = Resources.Load("Data/Item") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        List<string[]> itemDatas = new List<string[]>();
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            itemDatas.Add(line.Split(','));
        }

        foreach (var itemCategory in itemDatas)
        {
            List<GameObject> itemCategory_ = new List<GameObject>();
            for (int i = 0; i < 9; i++)
                itemCategory_.Add(Instantiate(itemBase));

            for (int i = 0; i < 9; i++)
            {
                var itemStatus = itemCategory_[i].GetComponent<ItemStatus>();

                itemStatus.id = int.Parse(itemCategory[3 * i + 0]);
                itemStatus.level = int.Parse(itemCategory[3 * i + 1]);
                itemStatus.num = int.Parse(itemCategory[3 * i + 2]);
            }

            items.Add(itemCategory_);
        }

        csvFile = Resources.Load("Data/ItemFirstGet") as TextAsset;
        reader = new StringReader(csvFile.text);

        List<string[]> Datas = new List<string[]>();
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            Datas.Add(line.Split(','));
        }

        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
            {
                items[i][k].GetComponent<ItemStatus>().isFirstGet = bool.Parse(Datas[i][k]);
            }
        }

        foreach (var itemCategory in items)
        {
            foreach (var item in itemCategory)
            {
                item.transform.SetParent(transform);
            }
        }


        for(int i = 0; i < sprites.Count / 9; i++)
        {
            for (int k = 0; k < 9; k++)
            {
                items[i][k].GetComponent<ItemStatus>().sprite = sprites[i * 9 + k];
            }
        }
    }

    IEnumerator Save()
    {
        Dictionary<string, string> header = new Dictionary<string, string>
        {
            {"Content-Type", "applicatioin/json" }
        };

        string json = "{";
        for (int i = 0; i < items.Count; i++)
        {
            for (int k = 0; k < items[i].Count; k++)
                json += "\"itemNum[" + i.ToString() + "][" + k.ToString() + "]\":" + items[i][k].GetComponent<ItemStatus>().num;
        }
        json += "}";

        byte[] data = Encoding.UTF8.GetBytes(json);

        string url = "http://localhost:57223/api/Sample";
        var wwwPost = new WWW(url, data, header);
        yield return wwwPost;
    }

    public void OnApplicationQuit()
    {
        StartCoroutine(Save());

        //StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/Data/Item.csv", false);
        //for (int i = 0; i < items.Count; i++)
        //{
        //    string txt = "";
        //    for (int k = 0; k < items[i].Count; k++)
        //    {
        //        var itemStatus = items[i][k].GetComponent<ItemStatus>();
        //        txt += itemStatus.id.ToString() + ','
        //            + itemStatus.level.ToString() + ','
        //            + itemStatus.num.ToString();

        //        if (k != items[i].Count - 1)
        //            txt += ',';
        //    }

        //    sw.WriteLine(txt);
        //}

        //sw.Flush();
        //sw.Close();

        StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/Data/ItemFirstGet.csv", false);
        for (int i = 0; i < items.Count; i++)
        {
            string txt = "";
            for (int k = 0; k < items[i].Count; k++)
            {
                var itemStatus = items[i][k].GetComponent<ItemStatus>();
                txt += itemStatus.isFirstGet.ToString();

                if (k != items[i].Count - 1)
                    txt += ',';
            }

            sw.WriteLine(txt);
        }

        sw.Flush();
        sw.Close();
    }

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


    }

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        StartCoroutine(LoadItem2());
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
        foreach (var itemCategory in items)
        {
            foreach (var item in itemCategory)
            {
                var itemStatus = item.GetComponent<ItemStatus>();
                if (id != itemStatus.id)
                    continue;

                return itemStatus;
            }
        }

        return null;
    }

    public int GetIdInCategoryItemLevel(int category, int level)
    {
        foreach (var item in items[category])
        {
            var itemStatus = item.GetComponent<ItemStatus>();
            if (itemStatus.level != level)
                continue;

            return itemStatus.id;
        }

        return -1;
    }
}