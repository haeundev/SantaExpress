using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class YieldCache
{
    private class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return Mathf.Abs(x - y) < 0.001f;
        }

        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> _timeInterval =
        new Dictionary<float, WaitForSeconds>(new FloatComparer());

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_timeInterval.TryGetValue(seconds, out var wfs))
            _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }

    private static readonly Dictionary<float, WaitForSecondsRealtime> RealTimeInterval =
        new Dictionary<float, WaitForSecondsRealtime>(new FloatComparer());

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
    {
        if (!RealTimeInterval.TryGetValue(seconds, out var wfsRt))
            RealTimeInterval.Add(seconds, wfsRt = new WaitForSecondsRealtime(seconds));
        return wfsRt;
    }

    private static readonly Dictionary<Func<bool>, WaitUntil> WaitUntilDic = new Dictionary<Func<bool>, WaitUntil>();

    public static WaitUntil WaitUntil(Func<bool> method)
    {
        if (!WaitUntilDic.TryGetValue(method, out var waitUntil))
            WaitUntilDic.Add(method, waitUntil = new WaitUntil(method));
        return waitUntil;
    }

    public static IEnumerator WaitForUnscaledSecond(float seconds)
    {
        var currentTime = 0f;
        while (currentTime < seconds)
        {
            currentTime += Time.unscaledDeltaTime;
            yield return WaitForEndOfFrame;
        }
    }
}