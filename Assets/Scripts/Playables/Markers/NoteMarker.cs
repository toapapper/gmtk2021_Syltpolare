using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class NoteMarker : Marker
    {
        [SerializeField, TextArea] private string _note;
    }
}
