using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDEUtils.StateMachine
{
    public class StateMachine<T>
    {
        public State<T> CurrentState { get; private set; }
        public Stack<State<T>> StateStack { get; private set; }

        T owner;
        public StateMachine(T owner)
        {
            this.owner = owner;
            StateStack = new Stack<State<T>>();
        }

        public void Execute()
        {
            CurrentState?.Execute();
        }


        public void Push(State<T> newState, bool exitCurrState=false)
        {
            CurrentState?.OnLostFocus();

            if (exitCurrState)
                CurrentState?.Exit();

            StateStack.Push(newState);
            CurrentState = newState;
            CurrentState.Enter(owner);
        }

        public void Pop(bool enterPrevState=false)
        {
            StateStack.Pop();
            CurrentState.Exit();
            CurrentState = StateStack.Peek();

            if (enterPrevState)
                CurrentState?.Enter(owner);

            CurrentState?.OnGainedFocus();
        }

        public void ChangeState(State<T> newState)
        {
            if (CurrentState != null)
            {
                StateStack.Pop();
                CurrentState.Exit();
            }

            StateStack.Push(newState);
            CurrentState = newState;
            CurrentState.Enter(owner);
        }

        public IEnumerator PushAndWait(State<T> newState)
        {
            var oldState = CurrentState;
            Push(newState);
            yield return new WaitUntil(() => CurrentState == oldState);
        }
    }
}
