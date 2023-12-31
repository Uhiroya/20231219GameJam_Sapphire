using System;
using DG.Tweening;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : SingletonMonoBehavior<UiManager>
{
    [SerializeField] private CanvasGroup _canvasGroup;

    //弾数
    [SerializeField] Text _bulletCountText;

    //敵数
    [SerializeField] Text _enemyCountText;

    //タイマー
    [SerializeField] Text _timerText;

    //照準
    [SerializeField] Image _image;

    private void Start()
    {
        SetActiveTimeText(false);
        InGameManager.Instance.OnStartDreamAsObservable.Subscribe(_ => { SetActiveTimeText(true); });
        InGameManager.Instance.OnStartRealAsObservable.Subscribe(_ => { SetActiveTimeText(false); });

        InGameManager.Instance.OnFinishGame.Subscribe(_ => { _canvasGroup.DOFade(0f, 1f); });
    }

    public void SetBulletCountText(int count)
    {
        _bulletCountText.text = "×" + count.ToString();
    }

    public void SetEnemyCountText(int count)
    {
        _enemyCountText.text = "×" + count.ToString();
    }

    public void SetActiveTimeText(bool isActive)
    {
        _timerText.gameObject.SetActive(isActive);
    }

    public void SetTimeText(float time)
    {
        int seconds = (int)(time / 60);　//秒
        int milisecond = (int)(time % 60);　//ミリ秒
        _timerText.text = $"{seconds.ToString("00")} : {milisecond.ToString("00")}";
    }

    public void SetImage(bool flg)
    {
        _image.gameObject.SetActive(false);
    }
}
