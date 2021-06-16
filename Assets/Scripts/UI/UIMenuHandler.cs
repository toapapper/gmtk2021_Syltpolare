using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class UIMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] SceneReference _mainMenu;

    private bool _isMenuActive;

    public void OnSwitchMenu(InputContext context)
    {
        switch (context.State)
        {
            case InputContext.InputState.Performed:
                _isMenuActive = !_isMenuActive;

                if (_isMenuActive)
                {
                    _content.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    _content.SetActive(false);
                    Time.timeScale = 1;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Return back to the game.
    /// </summary>
    public void OnReturn() => OnSwitchMenu(new InputContext(1, InputContext.InputState.Performed));

    /// <summary>
    /// Quit current game session and return to the start screen.
    /// </summary>
    public void OnMainMenu() => _mainMenu.LoadScene();

    /// <summary>
    /// Quit the game (works in editor mode).
    /// </summary>
    public void OnQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Start()
    {
        _content.SetActive(false);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
