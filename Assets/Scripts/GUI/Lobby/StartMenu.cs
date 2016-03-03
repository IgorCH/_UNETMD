using UnityEngine;
using System.Collections;

namespace MostDanger {
	
	public class StartMenu : MonoBehaviour {

		public LobbyManager lobbyManager;
		public RectTransform MultiplayerPanel;

		public void OnCampaignButton () 
		{
			
		}

		public void OnScenaryButton () 
		{

		}

		public void OnMultiplayerButton () 
		{
			lobbyManager.ChangeTo (MultiplayerPanel);
		}

		public void OnProfileButton () 
		{

		}

		public void OnSettingsButton () 
		{

		}

		public void OnExitButton () 
		{
			Application.Quit ();
		}
	}

}