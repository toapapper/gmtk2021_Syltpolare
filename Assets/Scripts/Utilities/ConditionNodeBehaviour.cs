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
        [SerializeField] private bool _onlyActiveOnProcess;

        private ConditionBehaviour _conditionBehaviour;

        public override void CreateNode(IReadOnlyList<NodeBehaviour> children, NodeBehaviour parent)
        {
            _conditionBehaviour = GetComponent<ConditionBehaviour>();

            if (_onlyActiveOnProcess)
                _conditionBehaviour.enabled = false;
        }

        public override INodeAsset ProcessNode(IReadOnlyList<NodeBehaviour> children, NodeBehaviour parent)
        {
            if (_onlyActiveOnProcess)
                _conditionBehaviour.enabled = true;

            bool condition = _conditionBehaviour.Condition;

            if (_invert ? !condition : condition)
                gameObject.SetActive(false);

            return new LeafAsset { Status = NodeStatus.Success };
        }
    }
}
