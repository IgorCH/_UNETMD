using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace MostDanger {
		
	public class TurnBasedController : NetworkBehaviour {

		static public TurnBasedController Instance;
		static public List<CharacterManager> Characters = new List<CharacterManager> ();
		public GameObject CharacterPrefab;

		[HideInInspector]
		[SyncVar]
		public bool IsGameFinished = false;

		void Awake()
		{
			Instance = this;
		}

		void Start ()
		{

		}

		void Update ()
		{

		}
	}

}
