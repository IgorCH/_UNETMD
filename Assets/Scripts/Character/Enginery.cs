using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger
{
    public class Enginery : NetworkBehaviour
    {
		[SyncVar]
		public GameObject Pilot;

    }
}
