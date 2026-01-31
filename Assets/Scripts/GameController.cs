using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private float roundTimerMin;
        [SerializeField] private float roundTimerMax;
        private float roundTimerLimit;
        public float RoundTimerLimit => roundTimerLimit;
        private float roundTimer;
        public float RoundTimer => roundTimer;
        
        // Singleton instance
        public static GameController Instance;
        public UnityAction acquireTargetEvent;
        
        
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
            acquireTargetEvent += OnFindNewTarget;
            acquireTargetEvent?.Invoke();
        }

        private void OnFindNewTarget()
        {
            roundTimerLimit = Random.Range(roundTimerMin, roundTimerMax);
            roundTimer = roundTimerLimit;
        }
    }
}