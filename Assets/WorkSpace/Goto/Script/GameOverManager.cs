using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _gameOverTextCanvasGroup;
    [SerializeField]
    CanvasGroup _goTitleCanvasGroup;
    [SerializeField]
    float _gameOverTextFadeTime = 1.0f;
    [SerializeField]
    float _goTitleTextFadeTime = 1.0f;

    bool _canGoTitleInput = false;

    private void Start()
    {
        _gameOverTextCanvasGroup.alpha = 0;
        _gameOverTextCanvasGroup.interactable = false;
        _gameOverTextCanvasGroup.blocksRaycasts = false;
        _goTitleCanvasGroup.alpha = 0;
        _goTitleCanvasGroup.interactable = false;
        _goTitleCanvasGroup.blocksRaycasts = false;
    }

    private void Update()
    {
        if (Input.anyKeyDown && _canGoTitleInput)
        {
#if UNITY_EDITOR
            Debug.Log("GO TitleScene");
#endif
            //SceneManager.LoadScene("Title");
        }
    }

    public void StartGameOverAnimation()
    {
        _gameOverTextCanvasGroup.DOFade(1.0f, _gameOverTextFadeTime).OnComplete(() =>
        {
            _canGoTitleInput = true;
            _goTitleCanvasGroup.DOFade(1.0f, _goTitleTextFadeTime).SetLoops(-1, LoopType.Yoyo);
        });
    }
}
