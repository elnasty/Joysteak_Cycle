using SimpleJSON;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static GameManager _instance;

	public bool isMouseControl = false;
	public Dictionary<string,DialogueCollection> dialogueCatalog = new Dictionary<string,DialogueCollection>();

	public static GameManager instance 
	{
		get 
		{
			if (_instance == null) 
			{
				GameObject manager= new GameObject("GameManager");
				_instance = manager.AddComponent<GameManager>();
				DontDestroyOnLoad(manager);
			}
			return _instance;
		}
	}

	public void GetDialogueData(string jsonData) {
		var dialogueCatalogData = JSON.Parse(jsonData);
		foreach (string eventName in dialogueCatalogData.Keys)
		{
			var dialogueLinesData = dialogueCatalogData[eventName]["dialogueLines"];
			List<Dialogue> dialogueLines = new List<Dialogue> ();
			for (int i = 0; i < dialogueLinesData.Count; i++)
			{
				var dialogueData = dialogueLinesData [i];
				string character = dialogueData ["character"].Value;
				string text 	 = dialogueData ["dialogue"].Value;
				int expression 	 = dialogueData ["expression"].AsInt;
				dialogueLines.Add (new Dialogue (character, text, expression));
			}
			DialogueCollection dialogueCollection = new DialogueCollection (eventName, dialogueLines);
			dialogueCatalog.Add (eventName, dialogueCollection);
		}
	}
}
