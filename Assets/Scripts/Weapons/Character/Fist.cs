using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {

    public class Fist : Weapon
    {

        public GameObject AirplanePrefab;
		public GameObject HelicopterPrefab;

        public override void ManualUpdate()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateAirplane();
            }

			if (Input.GetKeyDown(KeyCode.R))
			{
				CreateHelicopter();
			}
        }

        private void CreateAirplane()
        {
            var trans = GetComponent<Transform>();
            var pos = trans.position + trans.forward * 10;
            CmdCreateAirplane(pos);
        }

        [Command]
        private void CmdCreateAirplane(Vector3 pos)
        {
			var airplaneInstance = Instantiate(AirplanePrefab, pos, Quaternion.identity) as GameObject;
            airplaneInstance.name = airplaneInstance.name + "_" + gameObject.name;
			airplaneInstance.GetComponent<Enginery> ().Pilot = gameObject;

			NetworkServer.Spawn(airplaneInstance);
			airplaneInstance.GetComponent<NetworkIdentity>().AssignClientAuthority (connectionToClient);
			gameObject.SetActive(false);
        }

		private void CreateHelicopter()
		{
			var trans = GetComponent<Transform>();
			var pos = trans.position + trans.forward * 10;
			CmdCreateHelicopter(pos);
		}

		[Command]
		private void CmdCreateHelicopter(Vector3 pos)
		{
			var helicopterInstance = Instantiate(HelicopterPrefab, pos, Quaternion.identity) as GameObject;
			helicopterInstance.name = helicopterInstance.name + "_" + gameObject.name;
			helicopterInstance.GetComponent<Enginery> ().Pilot = gameObject;

			NetworkServer.Spawn(helicopterInstance);
			helicopterInstance.GetComponent<NetworkIdentity>().AssignClientAuthority (connectionToClient);
			gameObject.SetActive(false);
		}

        public override void SetDefaults()
        {

        }

    }

}
