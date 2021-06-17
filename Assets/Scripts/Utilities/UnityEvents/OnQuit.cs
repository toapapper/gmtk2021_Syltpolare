using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celezt.UnityEvent
{
    [AddComponentMenu("Celezt/Unity Events/On Quit")]
    public class OnQuit : MonoBehaviour
    {
        public void OnQuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
