using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    
    public class AssassinController : MonoBehaviour
    {
        // Assassin data struct goes here
        // public var assassinData
        private PatrolAgent2D currentAssassin;

        private SpawnManager spawnManager;
        private void Start()
        {
            spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }
        
    }
}