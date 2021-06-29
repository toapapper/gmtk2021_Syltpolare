using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class GotoMarker : Marker, INotification
    {
        [SerializeField] private ExposedReference<ConditionBehaviour> ConditionSource;
        [SerializeField] private bool Invert = true;

        public PropertyName id => new PropertyName();
    }
}
