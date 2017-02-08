using UnityEngine;
using System.Collections;

public class CollectionManager : MonoBehaviour
{
    [SerializeField]
    private ItemBoxManager itemBoxManager = null;
    [SerializeField]
    private GameObject back = null;
    [SerializeField]
    private GameObject front = null;

    void Awake()
    {

    }

    void Start()
    {
        itemBoxManager.SetActiveItemBoxs(true);
    }

    void Update()
    {
        var button = back.GetComponent<ButtonManager>();
        if (back.activeInHierarchy)
            button.UpdateButton();
        if (button.isPushed)
        {
            button.isPushed = false;
            itemBoxManager.ChangeDisplayItemCategory(-1);
        }

        button = front.GetComponent<ButtonManager>();
        if (front.activeInHierarchy)
            button.UpdateButton();
        if(button.isPushed)
        {
            button.isPushed = false;
            itemBoxManager.ChangeDisplayItemCategory(1);
        }
    }
}
