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

    private static int _countOfMessage = 0;

    public static string Text
    {
        get { return _text.text; }
        set 
        {
            if (_countOfMessage == 100)
            {
                _text.text = string.Empty;
                _countOfMessage = 0;
            }

            _text.text += _countOfMessage.ToString("D3") + " " + DateTime.Now.ToLongTimeString() + ": " + value + Environment.NewLine; 
            _countOfMessage++; 
        }
    }

    public static void SetActive(bool value)
    {
        _rectTransformMain.gameObject.SetActive(value);
    }

    public static void SetActive()
    {
        _rectTransformMain.gameObject.SetActive(!_rectTransformMain.gameObject.activeSelf);
    }
}
