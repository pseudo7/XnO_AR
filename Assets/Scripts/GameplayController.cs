using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public enum WinStreak : byte
{
    TopRow, MiddleRow, BottomRow, LeftColumn, CenterColumn, RightColumn, ForwardDiagonal, BackwardDiagonal
}

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance;
    public static bool isGameOver, isGameDraw;
    public bool didXWin;
    public WinStreak winStreak;
    public static TileState[,] tiles;
    public static AIDifficulty difficulty;
    public static int turns;

    void Awake()
    {
        if (!Instance)
            Instance = this;
        tiles = new TileState[3, 3];
    }

    public void UpdateTile(TileComponent tile)
    {
        tiles[tile.rowIndex, tile.columnIndex] = tile.tileState;
        turns++;
    }

    public bool CheckForWin()
    {
        // rows
        if ((tiles[0, 0] == TileState.MarkedX && tiles[0, 1] == TileState.MarkedX && tiles[0, 2] == TileState.MarkedX))
        { didXWin = true; isGameOver = true; winStreak = WinStreak.TopRow; return true; }

        if ((tiles[1, 0] == TileState.MarkedX && tiles[1, 1] == TileState.MarkedX && tiles[1, 2] == TileState.MarkedX))
        { didXWin = true; isGameOver = true; winStreak = WinStreak.MiddleRow; return true; }

        if ((tiles[2, 0] == TileState.MarkedX && tiles[2, 1] == TileState.MarkedX && tiles[2, 2] == TileState.MarkedX))
        { didXWin = true; isGameOver = true; winStreak = WinStreak.BottomRow; return true; }

        if ((tiles[0, 0] == TileState.MarkedO && tiles[0, 1] == TileState.MarkedO && tiles[0, 2] == TileState.MarkedO))
        { didXWin = false; isGameOver = true; winStreak = WinStreak.TopRow; return true; }

        if ((tiles[1, 0] == TileState.MarkedO && tiles[1, 1] == TileState.MarkedO && tiles[1, 2] == TileState.MarkedO))
        { didXWin = false; isGameOver = true; winStreak = WinStreak.MiddleRow; return true; }

        if ((tiles[2, 0] == TileState.MarkedO && tiles[2, 1] == TileState.MarkedO && tiles[2, 2] == TileState.MarkedO))
        { didXWin = false; isGameOver = true; winStreak = WinStreak.BottomRow; return true; }

        // columns
        if ((tiles[0, 0] == TileState.MarkedX && tiles[1, 0] == TileState.MarkedX && tiles[2, 0] == TileState.MarkedX))
        { didXWin = true; isGameOver = true; winStreak = WinStreak.LeftColumn; return true; }

        if ((tiles[0, 1] == TileState.MarkedX && tiles[1, 1] == TileState.MarkedX && tiles[2, 1] == TileState.MarkedX))
        { didXWin = true; isGameOver = true; winStreak = WinStreak.CenterColumn; return true; }

        if ((tiles[0, 2] == TileState.MarkedX && tiles[1, 2] == TileState.MarkedX && tiles[2, 2] == TileState.MarkedX))
        { didXWin = true; isGameOver = true; winStreak = WinStreak.RightColumn; return true; }

        if ((tiles[0, 0] == TileState.MarkedO && tiles[1, 0] == TileState.MarkedO && tiles[2, 0] == TileState.MarkedO))
        { didXWin = false; isGameOver = true; winStreak = WinStreak.LeftColumn; return true; }

        if ((tiles[0, 1] == TileState.MarkedO && tiles[1, 1] == TileState.MarkedO && tiles[2, 1] == TileState.MarkedO))
        { didXWin = false; isGameOver = true; winStreak = WinStreak.CenterColumn; return true; }

        if ((tiles[0, 2] == TileState.MarkedO && tiles[1, 2] == TileState.MarkedO && tiles[2, 2] == TileState.MarkedO))
        { didXWin = false; isGameOver = true; winStreak = WinStreak.RightColumn; return true; }

        // diagnals
        if ((tiles[0, 0] == TileState.MarkedX && tiles[1, 1] == TileState.MarkedX && tiles[2, 2] == TileState.MarkedX))
        { didXWin = true; isGameOver = true; winStreak = WinStreak.ForwardDiagonal; return true; }

        if ((tiles[0, 2] == TileState.MarkedX && tiles[1, 1] == TileState.MarkedX && tiles[2, 0] == TileState.MarkedX))
        { didXWin = true; isGameOver = true; winStreak = WinStreak.BackwardDiagonal; return true; }

        if ((tiles[0, 0] == TileState.MarkedO && tiles[1, 1] == TileState.MarkedO && tiles[2, 2] == TileState.MarkedO))
        { didXWin = false; isGameOver = true; winStreak = WinStreak.ForwardDiagonal; return true; }

        if ((tiles[0, 2] == TileState.MarkedO && tiles[1, 1] == TileState.MarkedO && tiles[2, 0] == TileState.MarkedO))
        { didXWin = false; isGameOver = true; winStreak = WinStreak.BackwardDiagonal; return true; }

        // check for draw
        if (turns == 9)
        { isGameDraw = true; }
        return false;
    }

    public int[,] GetWinStreakIndex(WinStreak streak)
    {
        switch (streak)
        {
            case WinStreak.TopRow:
                return new int[,] { { 0, 0 }, { 0, 1 }, { 0, 2 } };
            case WinStreak.MiddleRow:
                return new int[,] { { 1, 0 }, { 1, 1 }, { 1, 2 } };
            case WinStreak.BottomRow:
                return new int[,] { { 2, 0 }, { 2, 1 }, { 2, 2 } };
            case WinStreak.LeftColumn:
                return new int[,] { { 0, 0 }, { 1, 0 }, { 2, 0 } };
            case WinStreak.CenterColumn:
                return new int[,] { { 0, 1 }, { 1, 1 }, { 2, 1 } };
            case WinStreak.RightColumn:
                return new int[,] { { 0, 2 }, { 1, 2 }, { 2, 2 } };
            case WinStreak.ForwardDiagonal:
                return new int[,] { { 0, 0 }, { 1, 1 }, { 2, 2 } };
            case WinStreak.BackwardDiagonal:
                return new int[,] { { 0, 2 }, { 1, 1 }, { 2, 0 } };
        }
        return new int[,] { { -1, -1 }, { -1, -1 }, { -1, -1 } };
    }

    public int[] GetTransformIndeces(WinStreak streak)
    {
        switch (streak)
        {
            case WinStreak.TopRow:
                return new int[] { 0, 1, 2 };
            case WinStreak.MiddleRow:
                return new int[] { 3, 4, 5 };
            case WinStreak.BottomRow:
                return new int[] { 6, 7, 8 };
            case WinStreak.LeftColumn:
                return new int[] { 0, 3, 6 };
            case WinStreak.CenterColumn:
                return new int[] { 1, 4, 7 };
            case WinStreak.RightColumn:
                return new int[] { 2, 5, 8 };
            case WinStreak.ForwardDiagonal:
                return new int[] { 0, 4, 8 };
            case WinStreak.BackwardDiagonal:
                return new int[] { 2, 4, 6 };
        }
        return null;
    }

}
