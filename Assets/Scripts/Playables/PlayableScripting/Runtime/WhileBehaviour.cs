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

        public ExposedReference<ConditionBehaviour> ConditionSource;
    }
}
