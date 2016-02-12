using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MostDanger {
		
	public class ActionsGUI : MonoBehaviour {

        public static ActionsGUI Instance;

		private Action<WeaponStruct> Callback;

		void Awake ()
        {
			Instance = this;

		}

	}

}
