using UnityEngine;
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

		public int CurrentWeaponIdx;
		public Weapon CurrentWeapon;

		public Weapon[] Weapons;

		public Inventory () {
			
		}

		public void SetCurrentWeapon () {
			//Создает на персонаже Weapon Network Behaviour
		}

	}

}
