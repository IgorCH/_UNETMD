using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace MostDanger
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	public class NetworkManagerHUD : MonoBehaviour
	{
		public NetworkManager manager;
		[SerializeField] public bool showGUI = true;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;

		// Runtime variable
		bool _showServer = false;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
            //Debug.Log("*-------------*");
            //Debug.Log(NetworkClient.active);
            //Debug.Log(ClientScene.ready);

			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		void OnGUI()
		{
			if (!showGUI)
				return;

			int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;
			int spacing = 24;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
				{
					manager.StartHost();
				}
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
				{
					manager.StartClient();
				}
				manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
				{
					manager.StartServer();
				}
				ypos += spacing;
			}
			else
			{
				if (NetworkServer.active)
				{
					GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
					ypos += spacing;
				}
				if (NetworkClient.active)
				{
					GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
					ypos += spacing;
				}
			}



			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
				{
					ClientScene.Ready(manager.client.connection);
				
					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += spacing;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
				{
					manager.StopHost();
				}
				ypos += spacing;
			}

			if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null)
				{
					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
					{
						manager.StartMatchMaker();
					}
					ypos += spacing;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
							{
								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
							}
							ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
							manager.matchName = GUI.TextField(new Rect(xpos+100, ypos, 100, 20), manager.matchName);
							ypos += spacing;

							ypos += 10;

							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
							{
								manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);
							}
							ypos += spacing;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
								{
									manager.matchName = match.name;
									manager.matchSize = (uint)match.currentSize;
									manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
								}
								ypos += spacing;
							}
						}
					}

					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
					{
						_showServer = !_showServer;
					}
					if (_showServer)
					{
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
						{
							manager.SetMatchHost("localhost", 1337, false);
							_showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
						{
							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
							_showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
						{
							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
							_showServer = false;
						}
					}

					ypos += spacing;

					GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
					ypos += spacing;

					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
					{
						manager.StopMatchMaker();
					}
					ypos += spacing;
				}
			}
		}

        /////////////////////////////////
        //Functions invoked on the Server/Host:
        /////////////////////////////////

        // called when a client connects 
	    public virtual void OnServerConnect(NetworkConnection conn)
	    {
            Debug.Log("OnServerConnect");
	    }

	    // called when a client disconnects
        public virtual void OnServerDisconnect(NetworkConnection conn)
        {
            Debug.Log("OnServerDisconnect");
            //NetworkServer.DestroyPlayersForConnection(conn);
        }

        // called when a client is ready
        public virtual void OnServerReady(NetworkConnection conn)
        {
            Debug.Log("OnServerReady");
            //NetworkServer.SetClientReady(conn);
        }

        // called when a new player is added for a client
        public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            Debug.Log("OnServerAddPlayer");
            //var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
            //NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        // called when a player is removed for a client
        public virtual void OnServerRemovePlayer(NetworkConnection conn, short playerControllerId)
        {
            Debug.Log("OnServerRemovePlayer");
            //PlayerController player;
            //if (conn.GetPlayer(playerControllerId, out player))
            //{
            //    if (player.NetworkIdentity != null && player.NetworkIdentity.gameObject != null)
            //        NetworkServer.Destroy(player.NetworkIdentity.gameObject);
            //}
        }

        // called when a network error occurs
	    public virtual void OnServerError(NetworkConnection conn, int errorCode)
	    {
            Debug.Log("OnServerError");
	    }

        /////////////////////////////////
        //Functions invoked on the Client:
        /////////////////////////////////
    
        // called when connected to a server
        public virtual void OnClientConnect(NetworkConnection conn)
        {
            Debug.Log("OnClientConnect");
            //ClientScene.Ready(conn);
            //ClientScene.AddPlayer(0);
        }

        // called when disconnected from a server
        public virtual void OnClientDisconnect(NetworkConnection conn)
        {
            Debug.Log("OnClientDisconnect");
            //StopClient();
        }

        // called when a network error occurs
	    public virtual void OnClientError(NetworkConnection conn, int errorCode)
	    {
            Debug.Log("OnClientError");
	    }

        // called when told to be not-ready by a server
	    public virtual void OnClientNotReady(NetworkConnection conn)
	    {
            Debug.Log("OnClientNotReady");
	    }

        ///////////////////////////////////////
	    //Functions invoked for the Matchmaker:
        ///////////////////////////////////////
        // called when a match is created
	    public virtual void OnMatchCreate(CreateMatchResponse matchInfo)
	    {
            Debug.Log("OnMatchCreate");
	    }

        // called when a list of matches is received
	    public virtual void OnMatchList(ListMatchResponse matchList)
	    {
            Debug.Log("OnMatchList");
	    }

        // called when a match is joined
	    public void OnMatchJoined(JoinMatchResponse matchInfo)
	    {
            Debug.Log("OnMatchJoined");
	    }








        /*
	    //This function is called after a new level was loaded.
		public void OnLevelWasLoaded ()
		{
			Debug.Log ("OnLevelWasLoaded");
		}	

		//Used to customize synchronization of variables in a script watched by a network view.
		public void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
			Debug.Log ("OnSerializeNetworkView");
		}

		//Called on the client when you have successfully connected to a server.
		public void OnConnectedToServer	() 
		{
			Debug.Log ("OnConnectedToServer");
		}

		//Called on the client when the connection was lost or you disconnected from the server.
		public void OnDisconnectedFromServer (NetworkDisconnection info) 
		{
			Debug.Log ("OnDisconnectedFromServer");
		}

		//Called on the client when a connection attempt fails for some reason.
		public void OnFailedToConnect (NetworkConnectionError error)
		{
			Debug.Log ("OnFailedToConnect");
		}	

		//Called on clients or servers when there is a problem connecting to the MasterServer.
		public void OnFailedToConnectToMasterServer (NetworkConnectionError info)
		{
			Debug.Log ("OnFailedToConnectToMasterServer");
		}

		//Called on clients or servers when reporting events from the MasterServer.
		public void OnMasterServerEvent (MasterServerEvent msEvent) 
		{
			Debug.Log ("OnMasterServerEvent");
		}	

		//Called on objects which have been network instantiated with Network.Instantiate.
		public void OnNetworkInstantiate (NetworkMessageInfo info)
		{
			Debug.Log ("OnNetworkInstantiate");
		}	

		//Called on the server whenever a new player has successfully connected.
		public void OnPlayerConnected (NetworkPlayer player)
		{
			Debug.Log ("OnPlayerConnected");
		}	

		//Called on the server whenever a player disconnected from the server.
		public void OnPlayerDisconnected (NetworkPlayer player)
		{
			Debug.Log ("OnPlayerDisconnected");
		}				

		//Called on the server whenever a Network.InitializeServer was invoked and has completed.
		public void OnServerInitialized ()
		{
			Debug.Log ("OnServerInitialized");
		}*/
	}

};