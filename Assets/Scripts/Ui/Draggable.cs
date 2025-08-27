using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class Draggable : MonoBehaviour, IDraggable
{
    [Header("Animation Parameters")]
    [SerializeField] private float scaleOnDrag = 1.15f;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    [SerializeField] private Ease moveEase = Ease.OutCubic;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Slot currentSlot;

    private bool isReturning = false;

    public Transform OriginalParent => originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        // Keep track of the current slot the item is in.
        currentSlot = GetComponentInParent<Slot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isReturning) return;
        transform.DOKill();

        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0.6f, animationDuration).SetUpdate(true);
        transform.DOScale(scaleOnDrag, animationDuration).SetEase(scaleEase).SetUpdate(true);

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isReturning) return;
        rectTransform.anchoredPosition += eventData.delta / transform.root.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isReturning) return;

        if (transform.parent == transform.root)
        {
            StartCoroutine(ReturnToSlotCoroutine());
        }
        else
        {
            canvasGroup.DOFade(1f, animationDuration).SetUpdate(true);
            transform.DOScale(1f, animationDuration)
                .SetEase(scaleEase)
                .SetUpdate(true)
                .OnComplete(() => {
                    canvasGroup.blocksRaycasts = true;
                });
        }
    }
    
    private IEnumerator ReturnToSlotCoroutine()
    {
        isReturning = true;
        transform.SetParent(originalParent);

        rectTransform.DOAnchorPos(originalPosition, animationDuration).SetEase(moveEase).SetUpdate(true);
        transform.DOScale(1f, animationDuration).SetEase(scaleEase).SetUpdate(true);
        canvasGroup.DOFade(1f, animationDuration).SetUpdate(true);
        OnReturnToSlot();
        
        yield return new WaitForSecondsRealtime(animationDuration);
        canvasGroup.blocksRaycasts = true;
        isReturning = false;
    }
    
    public void SetAndAnimateToSlot(Slot newSlot)
    {
        currentSlot = newSlot;

        transform.SetParent(newSlot.transform);
        rectTransform.DOAnchorPos(Vector2.zero, animationDuration).SetEase(moveEase).SetUpdate(true);
    }
    
    // This interface method is now a wrapper.
    public void OnDroppedInSlot(Slot slot)
    {
        SetAndAnimateToSlot(slot);
    }

    public void OnLeaveSlot(Slot slot)
    {
        if (currentSlot == slot)
        {
            currentSlot = null;
        }
    }

    public void OnReturnToSlot()
    {
        
    }
}