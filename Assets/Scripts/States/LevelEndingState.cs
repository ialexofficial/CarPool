using Cysharp.Threading.Tasks;
using Ji2Core.Core.States;

namespace CarPool.States
{
    public class LevelEndingState : IState
    {
        private readonly StateMachine _stateMachine;

        public LevelEndingState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public async UniTask Enter()
        {
            _stateMachine.Enter<LevelLoadingState>();
        }

        public async UniTask Exit()
        {
        }
    }
}