using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
        //_texture2D = new Texture2D(_widthTexturePerPixels, _heightTexturePerPixels, TextureFormat.ARGB32, false);

        _sprite = Sprite.Create(_texture2D, new Rect(0f, 0f, _widthTexturePerPixels, _heightTexturePerPixels), new Vector2(0.5f, 0.5f), 32);
        _sprite.name = "mySprite";
       
        //Debug.Log("pixelsPerUnit " + _sprite.pixelsPerUnit);

        _spriteRenderer = null;
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


    public void SetTextureColorStreamApply(byte[] textureData)
    {
        try
        {
            _texture2D.SetPixelData(textureData, 0);
            _texture2D.Apply();
        }
        catch (System.Exception ex)
        {
            Logger.Text = "Exception GameRender::SetTextureColorStreamApply: " + ex.Message;
            Logger.SetActive(true);
        }
    }

    public void SetTextureColorStreamApply(int[] textureData)
    {
        try
        {
            _texture2D.SetPixelData(textureData, 0);
            _texture2D.Apply();
        }
        catch (System.Exception ex)
        {
            Logger.Text = "Exception GameRender::SetTextureColorStreamApply: " + ex.Message;
            Logger.SetActive(true);
        }
    }

    public void SetTextureColorStreamApply(NativeArray<byte> textureData)
    {
        try
        {
            _texture2D.SetPixelData(textureData, 0);
            _texture2D.Apply();
        }
        catch (System.Exception ex)
        {
            Logger.Text = "Exception GameRender::SetTextureColorStreamApply: " + ex.Message;
            Logger.SetActive(true);
        }
    }

    public void SetTextureColorStreamApply(NativeArray<int> textureData)
    {
        try
        {
            _texture2D.SetPixelData(textureData, 0);
            _texture2D.Apply();
        }
        catch (System.Exception ex)
        {
            Logger.Text = "Exception GameRender::SetTextureColorStreamApply: " + ex.Message;
            Logger.SetActive(true);
        }
    }
}
