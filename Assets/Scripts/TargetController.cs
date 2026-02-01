using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    
    public class TargetController : MonoBehaviour
    {
        private GuestIdPacket guestIdentifiers;
        public GuestIdPacket GuestData => guestIdentifiers;
        private PatrolAgent2D currentTarget;

        private SpawnManager spawnManager;
        private void Start()
        {
            spawnManager = GameObject.Find("AIManager").GetComponent<SpawnManager>();
        }

        public GuestIdPacket FindCurrentTarget()
        {
            guestIdentifiers = spawnManager.GetCurrentTarget();
            return guestIdentifiers;
        }

		public void SpawnNewTarget()
		{
			spawnManager.SpawnTarget();
		}

        public GuestIdentifiers GetIdentifiers()
        {
            return spawnManager.CurrentTargetIdentifiers;
        }
    }
}