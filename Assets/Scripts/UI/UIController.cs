using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

	[Header("Dialogue UI")]
	public float letterPause = 0.1f;
	public float textboxspeed;
	public GameObject m_dialogueBox;
	public GameObject m_dialogueText;
	public TextAsset dialogueJSON;
	public AudioClip typeSound1;
	public AudioClip typeSound2;
	public enum Expression { Neutral, Happy, Sad, Angry, Surprised };

	private bool isShowingDialogueBox = false;
	private bool isMovingDialogueBox = false;
	private bool isTypingDialogue = false;
	private bool isCurrentDialogueCollectionCompleted = true;

	private DialogueCollection currentDialogCollection;
	private Dictionary<string,DialogueCollection> dialogueCatalog = new Dictionary<string,DialogueCollection>();


	void Awake()
	{
		getDialogueData ();
		currentDialogCollection = dialogueCatalog ["Event1"];
	}

	//TODO: This function should not be in the UIController
	void getDialogueData() {
		var dialogueCatalogData = JSON.Parse(dialogueJSON.ToString());
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


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{	
			if (!isMovingDialogueBox && !isTypingDialogue && isCurrentDialogueCollectionCompleted)
			{
				MoveDialogueBox ();
			}

			if (isShowingDialogueBox)
			{
				if (isTypingDialogue) 
				{
					FastCompleteDialogue ();
				}
				else 
				{
					ShowNextDialogueInCurrentCollection ();
					if (isCurrentDialogueCollectionCompleted)
						MoveDialogueBox ();
				}
			} 

		}
	}


	void MoveDialogueBox() {
		if (isShowingDialogueBox) //hide dialogue box
		{
			Vector3 targetPos = m_dialogueBox.transform.position - new Vector3 (0, 3, 0);
			StartCoroutine(MoveDialogueBox(targetPos));
			isShowingDialogueBox = false;
			m_dialogueText.GetComponent<Text>().text = "";
			isCurrentDialogueCollectionCompleted = true;
		} 
		else //show dialogue box
		{
			Vector3 targetPos = m_dialogueBox.transform.position + new Vector3 (0, 3, 0);
			StartCoroutine(MoveDialogueBox(targetPos));
			isShowingDialogueBox = true;
			isCurrentDialogueCollectionCompleted = false;
		}	
		Debug.Log (isCurrentDialogueCollectionCompleted);
	}

		
	void ShowNextDialogueInCurrentCollection()
	{
		m_dialogueText.GetComponent<Text>().text = "";
		int currentIndex = currentDialogCollection.CurrentIndex;
		List<Dialogue> dialogueList = currentDialogCollection.DialogueList;
		if (currentIndex < dialogueList.Count) 
		{
			string message = dialogueList [currentIndex].Text;
			StartCoroutine (TypeText (message));
		} 
		else 
		{
			isCurrentDialogueCollectionCompleted = true;
		}
	}


	void FastCompleteDialogue()
	{
		int currentIndex = currentDialogCollection.CurrentIndex;
		List<Dialogue> dialogueList = currentDialogCollection.DialogueList;
		string message = dialogueList[currentIndex].Text;
		if(currentIndex <= dialogueList.Count) currentDialogCollection.CurrentIndex += 1;
		m_dialogueText.GetComponent<Text> ().text = message;
		isTypingDialogue = false;
	}


	IEnumerator TypeText(string message)
	{
		isTypingDialogue = true;
		foreach (char letter in message.ToCharArray())
		{
			m_dialogueText.GetComponent<Text>().text += letter;
			yield return new WaitForSeconds(letterPause);
			if (!isTypingDialogue) yield break;
		}
		int currentIndex = currentDialogCollection.CurrentIndex;
		List<Dialogue> dialogueList = currentDialogCollection.DialogueList;
		if(currentIndex <= dialogueList.Count) currentDialogCollection.CurrentIndex += 1;
		isTypingDialogue = false;
	}
		

	IEnumerator MoveDialogueBox(Vector3 target)
	{
		isMovingDialogueBox = true;
		float step = textboxspeed * Time.fixedDeltaTime;
		while (Vector3.Distance(target, m_dialogueBox.transform.position) > 0.1f)
		{
			m_dialogueBox.transform.position = Vector3.Lerp (m_dialogueBox.transform.position, target, step);
			yield return new WaitForFixedUpdate();
		}
		isMovingDialogueBox = false;
	}

}
