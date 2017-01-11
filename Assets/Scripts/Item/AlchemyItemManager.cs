using UnityEngine;
using System.Collections;
using System;

public class AlchemyItemManager : MonoBehaviour
{
    public bool isEaseEnd = false;
    private float time = 0.0f;

    void Awake()
    {
        
    }

    void Start()
    {
        this.transform.localScale = Vector3.zero;

        StartCoroutine(UpdateAlchemyItem());
    }

    private IEnumerator UpdateAlchemyItem()
    {
        while(true)
        {
            time = Math.Min(time + Time.deltaTime, 1.0f);
            if (time >= 1.0f)
                isEaseEnd = true;

            this.transform.localScale = new Vector3(
                EaseInExpo(time, 0.0f, 1.0f),
                EaseInExpo(time, 0.0f, 1.0f),
                1.0f);

            yield return null;
        }
    }

    float EaseInExpo(float t, float b, float e)
    {
        return (t == 0) ? b : (e - b) * Mathf.Pow(2.0f, 10.0f * (t - 1.0f)) + b;
    }
}
