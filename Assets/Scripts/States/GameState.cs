using CarPool.Level.Presenters;
using CarPool.UI.Views;
using Cysharp.Threading.Tasks;
using Ji2.Context;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Ji2Core.Core.Tools;

namespace CarPool.States
{
    public class GameState : IPayloadedState<GamePayload>
    {
        private readonly Context _context;
        private readonly StateMachine _stateMachine;
        private readonly ScreenNavigator _screenNavigator;
        private readonly TimeScaler _timeScaler;

        public GamePayload Payload { get; private set; }
        
        public GameState(Context context, StateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;

            _screenNavigator = _context.GetService<ScreenNavigator>();
            _timeScaler = _context.GetService<TimeScaler>();
        }
        
        public async UniTask Enter(GamePayload payload)
        {
            Payload = payload;

            Payload.LevelPresenter.OnLevelLoad += OnLevelEnded;
            Payload.LevelPresenter.BuildView(
                await _screenNavigator.PushScreen<GameScreen>(),
                _timeScaler
            );
        }

        public async UniTask Exit()
        {
            Payload.LevelPresenter.Clear();
            _timeScaler.ResetScale();
        }

        private void OnLevelEnded()
        {
            _stateMachine.Enter<LevelEndingState>();
        }
    }

    public class GamePayload
    {
        public LevelPresenter LevelPresenter { get; }

        public GamePayload(
            LevelPresenter levelPresenter
        )
        {
            LevelPresenter = levelPresenter;
        }
    }
}