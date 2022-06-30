using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InputFieldValue : MonoBehaviour
{
    public TMP_InputField inputField;

    public int GetValueInt
    {
        get
        {
            try
            {
                return Convert.ToInt32(inputField.text);
            }
            catch (Exception ex)
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
                var value = inputField.text.Replace('.', ',');
                return (float)Convert.ToDouble(value);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
    }
}
