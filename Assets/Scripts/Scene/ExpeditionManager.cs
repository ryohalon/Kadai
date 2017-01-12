using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ExpeditionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject goExpedition = null;
    [SerializeField]
    private GameObject map = null;
    [SerializeField]
    private List<GameObject> stages = new List<GameObject>();
    [SerializeField]
    private List<Sprite> stageSprites = new List<Sprite>();
    [SerializeField]
    private GameObject selectStage = null;
    [SerializeField]
    private GameObject timeDisplay = null;
    private Timer timer = null;
    private ItemManager itemManager = null;
    [SerializeField]
    private GameObject successDegreeLogo = null;
    private GameObject Logo = null;
    [SerializeField]
    private List<Sprite> logos = new List<Sprite>();

    public int minGetItemNum = 6;
    public int maxGetItemNum = 12;

    public enum ActionStatus
    {
        DEFAULT,
        SELECTMAP,
        GOINGEXPEDITION,
        RETURN
    }
    private ActionStatus status = ActionStatus.DEFAULT;

    void Awake()
    {
        status = ActionStatus.DEFAULT;
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (goExpedition.activeInHierarchy)
            goExpedition.GetComponent<ButtonManager>().UpdateButton();
        foreach (var stage in stages)
        {
            if (stage.activeInHierarchy)
                stage.GetComponent<ButtonManager>().UpdateButton();
        }
        if (selectStage.activeInHierarchy)
        {
            selectStage.transform.GetChild(1).GetComponent<ButtonManager>().UpdateButton();
            selectStage.transform.GetChild(2).GetComponent<ButtonManager>().UpdateButton();
        }

        GoExpedition();
        SelectMap();
        GoingExpedition();
        Return();
    }

    void GoExpedition()
    {
        if (status != ActionStatus.DEFAULT)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;
        var button = goExpedition.GetComponent<ButtonManager>();
        if (!button.isPushed)
            return;

        button.isPushed = false;
        status = ActionStatus.SELECTMAP;

        goExpedition.SetActive(false);
        map.SetActive(true);
        foreach (var stage in stages)
            stage.SetActive(true);
    }

    void SelectMap()
    {
        if (status != ActionStatus.SELECTMAP)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;

        ButtonManager button;

        if (!selectStage.activeInHierarchy)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                button = stages[i].GetComponent<ButtonManager>();
                if (!button.isPushed)
                    continue;

                selectStage.SetActive(true);
                selectStage.transform.GetChild(0).GetComponent<Image>().sprite =
                    stageSprites[i];
            }
        }
        else
        {
            for (int i = 1; i < selectStage.transform.childCount; i++)
            {
                button = selectStage.transform.GetChild(i).GetComponent<ButtonManager>();
                if (!button.isPushed)
                    continue;

                if (i == 1)
                {
                    button.isPushed = false;

                    map.SetActive(false);
                    selectStage.SetActive(false);
                    foreach (var stage in stages)
                        stage.SetActive(false);

                    timeDisplay.SetActive(true);

                    status = ActionStatus.GOINGEXPEDITION;

                    return;
                }
                else if (i == 2)
                {
                    button.isPushed = false;
                    selectStage.SetActive(false);

                    return;
                }
            }
        }
    }

    void GoingExpedition()
    {
        if (status != ActionStatus.GOINGEXPEDITION)
            return;
        if (!timer.isReturn)
            return;

        timeDisplay.SetActive(false);

        int successDegree = UnityEngine.Random.Range(0, 2);

        Logo = Instantiate(successDegreeLogo);
        Logo.transform.SetParent(GameObject.Find("Canvas").transform);
        Logo.transform.localPosition = Vector3.zero;
        Logo.transform.localScale = Vector3.one;
        Logo.GetComponent<Image>().sprite = logos[successDegree];

        int getItemNum = 0;
        if (successDegree == 0)
            getItemNum = minGetItemNum;
        else if (successDegree == 1)
            getItemNum = UnityEngine.Random.Range(minGetItemNum + 1, maxGetItemNum - 1);
        else if (successDegree == 2)
            getItemNum = maxGetItemNum;

        for (int i = 0; i < getItemNum; i++)
        {
            int getItemLevel = UnityEngine.Random.Range(0, 5);
            getItemLevel -= UnityEngine.Random.Range(0, getItemLevel);

            int id = itemManager.GetIdInCategoryItemLevel(timer.expeditionStageNum, getItemLevel);
            itemManager.AcquisitionItem(id);
        }

        status = ActionStatus.RETURN;
    }

    void Return()
    {
        if (status != ActionStatus.RETURN)
            return;
        if (!Logo.GetComponent<AlchemyItemManager>().isEaseEnd)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;

        Destroy(Logo);

        status = ActionStatus.DEFAULT;

        goExpedition.SetActive(true);
    }
}
