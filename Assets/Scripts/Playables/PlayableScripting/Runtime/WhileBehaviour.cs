using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class WhileBehaviour : PlayableBehaviour
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public bool HasBeenEligible { get; set; } = false;

        public ExposedReference<ConditionBehaviour> ConditionSource;
        public bool Invert = true;
        [Tooltip("Only need the condition to be eligible once whiles inside the clip.")]
        public bool OnlyEligibleOnce = false;
    }
}
