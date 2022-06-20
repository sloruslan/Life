using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(ColorsManager))]
public class GenerateTextureManager : MonoBehaviour
{
    private ColorsManager _colorsManager;

    private Texture2D _texture2D;
    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private Sprite _sprite;

    private int _widthTexturePerPixels, _heightTexturePerPixels, _cellSizePerPixel;
    private int _cellsPerHorizontal;

    public void Init(GameLifeManager gameLifeManager)
    {
        _cellSizePerPixel = gameLifeManager.PixelsPerCell;
        _widthTexturePerPixels = gameLifeManager.PixelsPerHorizontal;
        _heightTexturePerPixels = gameLifeManager.PixelsPerVercital;
        _cellsPerHorizontal = gameLifeManager.CellsPerHorizontal;

        _colorsManager = GetComponent<ColorsManager>();
        _colorsManager.Init(_cellSizePerPixel);

        _texture2D = new Texture2D(_widthTexturePerPixels, _heightTexturePerPixels, TextureFormat.RGB24, false);
        
        _sprite = Sprite.Create(_texture2D, new Rect(0f, 0f, _widthTexturePerPixels, _heightTexturePerPixels), new Vector2(0.5f, 0.5f));
        _sprite.name = "mySprite";

        if ((_spriteRenderer = GetComponent<SpriteRenderer>()) != null)
        {
            _spriteRenderer.sprite = _sprite;
        }
        else
        {
            _image = GetComponent<Image>();
            _image.sprite = _sprite;
        }
    }

    public void SetTextureColor(int indexOfColorsArray)
    {
        for (int x = 0; x < _widthTexturePerPixels; x+= _cellSizePerPixel)
            for (int y = 0; y < _heightTexturePerPixels; y+= _cellSizePerPixel)
                _texture2D.SetPixels(x, y, _cellSizePerPixel, _cellSizePerPixel, _colorsManager[indexOfColorsArray]);
    }

    

    public void SetTextureColorApply(int indexOfColorsArray)
    {
        SetTextureColor(indexOfColorsArray);
        _texture2D.Apply();
    }

    public void SetCellColor(int xCell, int yCell, int indexOfColorsArray)
    {
        _texture2D.SetPixels(xCell * _cellSizePerPixel, yCell * _cellSizePerPixel, _cellSizePerPixel, _cellSizePerPixel, _colorsManager[indexOfColorsArray]);
    }

    public void Apply()
    {
        _texture2D.Apply();
    }

    public void SetCellColorApply(int xCell, int yCell, int indexOfColorsArray)
    {
        SetCellColor(xCell, yCell, indexOfColorsArray);
        _texture2D.Apply();
    }

    public void SetTextureColorApply(CellsBase field)
    {
        for (int x = 0; x < _widthTexturePerPixels; x += _cellSizePerPixel)
            for (int y = 0; y < _heightTexturePerPixels; y += _cellSizePerPixel)
                _texture2D.SetPixels(x, y, _cellSizePerPixel, _cellSizePerPixel, _colorsManager[field[x + y * _cellsPerHorizontal]]);
        _texture2D.Apply();
    }

    public void SetTextureColorStreamApply(CellsBase cells)
    {
        _texture2D.SetPixelData(cells.ArrayColor, 0);
        _texture2D.Apply();
    }

}
