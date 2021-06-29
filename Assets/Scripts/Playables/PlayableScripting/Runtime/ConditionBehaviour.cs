using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celezt.Timeline
{
    public abstract class ConditionBehaviour : MonoBehaviour
    {
        public abstract bool Condition { get; set; }
    }
}
