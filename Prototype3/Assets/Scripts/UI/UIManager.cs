using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject victoryScreen;
    public GameObject warningMessage;  // Reference to warning text element

    public UnityEvent onShowDeathScreen;
    public UnityEvent onHideDeathScreen;
    public UnityEvent onShowVictoryScreen;
    public UnityEvent onHideVictoryScreen;

    public void ShowDeathScreen()
    {
        onShowDeathScreen?.Invoke();  // Trigger the event
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseGame();
    }

    public void HideDeathScreen()
    {
        onHideDeathScreen?.Invoke();  // Trigger the event
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
        SceneManager.LoadScene("Main_Menu"); // Replace with your menu scene name
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
