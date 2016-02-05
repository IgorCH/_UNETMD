using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MostDanger {

	//delegate bool InventoryCallback(int hWnd, int lParam);
		
	public class InventoryGUI : MonoBehaviour {

		private const float ClosedXPos = 425f;
		private const float OpenXPos = 0;

		public static InventoryGUI Instance;
		public bool IsOpened;

		private RectTransform rootTransform;
		public Transform itemsRoot;
		public GameObject InventoryItemPrefab;

		private Action<WeaponStruct> Callback;

		void Awake () {
			Instance = this;
			IsOpened = false;
			rootTransform = GetComponent<RectTransform> ();
			rootTransform.localPosition = new Vector3 (ClosedXPos, 0, 0);
		}

		void Start () {
		
		}

		void Update () {

		}

		public void Open (List<WeaponStruct> weapons, Action<WeaponStruct> callback) {

			Callback = callback;

			foreach(var item in weapons) {
				var newInventoryItem = Instantiate (InventoryItemPrefab);
				newInventoryItem.name = "item" + UnityEngine.Random.Range (0, 100);
				newInventoryItem.GetComponent<RectTransform> ().parent = itemsRoot;
				newInventoryItem.GetComponent<Button>().onClick.AddListener(() => OnInventoryItemClick(newInventoryItem));
			}

			IsOpened = true;
			rootTransform.localPosition = new Vector3 (OpenXPos, 0, 0);

		}

		public void Close () {

			IsOpened = false;
			rootTransform.localPosition = new Vector3 (ClosedXPos, 0, 0);

		}

		private void OnInventoryItemClick (GameObject item) {
			Debug.Log (item.name);

			Callback (new WeaponStruct(){});
		}
	}

}
