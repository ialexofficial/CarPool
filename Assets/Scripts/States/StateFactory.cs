using System;
using System.Collections.Generic;
using Ji2.Context;
using Ji2Core.Core.States;

namespace CarPool.States
{
    public class StateFactory : IStateFactory
    {
        private readonly Context _context;

        public StateFactory(Context context)
        {
            _context = context;
        }

        public Dictionary<Type, IExitableState> GetStates(StateMachine stateMachine) => new()
        {
            {typeof(InitialState), new InitialState(_context, stateMachine)},
            {typeof(LevelLoadingState), new LevelLoadingState(_context, stateMachine)},
            {typeof(GameState), new GameState(_context, stateMachine)},
            {typeof(LevelEndingState), new LevelEndingState(stateMachine)}
        };
    }
}