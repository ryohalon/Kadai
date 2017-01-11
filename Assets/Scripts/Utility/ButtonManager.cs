using UnityEngine;
using System.Collections;
using System;

public class ButtonManager : MonoBehaviour
{
    private RectTransform myrectTrans = null;
    private Vector2 mousePos = Vector2.zero;

    Vector2 canvasRefferenceResolution = Vector2.zero;
    Vector2 windowSizeRatio = Vector2.zero;

    public bool isPushed = false;

    void Awake()
    {
        myrectTrans = GetComponent<RectTransform>();

        var canvas = GameObject.Find("Canvas");
        var canvasPixelSize = canvas.GetComponent<Canvas>().pixelRect.size;
        canvasRefferenceResolution = canvas.GetComponent<RectTransform>().rect.size;
        windowSizeRatio.x = canvasRefferenceResolution.x / canvasPixelSize.x;
        windowSizeRatio.y = canvasRefferenceResolution.y / canvasPixelSize.y;
    }

    void Start()
    {

    }

    public void UpdateButton()
    {
        mousePos = new Vector2(Input.mousePosition.x * windowSizeRatio.x, Input.mousePosition.y * windowSizeRatio.y) - canvasRefferenceResolution / 2.0f;

        if (isPushed)
        {
            isPushed = false;
        }
        else
        {
            isPushed = IsPush();
        }
    }

    private bool IsPush()
    {
        if (!Input.GetMouseButtonDown(0))
            return false;

        if (mousePos.x < myrectTrans.localPosition.x - myrectTrans.rect.width / 2.0f ||
            mousePos.x > myrectTrans.localPosition.x + myrectTrans.rect.width / 2.0f)
            return false;
        if (mousePos.y < myrectTrans.localPosition.y - myrectTrans.rect.height / 2.0f ||
            mousePos.y > myrectTrans.localPosition.y + myrectTrans.rect.height / 2.0f)
            return false;

        return true;
    }
}
