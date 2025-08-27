using DG.Tweening;
using UnityEngine;
using VInspector;

public class TransitionLevelIndicator : MonoBehaviour
{
    [SerializeField] private Vector2 endPos;
    [SerializeField] private float transitionTime = .25f;
    
    [Button]
    public void OnStartTransition()
    {
        var rectTransfrom = transform as RectTransform;
        if (!rectTransfrom) return;
        DOTween.To(() => rectTransfrom.position, x => rectTransfrom.position = x, new Vector3(rectTransfrom.position.x, rectTransfrom.position.y + 200f), transitionTime);
    }
}
