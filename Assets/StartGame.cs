using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;

public class StartGame : MonoBehaviour
{
    [SerializeField] private SceneReference _gameScene;

    public void OnStartAndLoad()
    {
        _gameScene.LoadScene();
    }

    public void OnQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
