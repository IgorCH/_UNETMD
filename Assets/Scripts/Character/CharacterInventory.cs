using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

namespace MostDanger {

	public class CharacterInventory : NetworkBehaviour
	{
		public int m_PlayerNumber = 1;

		public Weapon CurrentWeapon;
		public List<WeaponStruct> Weapons;

	    private void Awake()
	    {
			Weapons = new List<WeaponStruct> ();

			Weapons.Add (new WeaponStruct() {
				Name = "Fist",
				ScriptName = "Fist",
				Count = -1
			});

			Weapons.Add (new WeaponStruct() {
				Name = "Bazooka",
				ScriptName = "Bazooka",
				Count = 100
			});	

			CurrentWeapon = (Weapon)gameObject.GetComponent("MostDanger.Fist");
		}
			
	    private void Start()
	    {
	     
	    }

	    [ClientCallback]
	    private void Update()
	    {

			if (!isLocalPlayer)
	            return;

			if (Input.GetKeyDown (KeyCode.Space))
			{
				if (InventoryGUI.Instance.IsOpened)
				{
					InventoryGUI.Instance.Close ();
				}
				else
				{
					InventoryGUI.Instance.Open (Weapons, OnInventorySelect);
				}
			}

			if (CurrentWeapon)
			{
				CurrentWeapon.ManualUpdate ();
			}
				
	    }

		private void OnInventorySelect (WeaponStruct weapon) 
		{
			if (CurrentWeapon)
			{
				Destroy (CurrentWeapon);
			}
			CurrentWeapon = (Weapon)gameObject.GetComponent("MostDanger." + Type.GetType(weapon.ScriptName));
            Debug.Log(CurrentWeapon);
		}

		public void SetDefaults()
		{
			if (CurrentWeapon)
			{
				CurrentWeapon.SetDefaults ();
			}
		}

	}

}