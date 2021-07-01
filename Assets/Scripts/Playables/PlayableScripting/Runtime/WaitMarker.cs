using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{

    public class WaitMarker : PlayableScriptingMarker
    {
        public bool Invert = false;
        [Tooltip("Only need the condition to be eligible once whiles inside the clip.")]
        public bool OnlyEligibleOnce = true;

        private bool _hasBeenEligible;
        private bool _isWaiting;

        public override void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData)
        {
            ConditionBehaviour conditionBehaviour = ConditionSource.Resolve(playable.GetGraph().GetResolver());
            bool condition = conditionBehaviour.Condition;
            double currentTime = playableDirector.time;

            Debug.Log(_hasBeenEligible);
            if (conditionBehaviour == null)
                return;

            if (OnlyEligibleOnce && (Invert ? condition : !condition))
                _hasBeenEligible = true;

            if ((OnlyEligibleOnce ? _hasBeenEligible : true) && (Invert ? condition : !condition) && (currentTime <= time && currentTime + info.deltaTime > time))
            {
                _isWaiting = true;
                playableDirector.time = time;
            }
            else if (_isWaiting)
            {
                _isWaiting = false;
                _hasBeenEligible = false;
            }
        }

        public override void OnInitialize(TrackAsset aPent)
        {
            if (!(aPent is PlayableScriptingTrack))
            {
                Debug.LogError(aPent.name + " does not support " + nameof(WaitMarker));
                return;
            }

            (aPent as PlayableScriptingTrack).CreateMarkers();
        }
    }
}
