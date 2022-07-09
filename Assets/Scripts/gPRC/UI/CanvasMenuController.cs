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
    }

    
}
