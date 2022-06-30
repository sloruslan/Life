using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIScript : MonoBehaviour
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

        mainUI.gameObject.SetActive(false);
    }
}
