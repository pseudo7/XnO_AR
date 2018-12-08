using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIDifficulty
{
    Easy, Medium, Hard
}

public class AIController : MonoBehaviour
{
    public static AIController Instance;
    public Transform parentTransform;
    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void Reset()
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);
            child.gameObject.SetActive(true);
            TileComponent tile = child.GetComponent<TileComponent>();
            if (tile)
                tile.isMarked = false;
            for (int j = 0; j < child.childCount; j++)
                child.GetChild(j).gameObject.SetActive(false);
        }

        GameplayController.isGameDraw = false;
        GameplayController.isGameOver = false;
        GameplayController.turns = 0;
        ReticleClickControllerAI.isPlayerTurn = true;
        UIManager.Instance.winText.text = "";
        UIManager.Instance.retryButton.gameObject.SetActive(false);
        UIManager.Instance.captureButton.gameObject.SetActive(true);
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                GameplayController.tiles[i, j] = TileState.Unset;
    }

    void EasyMove()
    {
        Transform childTransform = parentTransform.GetChild(Random.Range(0, 9));
        while (childTransform.GetComponent<TileComponent>().isMarked && GameplayController.turns < 8)
            childTransform = parentTransform.GetChild(Random.Range(0, 9));
        UpdateUIMark(childTransform);
    }

    bool StopMove()
    {
        // center tile
        if (!parentTransform.GetChild(4).GetComponent<TileComponent>().isMarked)
        {
            UpdateUIMark(parentTransform.GetChild(4));
            return true;
        }

        // edge tiles
        int[] indeces = { 1, 3, 5, 7 };
        ShuffleArray(indeces);
        if (parentTransform.GetChild(0).GetComponent<TileComponent>().isMarked || parentTransform.GetChild(2).GetComponent<TileComponent>().isMarked || parentTransform.GetChild(6).GetComponent<TileComponent>().isMarked || parentTransform.GetChild(8).GetComponent<TileComponent>().isMarked)
        {
            if (!parentTransform.GetChild(indeces[0]).GetComponent<TileComponent>().isMarked)
            {
                UpdateUIMark(parentTransform.GetChild(indeces[0]));
                return true;
            }
            if (!parentTransform.GetChild(indeces[1]).GetComponent<TileComponent>().isMarked)
            {
                UpdateUIMark(parentTransform.GetChild(indeces[1]));
                return true;
            }
            if (!parentTransform.GetChild(indeces[2]).GetComponent<TileComponent>().isMarked)
            {
                UpdateUIMark(parentTransform.GetChild(indeces[2]));
                return true;
            }
            if (!parentTransform.GetChild(indeces[3]).GetComponent<TileComponent>().isMarked)
            {
                UpdateUIMark(parentTransform.GetChild(indeces[3]));
                return true;
            }
        }

        // corner tiles
        indeces = new int[] { 0, 2, 6, 8 };
        ShuffleArray(indeces);

        if (!parentTransform.GetChild(indeces[0]).GetComponent<TileComponent>().isMarked)
        {
            UpdateUIMark(parentTransform.GetChild(indeces[0]));
            return true;
        }
        if (!parentTransform.GetChild(indeces[1]).GetComponent<TileComponent>().isMarked)
        {
            UpdateUIMark(parentTransform.GetChild(indeces[1]));
            return true;
        }
        if (!parentTransform.GetChild(indeces[2]).GetComponent<TileComponent>().isMarked)
        {
            UpdateUIMark(parentTransform.GetChild(indeces[2]));
            return true;
        }
        if (!parentTransform.GetChild(indeces[3]).GetComponent<TileComponent>().isMarked)
        {
            UpdateUIMark(parentTransform.GetChild(indeces[3]));
            return true;
        }
        return false;
    }

    bool CheckForTwoThird(TileState checkState)
    {

        // rows
        // top row
        if ((GameplayController.tiles[0, 0] == TileState.Unset && GameplayController.tiles[0, 1] == checkState && GameplayController.tiles[0, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(0, 0));
            return true;
        }
        if ((GameplayController.tiles[0, 0] == checkState && GameplayController.tiles[0, 1] == TileState.Unset && GameplayController.tiles[0, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(0, 1));
            return true;
        }
        if ((GameplayController.tiles[0, 0] == checkState && GameplayController.tiles[0, 1] == checkState && GameplayController.tiles[0, 2] == TileState.Unset))
        {
            UpdateUIMark(GetChildTransformFromIndex(0, 2));
            return true;
        }
        // middle row
        if ((GameplayController.tiles[1, 0] == TileState.Unset && GameplayController.tiles[1, 1] == checkState && GameplayController.tiles[1, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(1, 0));
            return true;
        }
        if ((GameplayController.tiles[1, 0] == checkState && GameplayController.tiles[1, 1] == TileState.Unset && GameplayController.tiles[1, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(1, 1));
            return true;
        }
        if ((GameplayController.tiles[1, 0] == checkState && GameplayController.tiles[1, 1] == checkState && GameplayController.tiles[1, 2] == TileState.Unset))
        {
            UpdateUIMark(GetChildTransformFromIndex(1, 2));
            return true;
        }
        // bottom row
        if ((GameplayController.tiles[2, 0] == TileState.Unset && GameplayController.tiles[2, 1] == checkState && GameplayController.tiles[2, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(2, 0));
            return true;
        }
        if ((GameplayController.tiles[2, 0] == checkState && GameplayController.tiles[2, 1] == TileState.Unset && GameplayController.tiles[2, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(2, 1));
            return true;
        }
        if ((GameplayController.tiles[2, 0] == checkState && GameplayController.tiles[2, 1] == checkState && GameplayController.tiles[2, 2] == TileState.Unset))
        {
            UpdateUIMark(GetChildTransformFromIndex(2, 2));
            return true;
        }

        // columns
        // left column
        if ((GameplayController.tiles[0, 0] == TileState.Unset && GameplayController.tiles[1, 0] == checkState && GameplayController.tiles[2, 0] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(0, 0));
            return true;
        }
        if ((GameplayController.tiles[0, 0] == checkState && GameplayController.tiles[1, 0] == TileState.Unset && GameplayController.tiles[2, 0] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(1, 0));
            return true;
        }
        if ((GameplayController.tiles[0, 0] == checkState && GameplayController.tiles[1, 0] == checkState && GameplayController.tiles[2, 0] == TileState.Unset))
        {
            UpdateUIMark(GetChildTransformFromIndex(2, 0));
            return true;
        }
        // center column
        if ((GameplayController.tiles[0, 1] == TileState.Unset && GameplayController.tiles[1, 1] == checkState && GameplayController.tiles[2, 1] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(0, 1));
            return true;
        }
        if ((GameplayController.tiles[0, 1] == checkState && GameplayController.tiles[1, 1] == TileState.Unset && GameplayController.tiles[2, 1] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(1, 1));
            return true;
        }
        if ((GameplayController.tiles[0, 1] == checkState && GameplayController.tiles[1, 1] == checkState && GameplayController.tiles[2, 1] == TileState.Unset))
        {
            UpdateUIMark(GetChildTransformFromIndex(2, 1));
            return true;
        }
        // right column
        if ((GameplayController.tiles[0, 2] == TileState.Unset && GameplayController.tiles[1, 2] == checkState && GameplayController.tiles[2, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(0, 2));
            return true;
        }
        if ((GameplayController.tiles[0, 2] == checkState && GameplayController.tiles[1, 2] == TileState.Unset && GameplayController.tiles[2, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(1, 2));
            return true;
        }
        if ((GameplayController.tiles[0, 2] == checkState && GameplayController.tiles[1, 2] == checkState && GameplayController.tiles[2, 2] == TileState.Unset))
        {
            UpdateUIMark(GetChildTransformFromIndex(2, 2));
            return true;
        }

        //diagonals
        //forward diagonal
        if ((GameplayController.tiles[0, 0] == TileState.Unset && GameplayController.tiles[1, 1] == checkState && GameplayController.tiles[2, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(0, 0));
            return true;
        }
        if ((GameplayController.tiles[0, 0] == checkState && GameplayController.tiles[1, 1] == TileState.Unset && GameplayController.tiles[2, 2] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(1, 1));
            return true;
        }
        if ((GameplayController.tiles[0, 0] == checkState && GameplayController.tiles[1, 1] == checkState && GameplayController.tiles[2, 2] == TileState.Unset))
        {
            UpdateUIMark(GetChildTransformFromIndex(2, 2));
            return true;
        }

        //backward diagonal
        if ((GameplayController.tiles[0, 2] == TileState.Unset && GameplayController.tiles[1, 1] == checkState && GameplayController.tiles[2, 0] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(0, 2));
            return true;
        }
        if ((GameplayController.tiles[0, 2] == checkState && GameplayController.tiles[1, 1] == TileState.Unset && GameplayController.tiles[2, 0] == checkState))
        {
            UpdateUIMark(GetChildTransformFromIndex(1, 1));
            return true;
        }
        if ((GameplayController.tiles[0, 2] == checkState && GameplayController.tiles[1, 1] == checkState && GameplayController.tiles[2, 0] == TileState.Unset))
        {
            UpdateUIMark(GetChildTransformFromIndex(2, 0));
            return true;
        }
        return false;
    }

    public void ExecuteGameplay(AIDifficulty aiDifficulty)
    {
        switch (aiDifficulty)
        {
            case AIDifficulty.Easy:
                EasyMove();
                break;
            case AIDifficulty.Medium:
                if (!CheckForTwoThird(TileState.MarkedO))
                    if (!CheckForTwoThird(TileState.MarkedX))
                        EasyMove();
                break;
            case AIDifficulty.Hard:
                if (!CheckForTwoThird(TileState.MarkedO))
                    if (!CheckForTwoThird(TileState.MarkedX))
                        if (!StopMove())
                            EasyMove();
                break;
        }
    }

    void UpdateUIMark(Transform tileTransform)
    {
        TileComponent tileComponent = tileTransform.GetComponent<TileComponent>();
        tileComponent.isMarked = true;
        tileComponent.tileState = TileState.MarkedO;
        tileTransform.GetChild(0).gameObject.SetActive(true);
        GameplayController.turns++;
    }

    Transform GetChildTransformFromIndex(int rowIndex, int columnIndex)
    {
        switch (rowIndex)
        {
            case 0:
                switch (columnIndex)
                {
                    case 0:
                        return parentTransform.GetChild(0);
                    case 1:
                        return parentTransform.GetChild(1);
                    case 2:
                        return parentTransform.GetChild(2);
                }
                break;
            case 1:
                switch (columnIndex)
                {
                    case 0:
                        return parentTransform.GetChild(3);
                    case 1:
                        return parentTransform.GetChild(4);
                    case 2:
                        return parentTransform.GetChild(5);
                }
                break;
            case 2:
                switch (columnIndex)
                {
                    case 0:
                        return parentTransform.GetChild(6);
                    case 1:
                        return parentTransform.GetChild(7);
                    case 2:
                        return parentTransform.GetChild(8);
                }
                break;
        }
        return null;
    }

    void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int index = Random.Range(0, i);
            int temp = array[index];
            array[index] = array[i];
            array[i] = temp;
        }
    }

}
