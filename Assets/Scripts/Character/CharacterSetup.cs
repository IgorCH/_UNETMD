using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MostDanger {
	
	//Purpose of that class is syncing data between server - client
	public class CharacterSetup : NetworkBehaviour 
	{

	    [Header("Network")]
	    
	    [SyncVar]
	    public Color Color;

	    [SyncVar]
	    public string PlayerName;

	    //this is the player number in all of the players
	    [SyncVar]
	    public int PlayerNumber;

	    [SyncVar]
	    public bool IsReady = false;

	    public override void OnStartClient()
	    {
	        base.OnStartClient();

	        if (!isServer) //if not hosting, we had the tank to the gamemanger for easy access!
	            GameManager.AddCharacter(gameObject, PlayerNumber, Color, PlayerName);

	        GameObject Renderers = transform.Find("Renderers").gameObject;

	        // Get all of the renderers of the tank.
	        Renderer[] renderers = Renderers.GetComponentsInChildren<Renderer>();

	        // Go through all the renderers...
	        for (int i = 0; i < renderers.Length; i++)
	        {
	            // ... set their material color to the color specific to this tank.
	            //TODOrenderers[i].material.color = Color;
	        }

	        if (Renderers)
	            Renderers.SetActive(false);

	       
			gameObject.name = PlayerName;
	    }

	    [ClientCallback]
	    public void Update()
	    {
	        if(!isLocalPlayer)
	        {
	            return;
	        }

	        /*if (GameManager.s_Instance.m_GameIsFinished && !m_IsReady)
	        {
	            if(Input.GetButtonDown("Fire"+(m_LocalID + 1)))
	            {
	                CmdSetReady();
	            }
	        }*/
	    }

	    [Command]
	    public void CmdSetReady()
	    {
	        IsReady = true;
	    }

	    public override void OnNetworkDestroy()
	    {
	        GameManager.Instance.RemoveCharacter(gameObject);
	    }
	}

}