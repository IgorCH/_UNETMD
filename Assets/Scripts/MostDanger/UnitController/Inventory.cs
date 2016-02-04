using UnityEngine;
using System.Collections;

/*
	Возможно стоит придумать InventoryType
	и им инициализировать
	требуется дефолтное состояние для каждого типа контроллера 
	(tank airplane gnome goblin etc)
*/
namespace MostDanger {
	
	public class Inventory: MonoBehaviour {

		public int CurrentWeaponIdx;
		public Weapon CurrentWeapon;

		public Weapon[] Weapons;

		public Inventory () {
			
		}

	}

}
