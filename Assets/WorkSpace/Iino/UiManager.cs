using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    //弾数
    [SerializeField] Text _bulletCountText;
    //敵数
    [SerializeField] Text _enemyCountText;
    //タイマー
    [SerializeField] Text _timerText;
    //照準
    [SerializeField] Image _image;

    public void SetBulletCountText(int count)
    {
        _bulletCountText.text = "×" + count.ToString();
    }

    public void SetEnemyCountText(int count)
    {
        _enemyCountText.text = "×" + count.ToString();
    }

    public void SetTimeText(float time)
    {
        int minutes = (int)(time / 60);　//分
        int seconds = (int)(time % 60);　//秒
        _timerText.text = $"{minutes.ToString("00")} : {seconds.ToString("00")}";
    }

    public void SetImage(bool flg)
    {
        _image.gameObject.SetActive(false);
    }
    private void Start()
    {
        SetBulletCountText(9);
        SetEnemyCountText(3);
        SetTimeText(511);
        SetImage(true);
    }
}
