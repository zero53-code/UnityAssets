using System;
using System.Collections;
using UnityEngine;

namespace Zero53.Gas.GameplayAbilityTasks
{
    [Serializable]
    public abstract class CoroutineGameplayAbilityTask : GameplayAbilityTask
    {
        private IEnumerator _enumerator;
        private Coroutine _coroutine;
        
        protected abstract IEnumerator OnUpdateCoroutine();

        protected override void OnStart()
        {
            _enumerator = OnUpdateCoroutine();
            if (_enumerator == null)
            {
                End();
                return;
            }

            _coroutine = abilitySystem.StartCoroutine(CoroutineWrapper());

            if (_coroutine == null) End();
        }

        protected internal sealed override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            if (_coroutine != null)
                abilitySystem.StopCoroutine(_coroutine);

            _coroutine = null;
        }

        private IEnumerator CoroutineWrapper()
        {
            while (_enumerator.MoveNext())
            {
                yield return _enumerator.Current;
            }

            End();
        }
    }
}