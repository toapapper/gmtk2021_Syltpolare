using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Celezt.Times;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Debug Log String")]
    public class OnDebug : MonoBehaviour
    {
        public void OnDebugLog(string print) => Debug.Log(print);
        public void OnDebugLog(int print) => Debug.Log(print);
        public void OnDebugLog(float print) => Debug.Log(print);
    }
}