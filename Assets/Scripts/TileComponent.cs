using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState : byte
{
    Unset = 0, MarkedX = 1, MarkedO = 2
}

public class TileComponent : MonoBehaviour
{
    [HideInInspector] public TileState tileState;
    [HideInInspector] public bool isMarked;

    public int rowIndex = -1;
    public int columnIndex = -1;

    void Awake()
    {
        tileState = TileState.Unset;
    }

    void OnDisable()
    {
        GameplayController.tiles[rowIndex, columnIndex] = TileState.Unset;
        tileState = TileState.Unset;
    }
}
