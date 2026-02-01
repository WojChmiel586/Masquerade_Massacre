using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        // Singleton instance
        public static GameController Instance;
        
        //  Round timer is the overall game length.
        [SerializeField] private float roundTimeLimit;
        public float RoundTimerLimit => roundTimeLimit;
        private float roundTimer;
        public float RoundTimer => roundTimer;
        
        // Assassin timer is how long you have before losing.
        [SerializeField] private float assassinTimerMin;
        [SerializeField] private float assassinTimerMax;

        [FormerlySerializedAs("_targetController")] public TargetController TargetController;

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
        private bool deferGameStart = false;
        private SimpleStateMachine<TargetState> targetState;
        public TargetState CurrentTargetState
        {
            get { return targetState.State; }
            set { targetState.SetState(value); }
        }

        public bool IsGamePaused => CurrentGameState is GameState.Paused;
        
        //Debug
        [SerializeField] private bool setLose = false;
        [SerializeField] private bool setWin = false;
        private void Awake() {
            // Ensure singleton instance
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);  // Ensure only one UIManager exists
            }
            gameState = new(GameState.Initialising);
            TargetController = GetComponentInChildren<TargetController>();
        }

        private void Start()
        {
            //  Initialise new state machines for game and assassin (target)
            gameState.StateChangeEvent.AddListener(OnGameStateChange);
            targetState = new(TargetState.INACTIVE);
            targetState.StateChangeEvent.AddListener(OnTargetStateChange);
            CurrentTargetState = TargetState.INACTIVE;
            roundTimer = roundTimeLimit;
            Debug.Log("Started. Current game state is  " + CurrentGameState.ToString());
            if (UIManager.Instance.initComplete) gameState.SetState(GameState.Menu);
            else deferGameStart = true;
        }

        private void Update()
        {
            if (deferGameStart)
            {
                gameState.SetState(GameState.Menu);
                deferGameStart = false;
            }
            CheckWinState();
            CheckLoseState();
            
            //  Just for debugging win and lose
            if (setWin)
            {
                gameState.SetState(GameState.Win);
                setWin = false;
            }

            if (setLose)
            {
                gameState.SetState(GameState.Lose);
                setLose = false;
            }
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
            assassinTimer = Random.Range(assassinTimerMin, assassinTimerMax);
            TargetController.SpawnNewTarget();
            TargetController.FindCurrentTarget();
        }

        private void OnGameStateChange(GameState newState, GameState oldState)
        {            
            Debug.Log("State is updating. new game state is  " + CurrentGameState.ToString());

            switch (newState)
            {
                case GameState.Playing:
                    if (oldState is GameState.Menu)
                    {
                        roundTimer = roundTimeLimit;
                        assassinTimer = 99f;
                        AudioManager.instance.PlayGameMusic();
                        //FindNewTarget();
                    }
                    //Cursor.lockState = CursorLockMode.Locked;
                    break;
                case GameState.Paused:
                    Cursor.lockState = CursorLockMode.None;
                    if (oldState is GameState.Playing or GameState.Initialising)
                    {
                        if (CurrentTargetState is TargetState.INACTIVE or TargetState.KIA)
                        {
                            FindNewTarget();
                        }
                    }

                    break;
                case GameState.Lose:
                    Cursor.lockState = CursorLockMode.None;
                    UIManager.Instance.ShowPanel("LosePanel");
                    AudioManager.instance.FailSFX();
                    break;
                case GameState.Win:
                    Cursor.lockState = CursorLockMode.None;
                    UIManager.Instance.ShowPanel("WinPanel");
                    AudioManager.instance.WinSFX();
                    break;
                case GameState.Menu:
                    Cursor.lockState = CursorLockMode.None;
                    UIManager.Instance.ShowPanel("MenuPanel");
                    AudioManager.instance.PlayMenuMusic();
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

		public void OnTargetKilled()
		{
			FindNewTarget();
		}
    }
}