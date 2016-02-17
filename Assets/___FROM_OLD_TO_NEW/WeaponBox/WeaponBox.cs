using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponBox : NetworkBehaviour
{

    /*public WeaponType WeaponType;
    public int Count;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<CharacterController>() != null)
        {
            Messager.SharedMessager.SetText("Pick up " + WeaponType.ToString() + " x" + Count);
            collider.GetComponent<CharacterController>().TakeWeapon(WeaponType, Count);
            Destroy(gameObject);
        }
    }*/
}
