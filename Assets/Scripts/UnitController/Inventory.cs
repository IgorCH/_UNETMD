using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

/*
	Возможно стоит придумать InventoryType
	и им инициализировать
	!строковый параметр названия ассета с забитыми данными
	требуется дефолтное состояние для каждого типа контроллера 
	(tank airplane gnome goblin etc)
*/
namespace MostDanger {
	
	public class Inventory: NetworkBehaviour {

		public Weapon CurrentWeapon;
		public WeaponStruct[] Weapons;

		public Inventory () {
			
		}

		public void CreateInventory(InventoryType type) {
			
			Debug.Log(type.ToString ());

		}

		public void SetCurrentWeapon (WeaponStruct weapon) {

			CurrentWeapon = (Weapon)gameObject.AddComponent(Type.GetType(weapon.ScriptName));
		
		}

	}

}
