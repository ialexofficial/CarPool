using System;
using CarPool.States.Tutorial.Steps;
using CarPool.UI.Views;
using Ji2.Context;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core.States;

namespace CarPool.States.Tutorial
{
    public class TutorialStepFactory : ITutorialFactory
    {
        private readonly Context _context;
        private readonly StateMachine _stateMachine;
        private readonly TutorialPointer _tutorialPointer; 
        
        public TutorialStepFactory(Context context, StateMachine stateMachine, TutorialPointer tutorialPointer)
        {
            _context = context;
            _stateMachine = stateMachine;
            _tutorialPointer = tutorialPointer;
        }
        
        public ITutorialStep Create<TStep>() where TStep : ITutorialStep
        {
            if (typeof(TStep) == typeof(InitialTutorialStep))
            {
                return new InitialTutorialStep(_context, _stateMachine, _tutorialPointer);
            }

            if (typeof(TStep) == typeof(MultipleCarTutorialStep))
            {
                return new MultipleCarTutorialStep(_context, _stateMachine, _tutorialPointer);
            }

            throw new NotImplementedException("No such tutorial step");
        }
    }
}