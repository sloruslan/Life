using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOrientationManager : MonoBehaviour
{
    public static void SetStartDeviceOrientationSimple()
    {
        StartDeviceOrientationSimple = GetDeviceOrientationSimple;
    }

    public static bool IsWasRotationDevice
    {
        get => StartDeviceOrientationSimple == GetDeviceOrientationSimple;
    }
    public static DeviceOrientationSimple GetDeviceOrientationSimple
    {
        get
        {
            if (Input.deviceOrientation == DeviceOrientation.LandscapeRight || Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
                return CurrentDeviceOrientationSimple = DeviceOrientationSimple.Landscape;
            else if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
                return CurrentDeviceOrientationSimple = DeviceOrientationSimple.Portrait;
            else
                return CurrentDeviceOrientationSimple;
        }
    }

    private static DeviceOrientationSimple _currentDeviceOrientationSimple = DeviceOrientationSimple.Portrait;
    public static DeviceOrientationSimple CurrentDeviceOrientationSimple
    {
        get => _currentDeviceOrientationSimple;
        set
        {
            if (_currentDeviceOrientationSimple == value) return;

            _currentDeviceOrientationSimple = value;
        }
    }

    private static DeviceOrientationSimple _startDeviceOrientationSimple;
    public static DeviceOrientationSimple StartDeviceOrientationSimple
    {
        get => _startDeviceOrientationSimple;
        set
        {
            if (_startDeviceOrientationSimple == value) return;

            _startDeviceOrientationSimple = value;
        }
    }

    public enum DeviceOrientationSimple
    {
        Portrait,
        Landscape
    }
}
