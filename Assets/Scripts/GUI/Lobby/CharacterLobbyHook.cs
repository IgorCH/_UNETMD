using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace MostDanger
{
		
	public class CharacterLobbyHook : LobbyHook 
	{
	    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	    {
	        if (lobbyPlayer == null)
	            return;

	        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();

	        if(lp != null)
	            GameManager.AddCharacter(gamePlayer, lp.slot, lp.playerColor, lp.nameInput.text);
			
	    }

		public override void OnLobbyServerSceneChanged(string sceneName) 
		{
			Debug.Log (sceneName);
			GameManager.AddBot();

		}


	}

}