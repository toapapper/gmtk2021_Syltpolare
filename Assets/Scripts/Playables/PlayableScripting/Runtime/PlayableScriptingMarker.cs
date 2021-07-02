using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public abstract class PlayableScriptingMarker : Marker
    {
        public abstract new string name { get; }

        public ExposedReference<ConditionBehaviour> ConditionSource;

        public abstract void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData);

        public sealed override void OnInitialize(TrackAsset aPent)
        {
            if (!(aPent is PlayableScriptingTrack))
            {
                Debug.LogError(aPent.name + " does not support " + name);
                return;
            }

            (aPent as PlayableScriptingTrack).CreateMarkers();
        }
    }
}
