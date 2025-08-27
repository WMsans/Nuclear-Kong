using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VInspector;

public class CameraShake : MonoSingleton<CameraShake>
{
    [Button]
    public void OnShake(float duration = 0.2f, float strength = .7f)
    {
        transform.DOShakePosition(duration, strength).SetUpdate(true);
        transform.DOShakeRotation(duration, strength).SetUpdate(true);
    }
}
