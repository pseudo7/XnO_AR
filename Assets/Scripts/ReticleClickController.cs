using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class ReticleClickController : MonoBehaviour
{
    public Image reticleImg;
    public float gazeTime = 2;
    public Camera cam;

    float timer;
    static bool toggle;

    void Start()
    {
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
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Tile"))
        {
            if (timer > gazeTime)
            {
                Transform hitTransform = hit.transform;
                TileComponent tileComponent = hitTransform.GetComponent<TileComponent>();
                if (!tileComponent.isMarked)
                {
                    hitTransform.GetComponent<TileComponent>().isMarked = true;
                    hitTransform.GetChild(toggle ? 0 : 1).gameObject.SetActive(true);
                    hitTransform.GetChild(toggle ? 1 : 0).gameObject.SetActive(false);
                    tileComponent.tileState = toggle ? TileState.MarkedX : TileState.MarkedO;
                    GameplayController.Instance.UpdateTile(tileComponent);
                    toggle = !toggle;
                }
                OnPointerExit();
                if (GameplayController.Instance.CheckForWin())
                {
                    UIManager.Instance.winText.text = !GameplayController.Instance.didXWin ? "X Won!" : "O Won!";
                    WinStreakController.Instance.ShowWinStreak();
                    UIManager.Instance.retryButton.gameObject.SetActive(true);
                    UIManager.Instance.captureButton.gameObject.SetActive(false);
                }
                else if (GameplayController.isGameDraw)
                {
                    GameplayController.isGameDraw = false;
                    UIManager.Instance.winText.text = "Draw";
                }
            }
            timer += Time.unscaledDeltaTime;
            reticleImg.fillAmount = timer / gazeTime;
        }
        else OnPointerExit();
    }

    public void OnPointerExit()
    {
        reticleImg.fillAmount = 0;
        timer = 0;
    }
}
