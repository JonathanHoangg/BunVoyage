using UnityEngine;
using UnityEngine.UI;

// Tracks how many lives the player has left and shows them as stars in the corner
public class LivesManager : MonoBehaviour
{
    // The star images and the colors for a full star and a lost star
    [SerializeField] Image[] stars;
    [SerializeField] Color fullColor = Color.white;
    [SerializeField] Color lostColor = new Color(0.25f, 0.25f, 0.25f, 0.6f);

    int lives;

    public int Lives => lives;
    public bool IsOutOfLives => lives <= 0;

    // Lets other scripts know when the last life is lost, which shows the results screen
    public event System.Action OutOfLives;

    // Starts with one life for each star
    void Start()
    {
        lives = stars.Length;
        UpdateStars();
    }

    // Removes a life and ends the run when none are left
    public void LoseLife()
    {
        if (lives <= 0) return;

        lives--;
        UpdateStars();

        if (lives <= 0)
        {
            Time.timeScale = 0f;
            OutOfLives?.Invoke();
        }
    }

    // Darkens a star for each life lost, starting from the right
    void UpdateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = i < lives ? fullColor : lostColor;
        }
    }
}