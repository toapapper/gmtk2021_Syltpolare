using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float _nonPossessedDuration = 1.0f;
    [SerializeField] private UnityEvent _nonPossessedEvent;

    private Coroutine _waitIfNonPossessed;

    public void OnRemovePossessable(IReadOnlyList<Possess> possesses)
    {
        if (possesses.Count > 0)
            return;

        if (_waitIfNonPossessed == null)
            _waitIfNonPossessed = StartCoroutine(WaitIfNonPossessed());
    }

    /// <summary>
    /// Must both contain a possess and a onDeath component to pass.
    /// </summary>
    public void DestroyAllRobots()
    {
        Possess[] possesses = FindObjectsOfType<Possess>(true);

        for (int i = possesses.Length - 1; i >= 0; i--)
            if (possesses[i].TryGetComponent(out OnDeath onDeath))
                onDeath.RespawnableDeathInvoke();
    }

    public void OnAddPossessable(IReadOnlyList<Possess> possesses)
    {
        if (_waitIfNonPossessed != null)
        {
            StopCoroutine(_waitIfNonPossessed);
            _waitIfNonPossessed = null;
        }
    }

    private void OnEnable()
    {
        Possess.RemoveEvent += OnRemovePossessable;
        Possess.AddEvent += OnAddPossessable;

    }

    private void OnDisable()
    {
        Possess.RemoveEvent -= OnRemovePossessable;
        Possess.AddEvent -= OnAddPossessable;
    }

    private IEnumerator WaitIfNonPossessed()
    {
        yield return new WaitForSeconds(_nonPossessedDuration);
        _nonPossessedEvent.Invoke();
    }
}
