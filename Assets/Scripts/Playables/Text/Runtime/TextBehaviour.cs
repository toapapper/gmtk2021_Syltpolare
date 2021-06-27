using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class TextBehaviour : PlayableBehaviour
    {
        public string Text;
        public Color FontColor = Color.white;
    }
}
