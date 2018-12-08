using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XTile : MonoBehaviour
{
    void OnEnable()
    {
        TileComponent tile = GetComponentInParent<TileComponent>();
        tile.tileState = TileState.MarkedX;
        GameplayController.tiles[tile.rowIndex, tile.columnIndex] = TileState.MarkedX;
    }
}
