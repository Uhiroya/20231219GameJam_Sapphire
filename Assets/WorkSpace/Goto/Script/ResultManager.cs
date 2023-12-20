using UnityEngine;
using UniRx;
using DG.Tweening;

public class ResultManager : MonoBehaviour
{
    //[SerializeField]
    //CanvasGroup _winCanvasGroup;
    [SerializeField]
    CanvasGroup _loseCanvasGroup;
    [SerializeField]
    GameOverManager _gameOverManager;
    [SerializeField]
    float _fadeTime = 1f;

    private void Start()
    {
        //_winCanvasGroup.alpha = 0;
        //_winCanvasGroup.interactable = false;
        //_winCanvasGroup.blocksRaycasts = false;
        _loseCanvasGroup.alpha = 0;
        _loseCanvasGroup.interactable = false;
        _loseCanvasGroup.blocksRaycasts = false;

        InGameManager.Instance.OnFinishGame.Subscribe(result =>
        {
            switch (result)
            {
                case ResultType.Win:
                    //_winCanvasGroup.DOFade(1.0f, _fadeTime)
                    //.OnComplete(() =>
                    //{
                    //    _winCanvasGroup.interactable = true;
                    //    _winCanvasGroup.blocksRaycasts = true;
                    //});
                    break;
                case ResultType.Lose:
                    _loseCanvasGroup.DOFade(1.0f, _fadeTime)
                    .OnComplete(() =>
                    {
                        _gameOverManager.StartGameOverAnimation();
                    });
                    break;
            }
        });
    }
}
