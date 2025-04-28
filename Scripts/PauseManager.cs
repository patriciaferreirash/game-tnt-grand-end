using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject resumeButton;

    private InputPlayer inputActions;
    private bool isPaused;

    private void Awake()
    {
        inputActions = new InputPlayer();
    }

    private void OnEnable()
    {
        inputActions.MenuPause.Enable();
        inputActions.MenuPause.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        inputActions.MenuPause.Pause.performed -= OnPause;
        inputActions.MenuPause.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        StartCoroutine(FocusButton(resumeButton));
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuPanel.SetActive(false);
    }

    IEnumerator FocusButton(GameObject button)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
