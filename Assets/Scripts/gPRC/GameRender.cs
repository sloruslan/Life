using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRender : MonoBehaviour
{
    private Texture2D _texture2D;
    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private Sprite _sprite;

    private int _widthTexturePerPixels, _heightTexturePerPixels, _cellSizePerPixel;
    private int _cellsPerHorizontal;

    public void Init(GameLoop gameLoop)
    {
        _cellSizePerPixel = gameLoop.PixelsPerCell;
        _widthTexturePerPixels = gameLoop.PixelsPerHorizontal;
        _heightTexturePerPixels = gameLoop.PixelsPerVercital;
        _cellsPerHorizontal = gameLoop.CellsPerHorizontal;

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

        Logger.Text = "GameRender is completed";
    }


    public void SetTextureColorStreamApply(byte[] cells)
    {
        _texture2D.SetPixelData(cells, 0);
        _texture2D.Apply();
        Logger.Text = "SetTextureColorStreamApply is completed";
    }
}