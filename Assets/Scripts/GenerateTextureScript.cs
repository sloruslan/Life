using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CellsGrid))]
[RequireComponent(typeof(ColorsManager))]
public class GenerateTextureScript : MonoBehaviour
{
    private CellsGrid _cellsGrid;
    private ColorsManager _colorsManager;

    private Texture2D _texture2D;
    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private Sprite _sprite;

    private int _widthTextureByPixels, _heightTextureByPixels, _cellSizeByPixel;   

    private void Start()
    {
        if ((_spriteRenderer = GetComponent<SpriteRenderer>()) == null)
            _image = GetComponent<Image>();

        _cellsGrid = GetComponent<CellsGrid>();
        _colorsManager = GetComponent<ColorsManager>();

        _cellSizeByPixel = _cellsGrid.PixelsByCell;
        _widthTextureByPixels = _cellsGrid.PixelsByHorizontal;
        _heightTextureByPixels = _cellsGrid.PixelsByVercital;
       

        _texture2D = new Texture2D(_widthTextureByPixels, _heightTextureByPixels);
        SetTextureColor(0);

        _sprite = Sprite.Create(_texture2D, new Rect(0f, 0f, _widthTextureByPixels, _heightTextureByPixels), new Vector2(0.5f, 0.5f));
        _sprite.name = "mySprite";

        if (_spriteRenderer != null)
            _spriteRenderer.sprite = _sprite;
        else
            _image.sprite = _sprite;
    }

    public void SetTextureColor(int indexOfColorsArray)
    {
        for (int x = 0; x < _widthTextureByPixels; x+= _cellSizeByPixel)
            for (int y = 0; y < _heightTextureByPixels; y+= _cellSizeByPixel)
                _texture2D.SetPixels(x, y, _cellSizeByPixel, _cellSizeByPixel, _colorsManager[indexOfColorsArray]);

        _texture2D.Apply();
    }

    public void SetCellColor(int xCell, int yCell, int indexOfColorsArray)
    {
        _texture2D.SetPixels(xCell * _cellSizeByPixel, yCell * _cellSizeByPixel, _cellSizeByPixel, _cellSizeByPixel, _colorsManager[indexOfColorsArray]);
    }

    public void Apply()
    {
        _texture2D.Apply();
    }

    public void SetCellColorApply(int xCell, int yCell, int indexOfColorsArray)
    {
        _texture2D.SetPixels(xCell * _cellSizeByPixel, yCell * _cellSizeByPixel, _cellSizeByPixel, _cellSizeByPixel, _colorsManager[indexOfColorsArray]);
        _texture2D.Apply();
    }
}
