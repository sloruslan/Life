using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Logger : MonoBehaviour
{
    private static Text _text;
    public RectTransform RectTransformMain;
    private static RectTransform _rectTransformMain;

    private void Awake()
    {
        _text = GetComponent<Text>();
        _rectTransformMain = RectTransformMain;
    }

    private void Start()
    {
        SetActive(false);
    }
    public static string Text
    {
        get { return _text.text; }
        set { _text.text += DateTime.Now.ToLongTimeString() + ": " + value + Environment.NewLine; }
    }

    public static void SetActive(bool value)
    {
        _rectTransformMain.gameObject.SetActive(value);
    }
}
