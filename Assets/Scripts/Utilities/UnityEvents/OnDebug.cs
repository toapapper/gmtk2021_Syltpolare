using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Time;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Debug")]
    public class OnDebug : MonoBehaviour
    {
        public void OnDebugLog(string print) => Debug.Log(print);
        public void OnDebugLog(int print) => Debug.Log(print);
        public void OnDebugLog(float print) => Debug.Log(print);
    }
}
