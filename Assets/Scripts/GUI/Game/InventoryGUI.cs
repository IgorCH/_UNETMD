using UnityEngine;
using System.Collections;

namespace MostDanger {
		
	public class InventoryGUI : MonoBehaviour {

		public static InventoryGUI Instance;

		void Awake () {
			Instance = this;
		}

		void Start () {
		
		}

		void Update () {
		
		}

		public void Open (Inventory inventory) {
			
			foreach(var item in inventory.Weapons) {
				//формируем гуи	
			}
		}
	}

}
