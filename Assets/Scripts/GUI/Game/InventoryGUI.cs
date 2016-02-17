using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MostDanger {
		
	public class InventoryGUI : MonoBehaviour {

		public static InventoryGUI Instance;
		public bool IsOpened;

		private RectTransform rootTransform;

		public GameObject InventoryRoot;
		public Transform itemsRoot;
		public GameObject InventoryItemPrefab;

		public Text SetSelectectedObjectName;
		public Text SetSelectectedWeaponName;

		private Action<WeaponStruct> Callback;

		void Awake ()
        {
			Instance = this;
			IsOpened = false;
			InventoryRoot.SetActive(IsOpened);
		}

		public void Open (List<WeaponStruct> weapons, Action<WeaponStruct> callback)
		{
		    Cursor.visible = false;
            Callback = callback; 
            
            foreach (Transform child in itemsRoot)
            {
                 Destroy(child.gameObject);
            }

			foreach(var weapon in weapons)
            {
				var newInventoryItem = Instantiate (InventoryItemPrefab);
                newInventoryItem.name = weapon.Name;
				newInventoryItem.GetComponent<RectTransform> ().SetParent(itemsRoot);
			    newInventoryItem.GetComponent<Image>().sprite = Resources.Load<Sprite>(weapon.Name);
			    WeaponStruct localWeapon = weapon;
                newInventoryItem.GetComponent<Button>().onClick.AddListener(() => OnInventoryItemClick(localWeapon));
			}

			IsOpened = true;
			InventoryRoot.SetActive(IsOpened);
		}

		public void Close ()
        {
            Cursor.visible = true;
			IsOpened = false;
			InventoryRoot.SetActive(IsOpened);
		}

		private void OnInventoryItemClick (WeaponStruct item)
		{
			SetSelectedWeapon (item.Name + " (" + item.Count + ")");
		    Close();
			Callback (item);
		}

		public void SetSelectedWeapon (string Name) 
		{
			SetSelectectedWeaponName.text = Name;
		}

		public void SetSelectectedObject (string Name)
		{
			SetSelectectedObjectName.text = Name;
		}
	}

}
