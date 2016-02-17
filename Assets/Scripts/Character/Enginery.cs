using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger
{
    public class Enginery : NetworkBehaviour
    {
		[SyncVar]
        [HideInInspector]
		public GameObject Pilot;

    }
}
