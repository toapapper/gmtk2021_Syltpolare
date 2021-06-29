using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    [CustomStyle("Annotation")]
    [DisplayName("Annotation")]
    public class AnnotationMarker : Marker
    {
        public Color Color = Color.white;
        public bool ShowOverlay = true;
        [TextArea(10, 15)] public string Note;
    }
}
