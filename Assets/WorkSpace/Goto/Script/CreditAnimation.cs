using UnityEngine;
using DG.Tweening;

public class CreditAnimation : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _winCanvasGroup;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    float _endPositionY = 0;
    [SerializeField]
    float _whitePanelAnimationTime = 0;
    [SerializeField]
    float _animationTime = 1f;

    private void Start()
    {
        _winCanvasGroup.alpha = 0;
        _winCanvasGroup.interactable = false;
        _winCanvasGroup.blocksRaycasts = false;
    }

    public void StartCreditAnimation()
    {
        _winCanvasGroup.DOFade(1.0f, _whitePanelAnimationTime).OnComplete(() =>
        rectTransform.DOAnchorPos(new Vector2(0, _endPositionY), _animationTime)
        );

    }
}
