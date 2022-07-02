using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMenuController : MonoBehaviour
{
    public MainUIScript MainUIScript;
    private void Update()
    {
        if (!MainUIScript.IsActive)
        {
            if (Input.touchSupported)
            {
                foreach (var touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Stationary)
                        MainUIScript.IsActive = true;
                }
            }
            else
            {
                if (Input.GetMouseButton(1))
                    MainUIScript.IsActive = true;
            }
        }


        if (Input.deviceOrientation != _currentOrientation)
        {
            _currentOrientation = Input.deviceOrientation;
            if (_currentOrientation != DeviceOrientation.Unknown)
                _lastKnowOrientation = _currentOrientation;

            Logger.Text = $"_currentOrientation: {_currentOrientation}; _lastKnowOrientation: {_lastKnowOrientation}";
        }
    }

    private DeviceOrientation _currentOrientation = DeviceOrientation.Unknown;
    private DeviceOrientation _lastKnowOrientation = DeviceOrientation.Unknown;
}
