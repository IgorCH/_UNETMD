using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MostDanger {
	
	//Purpose of that class is syncing data between server - client
	public class CharacterSetup : NetworkBehaviour 
	{
	    [Header("UI")]

	    public Text m_NameText;
	    public GameObject m_Crown;

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

	    //This allow to know if the crown must be displayed or not
	    protected bool m_isLeader = false;

	    public override void OnStartClient()
	    {
	        base.OnStartClient();

	        if (!isServer) //if not hosting, we had the tank to the gamemanger for easy access!
	            GameManager.AddTank(gameObject, PlayerNumber, Color, PlayerName);

	        GameObject m_TankRenderers = transform.Find("TankRenderers").gameObject;

	        // Get all of the renderers of the tank.
	        Renderer[] renderers = m_TankRenderers.GetComponentsInChildren<Renderer>();

	        // Go through all the renderers...
	        for (int i = 0; i < renderers.Length; i++)
	        {
	            // ... set their material color to the color specific to this tank.
	            //TODOrenderers[i].material.color = Color;
	        }

	        if (m_TankRenderers)
	            m_TankRenderers.SetActive(false);

	        m_NameText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(Color) + ">"+PlayerName+"</color>";
            
			gameObject.name = m_NameText.text;

	        m_Crown.SetActive(false);
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

	    public void SetLeader(bool leader)
	    {
	        RpcSetLeader(leader);
	    }

	    [ClientRpc]
	    public void RpcSetLeader(bool leader)
	    {
	        m_isLeader = leader;
	    }

	    [Command]
	    public void CmdSetReady()
	    {
	        IsReady = true;
	    }

	    public void ActivateCrown(bool active)
	    {//if we try to show (not hide) the crown, we only show it we are the current leader
	        m_Crown.SetActive(active ? m_isLeader : false);
	        m_NameText.gameObject.SetActive(active);
	    }

	    public override void OnNetworkDestroy()
	    {
	        GameManager.Instance.RemoveTank(gameObject);
	    }
	}

}