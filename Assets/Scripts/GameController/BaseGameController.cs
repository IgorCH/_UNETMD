using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

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
 * Кампании играешь одной командой.
 * Все остальное другой
 * 
 * 
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
 * Надо уметь создавать события
 *  
 *  
 * Логика
 * CharacterLobbyHook.OnLobbyServer SceneLoadedForPlayer
 * событие после загрузки карты у игрока. В этот момент создается персонаж GameManager.AddTank
 *  
 */

namespace MostDanger {
	
	public class BaseGameController : NetworkBehaviour {

		static public BaseGameController Instance;
		static public List<CharacterManager> Characters = new List<CharacterManager> ();
		public GameObject CharacterPrefab;

		[HideInInspector]
		[SyncVar]
		public bool IsGameFinished = false;

		void Awake()
		{
			Instance = this;
		}
			
		void Start ()
		{
		
		}

		void Update ()
		{
		
		}
	}

}
