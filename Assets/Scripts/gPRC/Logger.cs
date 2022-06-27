using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Logger : MonoBehaviour
{
    private static Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    public static string Text
    {
        get { return _text.text; }
        set { _text.text += DateTime.Now.ToLongTimeString() + ": " + value + Environment.NewLine; }
    }
}
