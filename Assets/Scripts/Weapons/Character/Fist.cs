using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

namespace MostDanger {

    public class Fist : Weapon
    {

        public GameObject AirplanePrefab;

        private void Awake()
        {
        }

        private void Start()
        {
        }

        public override void ManualUpdate()
        {

            if (Input.GetKeyDown(KeyCode.R))
            {
                CreateAirplane();
            }

        }

        private void CreateAirplane()
        {
            var trans = GetComponent<Transform>();
            var pos = trans.position + trans.forward * 10 + trans.up * 2;
            CmdCreateAirplane(pos);
        }

        [Command]
        private void CmdCreateAirplane(Vector3 pos)
        {
            var airplaneInstance = Instantiate(AirplanePrefab, pos, Quaternion.identity) as GameObject;
            NetworkServer.SpawnWithClientAuthority(airplaneInstance, connectionToClient);
            gameObject.SetActive(false);
        }

        public override void SetDefaults()
        {

        }

    }

}
