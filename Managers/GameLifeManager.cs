using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateTextureManager))]
public class GameLifeManager : MonoBehaviour
{
    [Range(0f, 2f)]
    public float TimeOfTick = 0.1f;

    private GenerateTextureManager _texture;

    public int CellsPerHorizontal = -1;
    public int CellsPerVertical = -1;

    public int PixelsPerHorizontal;
    public int PixelsPerVercital;
    public int PixelsPerCell = 8;

    public int StartChanceOfLifeForCell = 50;

    private GameLife _gameLife;

    public void Awake()
    {
        PixelsPerHorizontal = Screen.width - Screen.width % PixelsPerCell;
        PixelsPerVercital = Screen.height - Screen.height % PixelsPerCell;

        CellsPerHorizontal = PixelsPerHorizontal / PixelsPerCell;
        CellsPerVertical = PixelsPerVercital / PixelsPerCell;
    }

    private void Start()
    {
        _texture = GetComponent<GenerateTextureManager>();
        _gameLife = new GameLife(CellsPerHorizontal, PixelsPerVercital, StartChanceOfLifeForCell);
    }



    private IEnumerator TickRate()
    {
        if (TimeOfTick == 0f)
            yield return null;
        else
            yield return new WaitForSeconds(TimeOfTick);

        //
        //NextGeneration();

        StartCoroutine(TickRate());
    }
}
