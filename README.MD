
# NAnimate

NAnimate is a lightweight, easy-to-use UdonSharp animation framework for VRChat. It provides per-transform animation handling for position, rotation, scale, and bounce effects with smooth easing functions.

## Features

- Concurent animations (Up to 16 simultaneous animations.)
- Animate position, rotation, and scale on any `Transform`
- Built-in easing types:
  - Linear
  - EaseIn
  - EaseOut
  - EaseInOut
  - BounceOut (customizable, smooth "pop" effect)
  - ElasticOut (springy easing)
  - And many more!
- Utility `Bounce()` method for quick and dirty "pop" animations
- Lightweight, one-at-a-time per animation type per object

## Usage

### Animate Position

```csharp
NAnimate.AnimatePosition(myTransform, new Vector3(1, 2, 3), 1.0f, EaseType.EaseOut);
```

### Animate Rotation

```csharp
NAnimate.AnimateRotation(myTransform, Quaternion.Euler(0, 180, 0), 1.5f, EaseType.EaseInOut);
```

### Animate Scale

```csharp
NAnimate.AnimateScale(myTransform, Vector3.one * 1.5f, 0.75f, EaseType.ElasticOut);
```

### Bounce (Pop Effect)

```csharp
NAnimate.Bounce(myTransform, 0.5f, new Vector3(0.2f, 0.2f, 0.2f));
```

> ✅ All easing functions run independently per animation type (pos, rot, scale).

## Installation

1. Add this script to your UdonSharp project (inside `Assets/`).
2. Attach `NAnimate` to any active GameObject.
3. Call the methods via referencing NAnimate.

> You must be using UdonSharp (VRChat SDK3 + Udon) to use this script.

## Supported Easing Types

NAnimate supports a variety of easing functions:

| Easing Type     | Description |
|------------------|-------------|
| `Linear`         | Constant speed from start to end. |
| `EaseIn`         | Starts slow and accelerates. |
| `EaseOut`        | Starts fast and decelerates. |
| `EaseInOut`      | Starts and ends slow, fast in the middle. |
| `BounceOut`      | Bounces like a ball hitting the ground. |
| `ElasticOut`     | Overshoots and oscillates like a spring. |
| `SmoothInOut`    | Smooth curve similar to `EaseInOut` but with gentler transition. |
| `QuadIn`         | Quadratic ease-in (t²), starts slow. |
| `QuadOut`        | Quadratic ease-out, ends slow. |
| `QuadInOut`      | Combines `QuadIn` and `QuadOut`. |
| `CubicIn`        | Cubic ease-in (t³), starts slower. |
| `CubicOut`       | Cubic ease-out, ends slower. |
| `CubicInOut`     | Combines `CubicIn` and `CubicOut`. |


And many more. (Check EaseType enum for more info.)

## License

NAnimate is licensed under the MIT License.
Copyright © 2025 by Netcat.
---
