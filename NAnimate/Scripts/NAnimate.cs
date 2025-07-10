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
using UnityEngine.UIElements;
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
    [TextArea,SerializeField]private string _ = 
@"Hey!
This component does nothing until it is referenced by another script.";

    private Transform posTargetTransform;
    private Vector3 posStart;
    private Vector3 posEnd;
    private float posDuration;
    private float posElapsed;
    private bool posActive;
    private EaseType posEase;

    private Transform rotTargetTransform;
    private Quaternion rotStart;
    private Quaternion rotEnd;
    private float rotDuration;
    private float rotElapsed;
    private bool rotActive;
    private EaseType rotEase;

    private Transform scaleTargetTransform;
    private Vector3 scaleStart;
    private Vector3 scaleEnd;
    private float scaleDuration;
    private float scaleElapsed;
    private bool scaleActive;
    private EaseType scaleEase;

    private bool bounceReturning;

    void Update()
    {
        float dt = Time.deltaTime;

        if (posActive)
        {
            posElapsed += dt;
            float t = Mathf.Clamp01(posElapsed / posDuration);
            float easedT = Ease(t, posEase);
            posTargetTransform.localPosition = Vector3.LerpUnclamped(posStart, posEnd, easedT);

            if (posElapsed >= posDuration)
                posActive = false;
        }

        if (rotActive)
        {
            rotElapsed += dt;
            float t = Mathf.Clamp01(rotElapsed / rotDuration);
            float easedT = Ease(t, rotEase);
            rotTargetTransform.localRotation = Quaternion.SlerpUnclamped(rotStart, rotEnd, easedT);

            if (rotElapsed >= rotDuration)
                rotActive = false;
        }

        if (scaleActive)
        {
            scaleElapsed += dt;
            float t = Mathf.Clamp01(scaleElapsed / scaleDuration);
            float easedT = Ease(t, scaleEase);
            scaleTargetTransform.localScale = Vector3.LerpUnclamped(scaleStart, scaleEnd, easedT);

            if (scaleElapsed >= scaleDuration)
            {
                if (scaleEase == EaseType.BounceOut && !bounceReturning)
                {
                    Vector3 temp = scaleStart;
                    scaleStart = scaleEnd;
                    scaleEnd = temp;
                    scaleElapsed = 0f;
                    bounceReturning = true;
                }
                else
                {
                    scaleActive = false;
                    bounceReturning = false;
                }
            }
        }
    }

    public void AnimatePosition(Transform targetTransform, Vector3 target, float duration, EaseType ease)
    {
        posTargetTransform = targetTransform;
        posStart = targetTransform.localPosition;
        posEnd = target;
        posDuration = Mathf.Max(duration, 0.001f);
        posElapsed = 0f;
        posActive = true;
        posEase = ease;
    }

    public void AnimateRotation(Transform targetTransform, Quaternion target, float duration, EaseType ease)
    {
        rotTargetTransform = targetTransform;
        rotStart = targetTransform.localRotation;
        rotEnd = target;
        rotDuration = Mathf.Max(duration, 0.001f);
        rotElapsed = 0f;
        rotActive = true;
        rotEase = ease;
    }

    public void AnimateScale(Transform targetTransform, Vector3 target, float duration, EaseType ease)
    {
        scaleTargetTransform = targetTransform;
        scaleStart = targetTransform.localScale;
        scaleEnd = target;
        scaleDuration = Mathf.Max(duration, 0.001f);
        scaleElapsed = 0f;
        scaleActive = true;
        scaleEase = ease;
        bounceReturning = false;
    }

    public void Bounce(Transform targetTransform, float duration, Vector3 bounceAmount)
    {
        scaleTargetTransform = targetTransform;
        scaleStart = targetTransform.localScale;
        scaleEnd = scaleStart + bounceAmount;
        scaleDuration = Mathf.Max(duration, 0.001f);
        scaleElapsed = 0f;
        scaleActive = true;
        scaleEase = EaseType.BounceOut;
        bounceReturning = false;
    }

    private float Ease(float t, EaseType ease)
    {
        switch (ease)
        {
            case EaseType.EaseInQuad: return t * t;
            case EaseType.EaseOutQuad: return t * (2 - t);
            case EaseType.EaseInOutQuad: return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;

            case EaseType.EaseInCubic: return t * t * t;
            case EaseType.EaseOutCubic: t -= 1; return t * t * t + 1;
            case EaseType.EaseInOutCubic: return t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;

            case EaseType.EaseInQuart: return t * t * t * t;
            case EaseType.EaseOutQuart: t -= 1; return 1 - t * t * t * t;
            case EaseType.EaseInOutQuart: return t < 0.5f ? 8 * t * t * t * t : 1 - 8 * (t - 1) * (t - 1) * (t - 1) * (t - 1);

            case EaseType.EaseInQuint: return t * t * t * t * t;
            case EaseType.EaseOutQuint: t -= 1; return 1 + t * t * t * t * t;
            case EaseType.EaseInOutQuint: return t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * (t - 1) * (t - 1) * (t - 1) * (t - 1) * (t - 1);

            case EaseType.EaseInSine: return 1 - Mathf.Cos(t * Mathf.PI / 2);
            case EaseType.EaseOutSine: return Mathf.Sin(t * Mathf.PI / 2);
            case EaseType.EaseInOutSine: return -0.5f * (Mathf.Cos(Mathf.PI * t) - 1);

            case EaseType.EaseInExpo: return (t == 0f) ? 0f : Mathf.Pow(2, 10 * (t - 1));
            case EaseType.EaseOutExpo: return (t == 1f) ? 1f : 1 - Mathf.Pow(2, -10 * t);
            case EaseType.EaseInOutExpo:
                if (t == 0f) return 0f;
                if (t == 1f) return 1f;
                if (t < 0.5f) return Mathf.Pow(2, 20 * t - 10) / 2;
                return (2 - Mathf.Pow(2, -20 * t + 10)) / 2;

            case EaseType.EaseInCirc: return 1 - Mathf.Sqrt(1 - t * t);
            case EaseType.EaseOutCirc: t -= 1; return Mathf.Sqrt(1 - t * t);
            case EaseType.EaseInOutCirc:
                if (t < 0.5f) return (1 - Mathf.Sqrt(1 - 4 * (t * t))) / 2;
                t = t * 2 - 2;
                return (Mathf.Sqrt(1 - t * t) + 1) / 2;

            case EaseType.EaseInBack:
                const float s1 = 1.70158f;
                return t * t * ((s1 + 1) * t - s1);
            case EaseType.EaseOutBack:
                const float s2 = 1.70158f;
                t -= 1;
                return t * t * ((s2 + 1) * t + s2) + 1;
            case EaseType.EaseInOutBack:
                const float s3 = 1.70158f * 1.525f;
                if (t < 0.5f)
                    return (t * 2) * (t * 2) * ((s3 + 1) * (t * 2) - s3) / 2;
                else
                {
                    t = t * 2 - 2;
                    return (t * t * ((s3 + 1) * t + s3) + 2) / 2;
                }

            case EaseType.BounceOut: return BounceEaseOut(t);
            case EaseType.ElasticOut: return ElasticEaseOut(t);

            case EaseType.Linear:
            default:
                return t;
        }
    }

    private float BounceEaseOut(float t)
    {
        // Smooth single bounce arc
        float s = 1.5f; // lower = less overshoot
        t = Mathf.Clamp01(t);
        return 1f - Mathf.Pow(1f - t, 2) * (s - s * t); // custom curve with overshoot ease-out
    }

    private float ElasticEaseOut(float t)
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;
        float p = 0.3f;
        return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - p / 4f) * (2 * Mathf.PI) / p) + 1;
    }
}
