using UnityEngine;
using System.Collections;

namespace MostDanger {
	
	public class StartMenu : MonoBehaviour {

		public LobbyManager lobbyManager;
		public RectTransform CampaignPanel;
		public RectTransform ScenaryPanel;
		public RectTransform MultiplayerPanel;

		public void OnCampaignButton () 
		{
			lobbyManager.ChangeTo (CampaignPanel);
		}

		public void OnScenaryButton () 
		{
			lobbyManager.ChangeTo (ScenaryPanel);
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