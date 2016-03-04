using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger
{

    public class Dynamite : Weapon 
    {

		public GameObject DynamiteShellPrefab;

        public new void Awake()
        {
			base.Awake (); 
        }

        private void Start()
        {

        }

        public override void OnSelect()
        {
			Debug.Log ("Activate Dynamite in hands");
        }

        public override void OnDeselect()
        {
			Debug.Log ("Deactivate Dynamite in hands");
        }

        public override void ManualUpdate()
        {

            CameraParams = new Vector3(0, 0, 0);

			if (Input.GetMouseButtonDown (0))
			{
				CreateDynamite ();
			}

        }

		private void CreateDynamite()
        {
			CmdCreateDynamite(__transform.position + __transform.forward * 3 + __transform.up * 3);
		}

		[Command]
		private void CmdCreateDynamite(Vector3 position)
		{
			GameObject shellInstance = Instantiate(DynamiteShellPrefab, position, Quaternion.identity) as GameObject;
			NetworkServer.Spawn(shellInstance);
        }

        public override void SetDefaults()
        {

        }

    }

}