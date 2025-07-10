/*
*  NAnimate - Copyright (C) 2025 by Netcat.
*  A Usefull collection of animation utils for udon.
*  
*  Math equations from: https://easings.net/
*  Licensed under the MIT license.
*  
*/

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public enum EaseType
{
    Linear,
    EaseInQuad,
    EaseOutQuad,
    EaseInOutQuad,
    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic,
    EaseInQuart,
    EaseOutQuart,
    EaseInOutQuart,
    EaseInQuint,
    EaseOutQuint,
    EaseInOutQuint,
    EaseInSine,
    EaseOutSine,
    EaseInOutSine,
    EaseInExpo,
    EaseOutExpo,
    EaseInOutExpo,
    EaseInCirc,
    EaseOutCirc,
    EaseInOutCirc,
    EaseInBack,
    EaseOutBack,
    EaseInOutBack,
    BounceOut,
    ElasticOut
}

public class NAnimate : UdonSharpBehaviour
{
    [TextArea, SerializeField]
    private string _ =
@"Hey!
This component does nothing until it is referenced by another script.";


    // Set max animations per type to avoid memory overhead
    private const int MaxAnims = 16;

    private Transform[] posTargets = new Transform[MaxAnims];
    private Vector3[] posStarts = new Vector3[MaxAnims];
    private Vector3[] posEnds = new Vector3[MaxAnims];
    private float[] posDurations = new float[MaxAnims];
    private float[] posElapsed = new float[MaxAnims];
    private EaseType[] posEases = new EaseType[MaxAnims];
    private bool[] posActive = new bool[MaxAnims];

    private Transform[] rotTargets = new Transform[MaxAnims];
    private Quaternion[] rotStarts = new Quaternion[MaxAnims];
    private Quaternion[] rotEnds = new Quaternion[MaxAnims];
    private float[] rotDurations = new float[MaxAnims];
    private float[] rotElapsed = new float[MaxAnims];
    private EaseType[] rotEases = new EaseType[MaxAnims];
    private bool[] rotActive = new bool[MaxAnims];

    private Transform[] scaleTargets = new Transform[MaxAnims];
    private Vector3[] scaleStarts = new Vector3[MaxAnims];
    private Vector3[] scaleEnds = new Vector3[MaxAnims];
    private float[] scaleDurations = new float[MaxAnims];
    private float[] scaleElapsed = new float[MaxAnims];
    private EaseType[] scaleEases = new EaseType[MaxAnims];
    private bool[] scaleActive = new bool[MaxAnims];

