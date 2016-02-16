using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {

    public class Fist : Weapon
    {

        public GameObject AirplanePrefab;

        public override void ManualUpdate()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateAirplane();
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

			//NetworkServer.Spawn(airplaneInstance);

			Debug.Log(NetworkServer.SpawnWithClientAuthority(airplaneInstance, connectionToClient));
			gameObject.SetActive(false);
        }

        public override void SetDefaults()
        {

        }

    }

}
