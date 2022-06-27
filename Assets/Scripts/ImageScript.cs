using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageScript : MonoBehaviour//, IPointerClickHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    private GenerateTextureManager _texture;
    private GameLifeManager _gameLifeManager;

    private void Start()
    {
        _texture = GetComponent<GenerateTextureManager>();
        _gameLifeManager = GetComponent<GameLifeManager>();
    }

    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        SetNewColorForCell(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            SetNewColorForCell(eventData);
        }
    }
    */

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetNewLife(Input.mousePosition);
        }
    }

    private void SetNewLife(Vector3 position)
    {
        int x = (int)position.x / _gameLifeManager.PixelsPerCell;
        int y = (int)position.y / _gameLifeManager.PixelsPerCell;

        ///TODO: пересмотреть
       //_gameLifeManager._fields[x, y] = true;
    }

    private void SetNewColorForCell(PointerEventData eventData)
    {
        int x = (int)eventData.position.x / _gameLifeManager.PixelsPerCell;
        int y = (int)eventData.position.y / _gameLifeManager.PixelsPerCell;

        _texture.SetCellColorApply(x, y, 1);
    }

    private void SetNewColorForCell(Vector3 position)
    {
        int x = (int)position.x / _gameLifeManager.PixelsPerCell;
        int y = (int)position.y / _gameLifeManager.PixelsPerCell;

        _texture.SetCellColorApply(x, y, 1);
    }
}
