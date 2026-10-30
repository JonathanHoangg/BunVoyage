using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Runs the title screen: shows the best profit so far, opens the How to Play popup, and starts the game
public class TitleScreen : MonoBehaviour
{
    // The scene with the game in it, the text that shows the best profit, and the How to Play popup
    [SerializeField] string gameSceneName = "SampleScene";
    [SerializeField] TextMeshProUGUI topProfitText;
    [SerializeField] GameObject howToPlayPanel;

    // Makes sure the game is unfrozen, shows the saved top profit, and hides the popup
    void Start()
    {
        Time.timeScale = 1f;
        int topProfit = PlayerPrefs.GetInt("TopProfit", 0);
        if (topProfitText != null) topProfitText.text = "Top Profit Score: $" + topProfit;
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
    }

    // Enter starts the game, unless the popup is open, in which case Enter or Esc closes it
    void Update()
    {
        if (Keyboard.current == null) return;

        if (howToPlayPanel != null && howToPlayPanel.activeSelf)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                HideHowToPlay();
            }
            return;
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    // Loads the game scene
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Opens the How to Play popup
    public void ShowHowToPlay()
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(true);
    }

    // Closes the How to Play popup
    public void HideHowToPlay()
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
    }
}