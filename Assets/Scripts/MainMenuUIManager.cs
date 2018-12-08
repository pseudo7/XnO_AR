using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public Text easyText, mediumText, hardText, supportText;
    public Button Play2PButton, PlayAIButton;

    void Awake()
    {
        GameplayController.difficulty = AIDifficulty.Easy;
        Play2PButton.interactable = SystemInfo.supportsGyroscope;
        PlayAIButton.interactable = SystemInfo.supportsGyroscope;
        supportText.text = SystemInfo.supportsGyroscope ? "Device supports AR" : "Device doesn't supports AR";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void Play2P()
    {
        SceneManager.LoadScene("Gameplay2P");
    }

    public void PlayAI()
    {
        SceneManager.LoadScene("GameplayAI");
    }

    public void UpdateDifficultyText(Slider diffSlider)
    {
        switch ((int)diffSlider.value)
        {
            case 0:
                GameplayController.difficulty = AIDifficulty.Easy;
                easyText.fontStyle = FontStyle.Italic;
                mediumText.fontStyle = FontStyle.Normal;
                hardText.fontStyle = FontStyle.Normal;
                break;
            case 1:
                GameplayController.difficulty = AIDifficulty.Medium;
                easyText.fontStyle = FontStyle.Normal;
                mediumText.fontStyle = FontStyle.Italic;
                hardText.fontStyle = FontStyle.Normal;
                break;
            case 2:
                GameplayController.difficulty = AIDifficulty.Hard;
                easyText.fontStyle = FontStyle.Normal;
                mediumText.fontStyle = FontStyle.Normal;
                hardText.fontStyle = FontStyle.Italic;
                break;
        }
    }
}
