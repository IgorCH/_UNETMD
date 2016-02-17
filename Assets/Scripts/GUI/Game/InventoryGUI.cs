using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MostDanger {
		
	public class InventoryGUI : MonoBehaviour {

		private const float ClosedXPos = 425f;
		private const float OpenXPos = 0;

		public static InventoryGUI Instance;
		public bool IsOpened;

		private RectTransform rootTransform;
		public Transform itemsRoot;
		public GameObject InventoryItemPrefab;

		private Action<WeaponStruct> Callback;

		void Awake ()
        {
			Instance = this;
			IsOpened = false;
			//rootTransform = GetComponent<RectTransform> ();
			//rootTransform.localPosition = new Vector3 (ClosedXPos, 0, 0);
            gameObject.SetActive(IsOpened);
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
            gameObject.SetActive(IsOpened);
			//rootTransform.localPosition = new Vector3 (OpenXPos, 0, 0);

		}

		public void Close ()
        {
            Cursor.visible = true;
			IsOpened = false;
			//rootTransform.localPosition = new Vector3 (ClosedXPos, 0, 0);
            gameObject.SetActive(IsOpened);
		}

		private void OnInventoryItemClick (WeaponStruct item)
		{
		    Close();
			Callback (item);
		}
	}

}
