/*
 * Single
 * 	Campaign
 *   Gnomes
 * 		New
 * 		Resume
 *   Goblins
 * 		New
 * 		Resume
 *  Scenary
 * 		List
 *      (Карта -> Кол-во Команд -> Выбор стороны -> Заполнение остальных сторон ботами -> Ready(Ник Команда Цвет))
 * 		Бот слабый средний сильный
 * 		PlayBtn
 * 
 * 
 * Multiplayer
 * (Карта -> Кол-во Команд -> Выбор стороны -> Заполнение остальных сторон ботами -> Ready(Ник Команда Цвет))
 * 	LAN
 *  Internet
 * 
 * MyTeam
 * Options
 * Quit
 * 
 * Динамическая загрузка карты и контроллера
 * 
 * Добавление ботов в лобби
 * 
 * Кампании играешь одной командой.
 * Все остальное другой
 *  
 * Загрузка выбранной карты на старте
 * Сформировать список SpawnPoint и другие параметры карты
 * 	
 * Создание различных PlayerPrefab для выбранного игрока
 *   
 * Нужно ввести понятие команды
 * Нужно уметь получить победителя
 *  
 * При создании игры выбираем карту
 * Выбираем команды
 *  
 * Надо уметь слушать конец хода (по времени или по атаке)
 * Надо уметь слушать смерти и подводить итоги
 *  
 * Надо уметь создавать игровые события
 *  
 * Логика
 * CharacterLobbyHook.OnLobbyServer SceneLoadedForPlayer
 * событие после загрузки карты у игрока. В этот момент создается персонаж GameManager.AddTank
 *
 * План
 * 1. наладить инвентарь, сделать четким
 * 2. потом добавить динамит, гранату, винтовку персонажу
 * 3. потом джет пак и парашут
 * 4. hp, reborn
 * 5. наладить систему выделения техники (выделять допустим стрелочкой сверху)
 * 6. потом можно и танк добавить
 * 7. синхронизация всех машин
 * 8. потом горячие клавиши в инвентаре
 * 9. потом туррели с AI
 * 10. собрать полноценную карту
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace MostDanger {

	public class GameManager : NetworkBehaviour
	{
	    static public GameManager Instance;

	    //this is static so tank can be added even withotu the scene loaded (i.e. from lobby)
		static public List<CharacterManager> Characters = new List<CharacterManager>();             // A collection of managers for enabling and disabling different aspects of the tanks.

	    public int NumRoundsToWin = 5;          // The number of rounds a single player has to win to win the game.
	    public float StartDelay = 3f;           // The delay between the start of RoundStarting and RoundPlaying phases.
	    public float EndDelay = 3f;             // The delay between the end of RoundPlaying and RoundEnding phases.
	    public Text m_MessageText;              // Reference to the overlay Text to display winning text, etc.
	    
		//public GameObject GnomePrefab;        // Reference to the prefab the players will control.
		//public GameObject GoblinPrefab;       // Reference to the prefab the players will control.

		public GameObject CartoonLevel;
		public GameObject FootholdLevel;

	    private GameObject[] SpawnPoints;

	    [HideInInspector]
	    [SyncVar]
	    public bool _isGameFinished = false;

	    [Space]
	    [Header("UI")]
	    public CanvasGroup m_FadingScreen;  
	    public CanvasGroup m_EndRoundScreen;

	    private int _roundNumber;                  // Which round the game is currently on.
		private WaitForSeconds _startWait;         // Used to have a delay whilst the round starts.
		private WaitForSeconds _endWait;           // Used to have a delay whilst the round or game ends.
		private CharacterManager _roundWinner;     // Reference to the winner of the current round.  Used to make an announcement of who won.
		private CharacterManager _gameWinner;      // Reference to the winner of the game.  Used to make an announcement of who won.

	    void Awake()
	    {
	        Instance = this;
	    }

	    [ServerCallback]
	    private void Start()
	    {
	        // Create the delays so they only have to be made once.
	        _startWait = new WaitForSeconds(StartDelay);
	        _endWait = new WaitForSeconds(EndDelay);

			//TODO ALARM
			var level = Instantiate (FootholdLevel);
			NetworkServer.Spawn (level);
			//SpawnPoints = 
	
			RpcPrepareClients ();

	        // Once the tanks have been created and the camera is using them as targets, start the game.
	        StartCoroutine(GameLoop());
	    }

		[ClientRpc]
		public void RpcPrepareClients() 
		{
			//Debug.Log(GameObject.FindGameObjectsWithTag("SpawnPoint"));
			SpawnPoints = GameObject.FindGameObjectsWithTag ("SpawnPoint");
		}

	    /// <summary>
	    /// Add a character from the lobby hook
	    /// </summary>
	    /// <param name="tank">The actual GameObject instantiated by the lobby, which is a NetworkBehaviour</param>
	    /// <param name="playerNum">The number of the player (based on their slot position in the lobby)</param>
	    /// <param name="c">The color of the player, choosen in the lobby</param>
	    /// <param name="name">The name of the Player, choosen in the lobby</param>
	    static public void AddCharacter(GameObject character, int playerNum, Color c, string name)
	    {
			CharacterManager tmp = new CharacterManager();
	        tmp.Instance = character;
	        tmp.PlayerNumber = playerNum;
	        tmp.PlayerColor = c;
	        tmp.PlayerName = name;
	        tmp.Setup();

	        Characters.Add(tmp);
	    }

		static public void AddBot()
		{
			CharacterManager tmp = new CharacterManager();

			//GameObject создаем здесь
			//tmp.Instance = character;

			//номер выдаем пока рандомный
			//tmp.PlayerNumber = playerNum;

			//цвет выдаем пока рандомный
			//tmp.PlayerColor = c;

			tmp.PlayerName = "Bot";
			tmp.Setup();

			Characters.Add(tmp);
		}

	    public void RemoveCharacter(GameObject character)
	    {
			CharacterManager toRemove = null;
	        foreach (var tmp in Characters)
	        {
	            if (tmp.Instance == character)
	            {
	                toRemove = tmp;
	                break;
	            }
	        }

	        if (toRemove != null)
	            Characters.Remove(toRemove);
	    }

	    // This is called from start and will run each phase of the game one after another. ONLY ON SERVER (as Start is only called on server)
	    private IEnumerator GameLoop()
	    {
			//TODO Более продвинутый поиск конца игры
	        //while (m_Tanks.Count < 2)
	        //    yield return null;

	        //wait to be sure that all are ready to start
	        yield return new WaitForSeconds(2.0f);

	        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
	        yield return StartCoroutine(RoundStarting());

	        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
	        yield return StartCoroutine(RoundPlaying());

	        // Once execution has returned here, run the 'RoundEnding' coroutine.
	        yield return StartCoroutine(RoundEnding());

	        // This code is not run until 'RoundEnding' has finished.  At which point, check if there is a winner of the game.
	        if (_gameWinner != null)
	        {
				// If there is a game winner, wait for certain amount or all player confirmed to start a game again
	            _isGameFinished = true;
	            float leftWaitTime = 15.0f;
	            bool allAreReady = false;
	            int flooredWaitTime = 15;

	            while (leftWaitTime > 0.0f && !allAreReady)
	            {
	                yield return null;

	                allAreReady = true;
	                foreach (var tmp in Characters)
	                {
	                    allAreReady &= tmp.IsReady();
	                }

	                leftWaitTime -= Time.deltaTime;

	                int newFlooredWaitTime = Mathf.FloorToInt(leftWaitTime);

	                if (newFlooredWaitTime != flooredWaitTime)
	                {
	                    flooredWaitTime = newFlooredWaitTime;
	                    string message = EndMessage(flooredWaitTime);
	                    RpcUpdateMessage(message);
	                }
	            }

	            LobbyManager.Singleton.ServerReturnToLobby();
	        }
	        else
	        {
	            // If there isn't a winner yet, restart this coroutine so the loop continues.
	            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
	            StartCoroutine(GameLoop());
	        }
	    }


	    private IEnumerator RoundStarting()
	    {
	        //we notify all clients that the round is starting
	        RpcRoundStarting();

	        // Wait for the specified length of time until yielding control back to the game loop.
	        yield return _startWait;
	    }

	    [ClientRpc]
	    void RpcRoundStarting()
	    {
	        ResetAllCharacters();
			SetControlEnabled(false);
			_roundNumber++;
	        m_MessageText.text = "ROUND " + _roundNumber;

	        StartCoroutine(ClientRoundStartingFade());
	    }

	    private IEnumerator ClientRoundStartingFade()
	    {
	        float elapsedTime = 0.0f;
	        float wait = StartDelay - 0.5f;

	        yield return null;

	        while (elapsedTime < wait)
	        {
	            if(_roundNumber == 1)
	                m_FadingScreen.alpha = 1.0f - (elapsedTime / wait);
	            else
	                m_EndRoundScreen.alpha = 1.0f - (elapsedTime / wait);

	            elapsedTime += Time.deltaTime;

	            //sometime, synchronization lag behind because of packet drop, so we make sure our tank are reseted
	            if (elapsedTime / wait < 0.5f)
	                ResetAllCharacters();

	            yield return null;
	        }
	    }

	    private IEnumerator RoundPlaying()
	    {
	        //notify clients that the round is now started, they should allow player to move.
	        RpcRoundPlaying();
            
	        // While there is not one tank left...
	        while (true/*!OneTankLeft()*/)
	        {
	            // ... return on the next frame.
	            yield return null;
	        }
	    }

	    [ClientRpc]
	    void RpcRoundPlaying()
	    {
	        // As soon as the round begins playing let the players control the tanks.
			SetControlEnabled(true);

	        // Clear the text from the screen.
	        m_MessageText.text = string.Empty;
	    }

	    private IEnumerator RoundEnding()
	    {
	        // Clear the winner from the previous round.
	        _roundWinner = null;

	        // See if there is a winner now the round is over.
	        _roundWinner = GetRoundWinner();

	        // If there is a winner, increment their score.
	        if (_roundWinner != null)
	            _roundWinner.m_Wins++;

	        // Now the winner's score has been incremented, see if someone has one the game.
	        _gameWinner = GetGameWinner();

	        RpcUpdateMessage(EndMessage(0));

	        //notify client they should disable tank control
	        RpcRoundEnding();

	        // Wait for the specified length of time until yielding control back to the game loop.
	        yield return _endWait;
	    }

	    [ClientRpc]
	    private void RpcRoundEnding()
	    {
			SetControlEnabled(false);
	        StartCoroutine(ClientRoundEndingFade());
	    }

	    [ClientRpc]
	    private void RpcUpdateMessage(string msg)
	    {
	        m_MessageText.text = msg;
	    }

	    private IEnumerator ClientRoundEndingFade()
	    {
	        float elapsedTime = 0.0f;
	        float wait = EndDelay;
	        while (elapsedTime < wait)
	        {
	            m_EndRoundScreen.alpha = (elapsedTime / wait);

	            elapsedTime += Time.deltaTime;
	            yield return null;
	        }
	    }

	    // This is used to check if there is one or fewer tanks remaining and thus the round should end.
	    private bool OneCharacterLeft()
	    {
	        // Start the count of tanks left at zero.
			int numCharactersLeft = 0;

	        // Go through all the tanks...
	        for (int i = 0; i < Characters.Count; i++)
	        {
	            // ... and if they are active, increment the counter.
	            if (Characters[i].renderers.activeSelf)
	                numCharactersLeft++;
	        }

	        // If there are one or fewer tanks remaining return true, otherwise return false.
	        return numCharactersLeft <= 1;
	    }

	    // This function is to find out if there is a winner of the round.
	    // This function is called with the assumption that 1 or fewer tanks are currently active.
		private CharacterManager GetRoundWinner()
	    {
	        // Go through all the tanks...
	        for (int i = 0; i < Characters.Count; i++)
	        {
	            // ... and if one of them is active, it is the winner so return it.
	            if (Characters[i].renderers.activeSelf)
	                return Characters[i];
	        }

	        // If none of the tanks are active it is a draw so return null.
	        return null;
	    }

	    // This function is to find out if there is a winner of the game.
		private CharacterManager GetGameWinner()
	    {

	        // Go through all the tanks...
	        for (int i = 0; i < Characters.Count; i++)
	        {
				if (Characters[i].m_Wins == NumRoundsToWin)
					return Characters[i];
	        }

	        // If no tanks have enough rounds to win, return null.
	        return null;
	    }
			
	    // Returns a string of each player's score in their tank's color.
	    private string EndMessage(int waitTime)
	    {
	        // By default, there is no winner of the round so it's a draw.
	        string message = "DRAW!";

	        // If there is a game winner set the message to say which player has won the game.
	        if (_gameWinner != null)
	            message = "<color=#" + ColorUtility.ToHtmlStringRGB(_gameWinner.PlayerColor) + ">"+ _gameWinner.PlayerName + "</color> WINS THE GAME!";
	        // If there is a winner, change the message to display 'PLAYER #' in their color and a winning message.
	        else if (_roundWinner != null)
	            message = "<color=#" + ColorUtility.ToHtmlStringRGB(_roundWinner.PlayerColor) + ">" + _roundWinner.PlayerName + "</color> WINS THE ROUND!";

	        // After either the message of a draw or a winner, add some space before the leader board.
	        message += "\n\n";

	        // Go through all the tanks and display their scores with their 'PLAYER #' in their color.
	        for (int i = 0; i < Characters.Count; i++)
	        {
	            message += "<color=#" + ColorUtility.ToHtmlStringRGB(Characters[i].PlayerColor) + ">" + Characters[i].PlayerName + "</color>: " + Characters[i].m_Wins + " WINS " 
	                + (Characters[i].IsReady()? "<size=15>READY</size>" : "") + " \n";
	        }

	        if (_gameWinner != null)
	            message += "\n\n<size=20 > Return to lobby in " + waitTime + "\nPress Fire to get ready</size>";

	        return message;
	    }
			
	    // This function is used to turn all the tanks back on and reset their positions and properties.
	    private void ResetAllCharacters()
	    {
	        for (int i = 0; i < Characters.Count; i++)
	        {
				Characters[i].spawnPoint = SpawnPoints[Characters[i].characterSetup.PlayerNumber].GetComponent<Transform>();
	            Characters[i].Reset();
	        }
	    }

		private void SetControlEnabled(bool value)
	    {
	        for (int i = 0; i < Characters.Count; i++)
	        {
	            Characters[i].SetControlEnabled(value);
	        }
	    }

	}

}