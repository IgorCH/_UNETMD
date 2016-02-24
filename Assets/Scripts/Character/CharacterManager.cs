using System;
using UnityEngine;

namespace MostDanger {

	[Serializable]
	public class CharacterManager
	{
	    // This class is to manage various settings on a tank.
	    // It works with the GameManager class to control how the tanks behave
	    // and whether or not players have control of their tank in the 
	    // different phases of the game.

	    public Color PlayerColor;               // This is the color this tank will be tinted.
	    public Transform spawnPoint;            // The position and direction the tank will have when it spawns.

	    [HideInInspector]
	    public GameObject Instance;             // A reference to the instance of the tank when it is created.
	    
		[HideInInspector]
	    public GameObject renderers;        // The transform that is a parent of all the tank's renderers.  This is deactivated when the tank is dead.
	    
		[HideInInspector]
	    public int m_Wins;                        // The number of wins this player has so far.
	    
		[HideInInspector]
	    public string PlayerName;               // The player name set in the lobby

		[HideInInspector]
		public int PlayerNumber;                // This specifies which player this the manager for.

		public CharacterController characterController;      // References to various objects for control during the different game phases.
		public CharacterHealth characterHealth;
		public CharacterSetup characterSetup;

	    public void Setup()
		{
			characterController = Instance.GetComponent<CharacterController>();
			characterHealth = Instance.GetComponent<CharacterHealth>();
			characterSetup = Instance.GetComponent<CharacterSetup>();

	        renderers = characterHealth.renderers;

	        //Set a reference to that manаger in the health script, to disable control when dying
	        characterHealth.Manager = this;

	        // Set the player numbers to be consistent across the scripts.
			characterController.PlayerNumber = PlayerNumber;

	        //setup is use for diverse Network Related sync
	        characterSetup.Color = PlayerColor;
	        characterSetup.PlayerName = PlayerName;

	        characterSetup.PlayerNumber = PlayerNumber;
	    }

	    // Used during the phases of the game where the player shouldn't be able to control their tank.
	    public void DisableControl()
	    {
			characterController.enabled = false;
	    }

	    // Used during the phases of the game where the player should be able to control their tank.
	    public void EnableControl()
	    {
			characterController.enabled = true;
			characterController.ReEnableParticles();
	    }

	    public string GetName()
	    {
	        return characterSetup.PlayerName;
	    }

	    public bool IsReady()
	    {
	        return characterSetup.IsReady;
	    }

	    // Used at the start of each round to put the tank into it's default state.
	    public void Reset()
	    {
			characterController.SetDefaults();
	        characterHealth.SetDefaults();

			if (characterController.hasAuthority)
	        {
				characterController.Rigidbody.position = spawnPoint.position;
				characterController.Rigidbody.rotation = spawnPoint.rotation;
	        }
	    }
	}

}