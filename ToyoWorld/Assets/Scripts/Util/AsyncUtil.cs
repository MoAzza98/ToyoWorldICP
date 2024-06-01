using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncUtil
{
    public static IEnumerator RunAfterTime(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
