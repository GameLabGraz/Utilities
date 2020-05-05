using System.Collections;
using UnityEngine;

namespace GEAR.Utilities.Coroutine
{
    public class CoroutineWithData
    {
        public UnityEngine.Coroutine Coroutine { get; }
        public object Result { get; private set; }

        private readonly IEnumerator _target;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            _target = target;
            Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (_target.MoveNext())
            {
                Result = _target.Current;
                yield return Result;
            }
        }
    }
}
