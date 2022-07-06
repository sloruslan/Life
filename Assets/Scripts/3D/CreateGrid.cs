using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public int CellsPerHorizontal, CellsPerVertical, Z;
    public int StartChanceOfLifeForCell;

    public GameObject[] cellPrefabs; 

    private void Start()
    {
        for (int z = 0; z < Z; z++)
        for (int y = 0; y < CellsPerVertical; y++)   
        {
            for (int x = 0; x < CellsPerHorizontal; x++)
            {
                int index;
                var obj = Instantiate(cellPrefabs[index = Random.Range(0, 100) % 2], new Vector3(x, y, z), new Quaternion(), this.transform);
                if (index == 0)
                    obj.SetActive(false);
            }
        }
    }
}
