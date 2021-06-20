using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;


namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Scene")]
    public class OnScene : MonoBehaviour
    {
        [SerializeField] private SceneReference _scene;

        public void OnSceneChange()
        {
            _scene.LoadScene();
        }
    }
}
