using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _gameOverTextCanvasGroup;
    [SerializeField] CanvasGroup _returnToTitleCanvasGroup;
    [SerializeField] float _gameOverTextFadeTime = 1.0f;
    [SerializeField] float _returnToTitleTextFadeTime = 1.0f;

    bool _canReturnTOTitleInput = false;

    private void Start()
    {
        _gameOverTextCanvasGroup.alpha = 0;
        _gameOverTextCanvasGroup.interactable = false;
        _gameOverTextCanvasGroup.blocksRaycasts = false;
        _returnToTitleCanvasGroup.alpha = 0;
        _returnToTitleCanvasGroup.interactable = false;
        _returnToTitleCanvasGroup.blocksRaycasts = false;
    }

    private void Update()
    {
        if (Input.anyKeyDown && _canReturnTOTitleInput)
        {
#if UNITY_EDITOR
            Debug.Log("GO TitleScene");
#endif
            SceneManager.LoadScene("Title");
        }
    }

    public void StartGameOverAnimation()
    {
        _gameOverTextCanvasGroup.DOFade(1.0f, _gameOverTextFadeTime).OnComplete(() =>
        {
            _canReturnTOTitleInput = true;
            _returnToTitleCanvasGroup.DOFade(1.0f, _returnToTitleTextFadeTime).SetLoops(-1, LoopType.Yoyo);
        });
    }
}
