using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {
	
	public class ClusterBombShell : NetworkBehaviour {

		private const float TimeToExplosion = 5;


		private float _timer;


		private void Awake()
		{

		}

		private void Start()
		{

		}

		void Update()
		{
			_timer -= Time.deltaTime;
			if (_timer < 0)
			{
				Fire ();
			}
		}

		private void Fire()
		{
			CmdFire ();
		}

		[Command]
		private void CmdFire()
		{

		}


	}

}
