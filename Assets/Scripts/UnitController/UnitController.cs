using UnityEngine;
using System.Collections;

namespace MostDanger {
	
	[RequireComponent(typeof(Inventory))]
	public class UnitController : MonoBehaviour {

		protected Inventory unitInventory;

		//Базовые фозможности связывающие камеру с юнитом
		//Работа с камерой

		//возможно стоит отделить от наследования в пользу компонента
		//и работать с событиями типа OnWeaponChanged

		void Start ()
		{
			unitInventory = GetComponent<Inventory> ();
		}

		void Update ()
		{
		
		}

		void LateUpdate () 
		{
			
		}
	}

}