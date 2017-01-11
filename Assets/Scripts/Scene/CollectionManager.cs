using UnityEngine;
using System.Collections;

public class CollectionManager : MonoBehaviour
{
    [SerializeField]
    private ItemBoxManager itemBoxManager = null;

    void Awake()
    {

    }

    void Start()
    {
        itemBoxManager.SetActiveItemBoxs(true);
    }

    void Update()
    {

    }
}
