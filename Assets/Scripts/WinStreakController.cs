using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinStreakController : MonoBehaviour
{
    public static WinStreakController Instance;
    int rotations = 6;
    float rotationSpeed = 5f;
    float countdown;

    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void ShowWinStreak()
    {
        int[] indeces = GameplayController.Instance.GetTransformIndeces(GameplayController.Instance.winStreak);
        float deg = 0f;
        deg += rotationSpeed;
        Vector3[] localScales = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            Transform tempTransform = transform.GetChild(indeces[i]);
            localScales[i] = tempTransform.localScale;
            tempTransform.localScale *= .5f;
            LeanTween.rotateAround(tempTransform.gameObject, Vector3.up, 359, 1).setLoopCount(rotations);
            LeanTween.scale(tempTransform.gameObject, localScales[i] * 1.3f, 1).setLoopPingPong(rotations / 2);//.setOnComplete(() => { tempTransform.localScale = localScales[i]; });
            if (i == 2) StartCoroutine(NormalizeScale(tempTransform.gameObject, indeces, localScales));
        }
        UIManager.Instance.retryButton.interactable = false;
    }
    IEnumerator NormalizeScale(GameObject obj, int[] indeces, Vector3[] localScales)
    {
        yield return new WaitWhile(() => LeanTween.isTweening(obj));
        for (int x = 0; x < 3; x++) transform.GetChild(indeces[x]).localScale = localScales[x];
        UIManager.Instance.retryButton.interactable = true;
    }
}
