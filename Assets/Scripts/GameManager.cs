
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text pauseText;
    public TMP_Text playerNameText;

    private int score = 0;
    private float survivalTime = 0f;
    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;

        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

        if (playerNameText != null)
            playerNameText.text = "Player: " + playerName;

        if (pauseText != null)
            pauseText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (!isPaused)
            survivalTime += Time.deltaTime;

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (timerText != null)
            timerText.text = "Time: "
                + Mathf.FloorToInt(survivalTime) + " seconds";
    }

    public void AddScore(int points)
    {
        score += points;
    }

    public void SaveGameResults()
    {
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetFloat("SurvivalTime", survivalTime);
        PlayerPrefs.Save();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(isPaused);

            if (isPaused)
                pauseText.text = "PAUSED\nPress ESC to Resume";
        }
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
