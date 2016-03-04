using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Profile {

	public static Profile Instance;

	public int Money;
	public List<ProfileCharacter> MyTeam;

	public void Init ()
	{
		Instance = this;

		MyTeam = new List<ProfileCharacter> ();

		//TODO
		//Грузим данные о команде и тп из UserDefaults
		//Грузим данные о команде и тп с Сервера
	}
		
}

public class ProfileCharacter {

	public int Level;

}
