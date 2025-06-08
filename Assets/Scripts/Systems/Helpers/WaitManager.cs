using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitManager
{
    private static readonly Dictionary<float, Stack<WaitForSeconds>> _pool = new();
    private static WaitForSeconds GetWait(float time)
    {
        if (_pool.TryGetValue(time, out var stack) && stack.Count > 0)
        {
            return stack.Pop(); // Reuse if available
        }
        return new WaitForSeconds(time); // Otherwise, create a new one
    }
    private static void ReturnWait(float time, WaitForSeconds wait)
    {
        if (!_pool.ContainsKey(time))
        {
            _pool[time] = new Stack<WaitForSeconds>();
        }
        _pool[time].Push(wait); // Store back for reuse
    }
    public static IEnumerator Wait(float time)
    {
        var wait = GetWait(time);
        yield return wait;
        ReturnWait(time, wait); // Auto return after wait is done
    }

    private static WaitForFixedUpdate _waitForFixed;
    public static WaitForFixedUpdate WaitForFixed()
    {
        if (_waitForFixed == null)
            _waitForFixed = new WaitForFixedUpdate();

        return _waitForFixed;
    }
}