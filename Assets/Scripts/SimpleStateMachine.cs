using System;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class SimpleStateMachine<T> where T : Enum
    {
        private T currentState;
        private T previousState;
        
        public T State => currentState;
        public UnityEvent<T,T> StateChangeEvent;
    
        public SimpleStateMachine(T state)
        {
          currentState = state;
          previousState = currentState;
          StateChangeEvent = new UnityEvent<T, T>();
        }

        public void SetState(T state)
        {
            previousState = currentState;
            currentState = state;
            StateChangeEvent?.Invoke(currentState, previousState);
        }
    }
}