    void Update()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < MaxAnims; i++)
        {
            if (posActive[i])
            {
                posElapsed[i] += dt;
                float t = Mathf.Clamp01(posElapsed[i] / posDurations[i]);
                posTargets[i].localPosition = Vector3.LerpUnclamped(posStarts[i], posEnds[i], Ease(t, posEases[i]));
                if (posElapsed[i] >= posDurations[i]) posActive[i] = false;
            }

            if (rotActive[i])
            {
                rotElapsed[i] += dt;
                float t = Mathf.Clamp01(rotElapsed[i] / rotDurations[i]);
                rotTargets[i].localRotation = Quaternion.SlerpUnclamped(rotStarts[i], rotEnds[i], Ease(t, rotEases[i]));
                if (rotElapsed[i] >= rotDurations[i]) rotActive[i] = false;
            }

            if (scaleActive[i])
            {
                scaleElapsed[i] += dt;
                float t = Mathf.Clamp01(scaleElapsed[i] / scaleDurations[i]);
                scaleTargets[i].localScale = Vector3.LerpUnclamped(scaleStarts[i], scaleEnds[i], Ease(t, scaleEases[i]));
                if (scaleElapsed[i] >= scaleDurations[i]) scaleActive[i] = false;
            }
        }
    }

    public void AnimatePosition(Transform target, Vector3 end, float duration, EaseType ease)
    {
        int i = GetFreeIndex(posActive);
        if (i == -1) return;

        posTargets[i] = target;
        posStarts[i] = target.localPosition;
        posEnds[i] = end;
        posDurations[i] = Mathf.Max(duration, 0.001f);
        posElapsed[i] = 0f;
        posEases[i] = ease;
        posActive[i] = true;
    }

    public void AnimateRotation(Transform target, Quaternion end, float duration, EaseType ease)
    {
        int i = GetFreeIndex(rotActive);
        if (i == -1) return;

        rotTargets[i] = target;
        rotStarts[i] = target.localRotation;
        rotEnds[i] = end;
        rotDurations[i] = Mathf.Max(duration, 0.001f);
        rotElapsed[i] = 0f;
        rotEases[i] = ease;
        rotActive[i] = true;
    }

    public void AnimateScale(Transform target, Vector3 end, float duration, EaseType ease)
    {
        int i = GetFreeIndex(scaleActive);
        if (i == -1) return;

        scaleTargets[i] = target;
        scaleStarts[i] = target.localScale;
        scaleEnds[i] = end;
        scaleDurations[i] = Mathf.Max(duration, 0.001f);
        scaleElapsed[i] = 0f;
        scaleEases[i] = ease;
        scaleActive[i] = true;
    }

    private int GetFreeIndex(bool[] activeArray)
    {
        for (int i = 0; i < activeArray.Length; i++)
        {
            if (!activeArray[i]) return i;
        }
        return -1;
    }


    public void Bounce(Transform target, float duration, Vector3 bounceAmount)
    {
        int i = GetFreeIndex(scaleActive);
        if (i == -1) return;

        // Phase 1: Bounce out
        Vector3 start = target.localScale;
        Vector3 peak = start + bounceAmount;

        scaleTargets[i] = target;
        scaleStarts[i] = start;
        scaleEnds[i] = peak;
        scaleDurations[i] = duration / 2f;
        scaleElapsed[i] = 0f;
        scaleEases[i] = EaseType.BounceOut;
        scaleActive[i] = true;

        // Schedule return to original
        SendCustomEventDelayedSeconds(nameof(BounceReturn0), duration / 2f); // You can add BounceReturn1, etc. for more targets
    }

    // Helper to return to original scale
    public void BounceReturn0()
    {
        int i = GetFreeIndex(scaleActive);
        if (i == -1 || scaleTargets[0] == null) return;

        Transform target = scaleTargets[0];
        Vector3 end = target.localScale / 1.2f; // Return to near original scale

        scaleTargets[i] = target;
        scaleStarts[i] = target.localScale;
        scaleEnds[i] = end;
        scaleDurations[i] = 0.2f;
        scaleElapsed[i] = 0f;
        scaleEases[i] = EaseType.EaseOutBack;
        scaleActive[i] = true;
    }


    private float Ease(float t, EaseType ease)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        switch (ease)
        {
            case EaseType.EaseInQuad: return t * t;
            case EaseType.EaseOutQuad: return 1 - (1 - t) * (1 - t);
            case EaseType.EaseInOutQuad: return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
            case EaseType.EaseInCubic: return t * t * t;
            case EaseType.EaseOutCubic: return 1 - Mathf.Pow(1 - t, 3);
            case EaseType.EaseInOutCubic: return t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
            case EaseType.EaseInQuart: return t * t * t * t;
            case EaseType.EaseOutQuart: return 1 - Mathf.Pow(1 - t, 4);
            case EaseType.EaseInOutQuart: return t < 0.5f ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;
            case EaseType.EaseInQuint: return t * t * t * t * t;
            case EaseType.EaseOutQuint: return 1 - Mathf.Pow(1 - t, 5);
            case EaseType.EaseInOutQuint: return t < 0.5f ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;
            case EaseType.EaseInSine: return 1 - Mathf.Cos(t * Mathf.PI / 2);
            case EaseType.EaseOutSine: return Mathf.Sin(t * Mathf.PI / 2);
            case EaseType.EaseInOutSine: return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
            case EaseType.EaseInExpo: return t == 0f ? 0f : Mathf.Pow(2, 10 * t - 10);
            case EaseType.EaseOutExpo: return t == 1f ? 1f : 1 - Mathf.Pow(2, -10 * t);
            case EaseType.EaseInOutExpo:
                if (t == 0f) return 0f;
                if (t == 1f) return 1f;
                return t < 0.5f
                    ? Mathf.Pow(2, 20 * t - 10) / 2
                    : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
            case EaseType.EaseInCirc: return 1 - Mathf.Sqrt(1 - t * t);
            case EaseType.EaseOutCirc: return Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
            case EaseType.EaseInOutCirc:
                return t < 0.5f
                    ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2
                    : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;
            case EaseType.EaseInBack:
                return c3 * t * t * t - c1 * t * t;
            case EaseType.EaseOutBack:
                return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
            case EaseType.EaseInOutBack:
                return t < 0.5f
                    ? (Mathf.Pow(2 * t, 2) * ((c1 + 1) * 2 * t - c1)) / 2
                    : (Mathf.Pow(2 * t - 2, 2) * ((c1 + 1) * (t * 2 - 2) + c1) + 2) / 2;
            case EaseType.BounceOut:
                if (t < 1 / 2.75f)
                    return 7.5625f * t * t;
                else if (t < 2 / 2.75f)
                {
                    t -= 1.5f / 2.75f;
                    return 7.5625f * t * t + 0.75f;
                }
                else if (t < 2.5f / 2.75f)
                {
                    t -= 2.25f / 2.75f;
                    return 7.5625f * t * t + 0.9375f;
                }
                else
                {
                    t -= 2.625f / 2.75f;
                    return 7.5625f * t * t + 0.984375f;
                }
            case EaseType.ElasticOut:
                if (t == 0f) return 0f;
                if (t == 1f) return 1f;
                float p = 0.3f;
                return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - p / 4) * (2 * Mathf.PI) / p) + 1;
            case EaseType.Linear:
            default:
                return t;
        }
    }
}
