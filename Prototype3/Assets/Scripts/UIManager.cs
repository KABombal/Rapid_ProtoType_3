using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject victoryScreen;
    public GameObject warningMessage;  // Reference to warning text element
    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        PauseGame();
    }

    public void HideDeathScreen()
    {
        deathScreen.SetActive(false);
        UnpauseGame();
    }

    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
        PauseGame();
    }

    public void HideVictoryScreen()
    {
        victoryScreen.SetActive(false);
        UnpauseGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Hide death screen if needed
        HideDeathScreen();
    }

    public void ReturnToMenu()
    {
        UnpauseGame();
        SceneManager.LoadScene("MenuSceneName"); // Replace with your menu scene name
    }

    public void ExitGame()
    {
        UnpauseGame();
        Application.Quit();
    }

    public void ShowWarningMessage()
    {
        warningMessage.SetActive(true);
    }

    public void HideWarningMessage()
    {
        warningMessage.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
