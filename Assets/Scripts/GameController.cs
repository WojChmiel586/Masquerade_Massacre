using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        // Singleton instance
        public static GameController Instance;
        
        //  Round timer is the overall game length.
        private float roundTimerLimit;
        public float RoundTimerLimit => roundTimerLimit;
        private float roundTimer;
        public float RoundTimer => roundTimer;
        
        // Assassin timer is how long you have before losing.
        [SerializeField] private float assassinTimerMin;
        [SerializeField] private float assassinTimerMax;

        private float assassinTimer;

        public float AssassinTimer => assassinTimer;

        private float assassinTimerLimit;

        public float AssassinTimerLimit => assassinTimerLimit;
        
        //  Game State
        private SimpleStateMachine<GameState> gameState;
        public GameState CurrentGameState
        {
            get { return gameState.State; }
            set { gameState.SetState(value); }
        }
        public bool IsInitComplete => CurrentGameState is not GameState.Initialising;
        //  Game state transition events
        private SimpleStateMachine<TargetState> targetState;
        public TargetState CurrentTargetState
        {
            get { return targetState.State; }
            set { targetState.SetState(value); }
        }

        public bool IsGamePaused => CurrentGameState is GameState.Paused;
        
        private void Awake() {
            // Ensure singleton instance
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);  // Ensure only one UIManager exists
            }

        }

        private void Start()
        {
            //  Initialise new state machines for game and assassin (target)
            gameState = new(GameState.Initialising);
            gameState.StateChangeEvent.AddListener(OnGameStateChange);
            targetState = new(TargetState.INACTIVE);
            targetState.StateChangeEvent.AddListener(OnTargetStateChange);
            CurrentTargetState = TargetState.INACTIVE;
            roundTimer = 0;
            Debug.Log("Started. Current game state is  " + CurrentGameState.ToString());
        }

        private void Update()
        {
            CheckWinState();
            CheckLoseState();
        }

        private void CheckWinState()
        {
            //  Win criteria here
            var gameIsRunning = CurrentGameState is GameState.Playing;
            var timerHasExpired = assassinTimer <= 0;
            var roundTimerHasExpired = roundTimer <= 0;
            if (gameIsRunning && roundTimerHasExpired)
            {
                gameState.SetState(GameState.Win);
            }
        }

        private void CheckLoseState()
        {
            // Lose criteria here
            var gameIsRunning = CurrentGameState is GameState.Playing;
            var timerHasExpired = assassinTimer <= 0;
            var roundTimerHasExpired = roundTimer <= 0;
            if (gameIsRunning && timerHasExpired && !roundTimerHasExpired)
            {
                gameState.SetState(GameState.Lose);
            }
        }
        
        private void FindNewTarget()
        {
            //  Find a new assassin target and set a new assassin timer
            roundTimerLimit = Random.Range(assassinTimerMin, assassinTimerMax);
            roundTimer = roundTimerLimit;
            Debug.Log("Assigning new target.");
        }

        private void OnGameStateChange(GameState newState, GameState oldState)
        {            
            Debug.Log("State is updating. new game state is  " + CurrentGameState.ToString());

            switch (newState)
            {
                case GameState.Playing:
                    break;
                case GameState.Paused:
                    if (oldState is GameState.Playing or GameState.Initialising)
                    {
                        if (CurrentTargetState is TargetState.INACTIVE or TargetState.KIA)
                        {
                            FindNewTarget();
                        }
                    }
                    break;
                case GameState.Lose:
                    break;
                case GameState.Win:
                    break;
                default:
                    break;
            }
        }
        private void OnTargetStateChange(TargetState newState, TargetState oldState)
        {
            switch (newState)        
            {
                case TargetState.ACTIVE:
                    break;
                case TargetState.INACTIVE:
                    break;
                case TargetState.KIA:
                    break;
                default:
                    break;
            }
        }
    }
}