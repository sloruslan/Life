using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InputFieldValue : MonoBehaviour
{
    private TMP_InputField _inputField;

    public float FontSize
    {
        get => InputField == null ? 14 : _inputField.textComponent.fontSize;
        set => _inputField.textComponent.fontSizeMax = InputField == null ? 14 : value;
    }

    public TMP_InputField InputField
    {
        get
        {
            if (_inputField == null)
                _inputField = GetComponent<TMP_InputField>();

            return _inputField;
        }
    }

    public int GetValueInt
    {
        get
        {
            try
            {
                return Convert.ToInt32(_inputField.text);
            }
            catch 
            {
                return 0;
            }
        }
    }

    public float GetValueFloat
    {
        get
        {
            try
            {
                var value = _inputField.text.Replace('.', ',');
                return (float)Convert.ToDouble(value);
            }
            catch 
            {
                return 0;
            }
        }
    }
}
