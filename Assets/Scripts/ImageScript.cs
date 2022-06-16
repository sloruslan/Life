using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageScript : MonoBehaviour//, IPointerClickHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    private GenerateTextureScript _texture;
    private CellsGrid _cellsGrid;

    private void Start()
    {
        _texture = GetComponent<GenerateTextureScript>();
        _cellsGrid = GetComponent<CellsGrid>();
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
        int x = (int)position.x / _cellsGrid.PixelsByCell;
        int y = (int)position.y / _cellsGrid.PixelsByCell;

        _cellsGrid._fields[x, y] = true;
    }

    private void SetNewColorForCell(PointerEventData eventData)
    {
        int x = (int)eventData.position.x / _cellsGrid.PixelsByCell;
        int y = (int)eventData.position.y / _cellsGrid.PixelsByCell;

        _texture.SetCellColorApply(x, y, 1);
    }

    private void SetNewColorForCell(Vector3 position)
    {
        int x = (int)position.x / _cellsGrid.PixelsByCell;
        int y = (int)position.y / _cellsGrid.PixelsByCell;

        _texture.SetCellColorApply(x, y, 1);
    }
}
