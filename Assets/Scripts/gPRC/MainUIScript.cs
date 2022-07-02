using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MainUIScript : UIBehaviour
{
    public GameLoop gameLoop;

    public InputFieldValue tick;
    public InputFieldValue pixelPerCell;
    public InputFieldValue cellsH;
    public InputFieldValue cellsV;
    public InputFieldValue pixH;
    public InputFieldValue pixV;

    public RectTransform mainUI;
    public void StartGame()
    {
        float timeOfTick = tick.GetValueFloat;
        int pixelsPerCell = pixelPerCell.GetValueInt;
        int cellsPerHorizontal = cellsH.GetValueInt;
        int cellsPerVertical = cellsV.GetValueInt;
        int pixelsPerHorizontal = pixH.GetValueInt;
        int pixelsPerVercital = pixV.GetValueInt;

        timeOfTick = timeOfTick == 0 ? 0.1f : timeOfTick;
        pixelsPerCell = pixelsPerCell == 0 ? 8 : pixelsPerCell;

        gameLoop.Init(timeOfTick, pixelsPerCell, cellsPerHorizontal, cellsPerVertical, pixelsPerHorizontal, pixelsPerVercital);

        IsActive = false;
    }

    public void DisableMainUI()
    {
        IsActive = false;
    }

    public bool IsActive 
    { 
        get => mainUI.gameObject.activeSelf; 
        set 
        { 
            if (mainUI.gameObject.activeSelf == value) return;  
            mainUI.gameObject.SetActive(value);

            if (value)
                UpdateFontSize();
        } 
    }

    protected override void OnRectTransformDimensionsChange()
    {
        UpdateFontSize();
        base.OnRectTransformDimensionsChange();
    }

    public void UpdateFontSize()
    {
        if (tick.InputField == null || pixelPerCell.InputField == null || cellsH.InputField == null || cellsV.InputField == null || pixH.InputField == null || pixV.InputField == null)
            return;

        float[] sizes = new float[6];

        sizes[0] = tick.FontSize;
        sizes[1] = pixelPerCell.FontSize;
        sizes[2] = cellsH.FontSize;
        sizes[3] = cellsV.FontSize;
        sizes[4] = pixH.FontSize;
        sizes[5] = pixV.FontSize;

        float max = sizes[0];
        foreach (var size in sizes)
        {
            if (size > max)
                max = size;
        }

        tick.FontSize = pixelPerCell.FontSize = cellsH.FontSize = cellsV.FontSize = pixH.FontSize = pixV.FontSize = max;
    }
}
