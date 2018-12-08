using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class ReticleClickControllerAI : MonoBehaviour
{
    public static bool isPlayerTurn = true;
    public Image reticleImg;
    public float gazeTime = 2;
    public Camera cam;

    float timer;

    void Start()
    {
        isPlayerTurn = true;
        GameplayController.isGameOver = false;
        reticleImg.fillAmount = 0;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AIController.Instance.Reset();
            SceneManager.LoadScene("MainMenu");
        }
        if (GameplayController.isGameOver || GameplayController.isGameDraw)
        {
            OnPointerExit();
            return;
        }
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (isPlayerTurn)
        {
            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Tile"))
            {
                if (timer > gazeTime)
                {
                    Transform hitTransform = hit.transform;
                    TileComponent tileComponent = hitTransform.GetComponent<TileComponent>();
                    if (!tileComponent.isMarked)
                    {
                        hitTransform.GetChild(1).gameObject.SetActive(true);
                        tileComponent.isMarked = true;
                        tileComponent.tileState = TileState.MarkedX;
                        GameplayController.Instance.UpdateTile(tileComponent);
                        isPlayerTurn = false;
                    }
                    if (GameplayController.Instance.CheckForWin())
                    {
                        UIManager.Instance.winText.text = GameplayController.Instance.didXWin ? "X Won!" : "O Won!";
                        WinStreakController.Instance.ShowWinStreak();
                        UIManager.Instance.retryButton.gameObject.SetActive(true);
                        UIManager.Instance.captureButton.gameObject.SetActive(false);
                    }
                    else if (GameplayController.isGameDraw)
                    {
                        GameplayController.isGameDraw = false;
                        UIManager.Instance.winText.text = "Draw";
                        UIManager.Instance.retryButton.gameObject.SetActive(true);
                        UIManager.Instance.captureButton.gameObject.SetActive(false);
                    }
                    OnPointerExit();
                }
                timer += Time.unscaledDeltaTime;
                reticleImg.fillAmount = timer / gazeTime;
            }
            else OnPointerExit();
        }
        else
        {
            if (GameplayController.turns < 8 && !GameplayController.isGameOver)
            {
                AIController.Instance.ExecuteGameplay(GameplayController.difficulty);
                isPlayerTurn = true;
            }
            if (GameplayController.Instance.CheckForWin())
            {
                UIManager.Instance.winText.text = GameplayController.Instance.didXWin ? "X Won!" : "O Won!";
                WinStreakController.Instance.ShowWinStreak();
                UIManager.Instance.retryButton.gameObject.SetActive(true);
                UIManager.Instance.captureButton.gameObject.SetActive(false);
            }
            else if (GameplayController.isGameDraw)
            {
                GameplayController.isGameDraw = false;
                UIManager.Instance.winText.text = "Draw";
                UIManager.Instance.retryButton.gameObject.SetActive(true);
                UIManager.Instance.captureButton.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerExit()
    {
        reticleImg.fillAmount = 0;
        timer = 0;
    }
}
