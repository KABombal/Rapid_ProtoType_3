using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject victoryScreen;
    public GameObject warningMessage;  // Reference to warning text element
    public GameObject pauseScreen;

    public UnityEvent onShowDeathScreen;
    public UnityEvent onHideDeathScreen;
    public UnityEvent onShowVictoryScreen;
    public UnityEvent onHideVictoryScreen;

    public SpiderDriver aaaaaaaah;
    public Image[] liveIcons;
    public Image[] deathIcons;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseScreen(!pauseScreen.activeInHierarchy);
    }
    public void ShowDeathScreen()
    {
        HideWarningMessage();
        onShowDeathScreen?.Invoke();  // Trigger the event
        aaaaaaaah.enabled = false;
        PauseGame();
    }

    public void HideDeathScreen()
    {
        onHideDeathScreen?.Invoke();  // Trigger the event
        aaaaaaaah.enabled = true;
        UnpauseGame();
    }

    public void ShowVictoryScreen()
    {
        HideWarningMessage();
        victoryScreen.SetActive(true);
        aaaaaaaah.enabled = false;
        PauseGame();
    }

    public void HideVictoryScreen()
    {
        victoryScreen.SetActive(false);
        aaaaaaaah.enabled = true;
        UnpauseGame();
    }

    public void RestartGame()
    {
        UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Hide death screen if needed
        HideDeathScreen();
    }

    public void RestartCheckpoint()
    {
        GameManager_Scr.Instance.OnPlayerDeath();

        PauseScreen(false);
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
    public void PauseScreen(bool paused)
    {
        Time.timeScale = paused ? 0 : 1;

        pauseScreen.gameObject.SetActive(paused);

        aaaaaaaah.enabled = !paused;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public void SetLives(int lives)
    {
        for (int i = 0; i < liveIcons.Length; i++)
        {
            bool live = i < lives;

            liveIcons[i].enabled = live;
            deathIcons[i].enabled = !live;
        }
    }
}
