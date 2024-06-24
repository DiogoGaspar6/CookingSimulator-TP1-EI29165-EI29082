using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    [SerializeField] private GameObject mainPlanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private string GameScene;

    public void StartGame()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void ShowControls()
    {
        mainPlanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        mainPlanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Saiu");
        Application.Quit();
    }
}
