using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Celezt.Timeline;

namespace Celezt.BehaviourTree.GameObject
{
    [AddComponentMenu("Celezt/BehaviourTree/GameObject/Leaves/Condition Node")]
    [RequireComponent(typeof(ConditionBehaviour))]
    public class ConditionNodeBehaviour : NodeBehaviour
    {
        [SerializeField] private bool _invert;

        private ConditionBehaviour _conditionBehaviour;

        public override void CreateNode(IReadOnlyList<NodeBehaviour> children, NodeBehaviour parent)
        {
            _conditionBehaviour = GetComponent<ConditionBehaviour>();
        }

        public override INodeAsset ProcessNode(IReadOnlyList<NodeBehaviour> children, NodeBehaviour parent)
        {
            NodeStatus status = (_invert ? !_conditionBehaviour.Condition : _conditionBehaviour.Condition) ? NodeStatus.Success : NodeStatus.Failure;

            return new LeafAsset { Status = status };
        }
    }
}
