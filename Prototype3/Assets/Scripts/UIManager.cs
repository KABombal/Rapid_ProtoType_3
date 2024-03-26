using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject victoryScreen;

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

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
