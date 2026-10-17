
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PreferencesManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Dropdown enemySpeedDropdown;
    public Slider spawnRateSlider;
    public TMP_Text spawnRateText;

    public void SavePreferences()
    {
        string playerName = playerNameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Player";
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("EnemySpeed", enemySpeedDropdown.value + 1);
        PlayerPrefs.SetFloat("SpawnRate", spawnRateSlider.value);

        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }

    public void UpdateSpawnRateText()
    {
        if (spawnRateText != null && spawnRateSlider != null)
        {
            spawnRateText.text = "Enemies per minute: "
                + Mathf.RoundToInt(spawnRateSlider.value);
        }
    }
}
