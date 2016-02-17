using UnityEngine;
using System.Collections;

/*
 * TODO Это базовый портал
 * Можно добавить односторонние
 */
public class Portal : MonoBehaviour
{

    public Transform MyTargetObject;
    public Portal TargetPortal;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<CharacterController>() != null && TargetPortal != null)
        {
            collider.transform.position = TargetPortal.MyTargetObject.position;
            collider.transform.rotation = TargetPortal.MyTargetObject.rotation;
        }
    }
}
