using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Runs the title screen: shows the best profit so far and starts the game from the Start button or the Enter key
public class TitleScreen : MonoBehaviour
{
    // The scene with the game in it and the text that shows the best profit
    [SerializeField] string gameSceneName = "SampleScene";
    [SerializeField] TextMeshProUGUI topProfitText;

    // Makes sure the game is unfrozen and shows the saved top profit
    void Start()
    {
        Time.timeScale = 1f;
        int topProfit = PlayerPrefs.GetInt("TopProfit", 0);
        if (topProfitText != null) topProfitText.text = "Top Profit Score: $" + topProfit;
    }

    // Lets the player press Enter to start
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    // Loads the game scene
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}