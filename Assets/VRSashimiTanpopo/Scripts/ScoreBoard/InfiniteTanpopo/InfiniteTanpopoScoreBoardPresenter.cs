using System;
using UniRx;
using VContainer.Unity;
using VRSashimiTanpopo.Model;

namespace VRSashimiTanpopo.ScoreBoard.InfiniteTanpopo
{
    public class InfiniteTanpopoScoreBoardPresenter : IStartable, IDisposable
    {
        readonly InfiniteTanpopoScoreBoardView infiniteTanpopoScoreBoardView;
        
        readonly WorkingTimeModel workingTimeModel;
        readonly GameModel gameModel;

        readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

        public InfiniteTanpopoScoreBoardPresenter(
            InfiniteTanpopoScoreBoardView infiniteTanpopoScoreBoardView,
            WorkingTimeModel workingTimeModel,
            GameModel gameModel)
        {
            this.infiniteTanpopoScoreBoardView = infiniteTanpopoScoreBoardView;
            this.workingTimeModel = workingTimeModel;
            this.gameModel = gameModel;
        }
        
        public void Start()
        {
            gameModel.IsGameOver.Subscribe(infiniteTanpopoScoreBoardView.UpdateGameOverText).AddTo(compositeDisposable);
            gameModel.Playing.Subscribe(infiniteTanpopoScoreBoardView.EnableTextBlinker)
                .AddTo(compositeDisposable);
            
            workingTimeModel.ElapsedTimeSec.Subscribe(infiniteTanpopoScoreBoardView.UpdateWorkingTimeText)
                .AddTo(compositeDisposable);
        }

        public void Dispose()
        {
            compositeDisposable.Dispose();
        }
    }
}