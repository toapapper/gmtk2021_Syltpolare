using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class GotoMarker : PlayableScriptingMarker
    {
        public bool Invert = true;
        public bool EmitOnce = true;

        private bool _oldCondition;

        public override void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData)
        {
            ConditionBehaviour conditionBehaviour = ConditionSource.Resolve(playable.GetGraph().GetResolver());
            bool condition = conditionBehaviour.Condition;

            if (conditionBehaviour == null)
                return;

            if ((Invert ? condition : !condition) && (EmitOnce ? _oldCondition != condition : true))
                playableDirector.time = time;

            _oldCondition = condition;
        }

        public override void OnInitialize(TrackAsset aPent)
        {
            if (!(aPent is PlayableScriptingTrack))
            {
                Debug.LogError(aPent.name + " does not support " + nameof(GotoMarker));
                return;
            }

            (aPent as PlayableScriptingTrack).CreateMarkers();
        }

    }
}
