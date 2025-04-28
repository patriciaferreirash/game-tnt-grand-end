using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerTCC : MonoBehaviour
{
    [SerializeField]
    private GameObject firstSelectedButton;
    [SerializeField]
    private GameObject creditsPanel;
    [SerializeField]
    private GameObject backButton;
    [SerializeField]
    private GameObject buttonMenu;

    private InputPlayer inputActions;

    private void Awake()
    {
        inputActions = new InputPlayer();
    }

    private void OnEnable()
    {
        inputActions.MenuStart.Enable();
        inputActions.MenuStart.Submit.performed += OnSubmit;

        if (EventSystem.current != null && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    private void OnDisable()
    {
        inputActions.MenuStart.Submit.performed -= OnSubmit;
        inputActions.MenuStart.Disable();
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current != null)
        {
            var button = current.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
        StartCoroutine(SelectedBackButton());
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
        StartCoroutine(SelectButtonMenu());
    }

    IEnumerator SelectedBackButton()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    IEnumerator SelectButtonMenu()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(buttonMenu);
    }

}
