using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class GotoMarker : Marker
    {
        public ExposedReference<ConditionBehaviour> ConditionSource;
        public bool Invert;
        public bool EmitOnce = true;
    }
}
