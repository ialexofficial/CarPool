using CarPool.Tools;
using Cysharp.Threading.Tasks;
using Ji2.Context;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using UI.Screens;

namespace CarPool.States
{
    public class InitialState : IState
    {
        private readonly Context _context;
        private readonly StateMachine _stateMachine;

        public InitialState(Context context, StateMachine stateMachine)
        {
            _context = context;
            _stateMachine = stateMachine;
        }
        
        public async UniTask Enter()
        {
            var screenNavigator = _context.GetService<ScreenNavigator>();
            await screenNavigator.PushScreen<LoadingScreen>();

            _context.SaveDataContainer.Load();
            _context.LevelsLoopProgress.Load();
            _context.GetService<MoneyDataContainer>().Bootstrap();
            _context.GetService<TutorialService>().TryRunSteps();

            _stateMachine.Enter<LevelLoadingState>();
        }

        public async UniTask Exit()
        {
        }
    }
}