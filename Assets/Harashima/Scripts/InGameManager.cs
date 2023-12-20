using System;
using UnityEngine;
using UniRx;

public class InGameManager : SingletonMonoBehavior<InGameManager>
{
    [SerializeField, Header("夢状態の制限時間")] private float _dreamTimeLimit = 10f;

    private InGameState _currentState;
    public InGameState CurrentState => _currentState;

    private readonly Subject<Unit> _onStartDreamState = new Subject<Unit>();

    /// <summary>
    /// 夢の状態に遷移する際に呼ばれるイベント
    /// </summary>
    public IObservable<Unit> OnStartDreamAsObservable => _onStartDreamState;

    private readonly Subject<Unit> _onStartRealState = new Subject<Unit>();

    /// <summary>
    /// 現実の状態に遷移する際に呼ばれるイベント
    /// </summary>
    public IObservable<Unit> OnStartRealAsObservable => _onStartRealState;

    private readonly Subject<ResultType> _onFinishGame = new Subject<ResultType>();
    public IObservable<ResultType> OnFinishGame => _onFinishGame;

    /// <summary>
    /// 現在の状態を変更して開始時のイベントを発火する
    /// </summary>
    /// <param name="inGameState"></param>
    public void ChangeInGameState(InGameState inGameState)
    {
        if (_currentState == inGameState)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"同じStateに遷移しようとしています。現在のState{_currentState}");
#endif
            return;
        }

        _currentState = inGameState;

        switch (inGameState)
        {
            case InGameState.Dream:
                _onStartDreamState.OnNext(Unit.Default);
                break;
            case InGameState.Real:
                _onStartRealState.OnNext(Unit.Default);
                break;
        }
    }

    public void FinishGame(ResultType resultType)
    {
        _onFinishGame.OnNext(resultType);
    }
}

public enum InGameState
{
    Dream,
    Real
}

public enum ResultType
{
    Win,
    Lose
}
