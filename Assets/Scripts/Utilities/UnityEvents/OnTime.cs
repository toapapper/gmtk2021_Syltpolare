using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celezt.Event
{
    [AddComponentMenu("Celezt/Events/On Time")]
    public class OnTime : MonoBehaviour
    {
        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }
    }
}