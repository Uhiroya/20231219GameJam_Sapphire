using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup _startCanvasGroup;
    [SerializeField]
    float _imageFadeTime = 1f;

    private void Start()
    {
        _startCanvasGroup.alpha = 0;
        _startCanvasGroup.DOFade(1.0f, _imageFadeTime).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
#if UNITY_EDITOR
            Debug.Log("GO InGameScene");
#endif
            //SceneManager.LoadScene("InGame");
        }
    }
}
