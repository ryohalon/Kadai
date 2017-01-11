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
    private List<Sprite> stageSprite = new List<Sprite>();

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
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (goExpedition.activeInHierarchy)
            goExpedition.GetComponent<ButtonManager>().UpdateButton();

        GoExpedition();
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




        status = ActionStatus.GOINGEXPEDITION;
    }

    void GoingExpedition()
    {
        if (status != ActionStatus.GOINGEXPEDITION)
            return;



        status = ActionStatus.RETURN;
    }

    void Return()
    {
        if (status != ActionStatus.RETURN)
            return;




        status = ActionStatus.DEFAULT;


        goExpedition.SetActive(true);
    }
}
