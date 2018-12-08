using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTile : MonoBehaviour
{
    void OnEnable()
    {
        TileComponent tile = GetComponentInParent<TileComponent>();
        tile.tileState = TileState.MarkedO;
        GameplayController.tiles[tile.rowIndex, tile.columnIndex] = TileState.MarkedO;
    }
}
