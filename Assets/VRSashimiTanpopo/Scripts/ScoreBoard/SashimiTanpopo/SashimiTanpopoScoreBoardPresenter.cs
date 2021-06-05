using System;
using UniRx;
using VRSashimiTanpopo.Model;
using VContainer.Unity;

namespace VRSashimiTanpopo.ScoreBoard.SashimiTanpopo
{
    public class SashimiTanpopoScoreBoardPresenter : IStartable, IDisposable
    {
        readonly SashimiTanpopoScoreBoardView sashimiTanpopoScoreBoardView;

        readonly GameModel gameModel;
        readonly ScoreModel scoreModel;
        readonly CountdownTimerModel countdownTimerModel;

        readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        public SashimiTanpopoScoreBoardPresenter(
            SashimiTanpopoScoreBoardView sashimiTanpopoScoreBoardView,
            GameModel gameModel,
            ScoreModel scoreModel,
            CountdownTimerModel countdownTimerModel)
        {
            this.sashimiTanpopoScoreBoardView = sashimiTanpopoScoreBoardView;
            this.gameModel = gameModel;
            this.scoreModel = scoreModel;
            this.countdownTimerModel = countdownTimerModel;
        }

        public void Start()
        {
            scoreModel.Score.Subscribe(sashimiTanpopoScoreBoardView.UpdateScoreText).AddTo(compositeDisposable);
            gameModel.IsGameOver.Subscribe(sashimiTanpopoScoreBoardView.UpdateGameOverText).AddTo(compositeDisposable);
            gameModel.Playing.Subscribe(sashimiTanpopoScoreBoardView.EnableTextBlinker).AddTo(compositeDisposable);
            
            countdownTimerModel.TimeRemaining.Subscribe(sashimiTanpopoScoreBoardView.UpdateTimeRemainingText).AddTo(compositeDisposable);
        }

        public void Dispose()
        {
            compositeDisposable.Dispose();
        }
    }
}
