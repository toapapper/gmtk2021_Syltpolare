using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class GotoMarker : Marker
    {
        public bool OldCondition { get; set; }

        public ExposedReference<ConditionBehaviour> ConditionSource;
        public bool Invert;
        public bool EmitOnce = true;

        public override void OnInitialize(TrackAsset aPent)
        {
            if (!(aPent is PlayableScriptingTrack))
                Debug.LogError(aPent.name + " does not support " + nameof(GotoMarker));
        }
    }
}
