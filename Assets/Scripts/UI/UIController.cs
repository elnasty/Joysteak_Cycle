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

	private bool isShowingDialogueBox;
	private bool isMovingDialogueBox;
	private bool isTypingDialogue;

	private DialogueCollection dialogueCollection;


	void Awake()
	{
		isShowingDialogueBox = false;
		string dialogueName = "Test";
		List<Dialogue> dialogueList = new List<Dialogue> ();
		dialogueList.Add (new Dialogue("I wonder what Elliot would say...", 0));
		dialogueList.Add (new Dialogue("I don't want to deal with this right now.", 2));
		dialogueList.Add (new Dialogue("No!", 3));
		dialogueCollection = new DialogueCollection (dialogueList, dialogueName);
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (!isMovingDialogueBox && !isTypingDialogue)
				MoveDialogueBox ();
			
			if (isShowingDialogueBox) 
			{
				if (isTypingDialogue)
					FastCompleteDialogue ();
				else
					ShowNextDialogue ();
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
		} 
		else //show dialogue box
		{
			Vector3 targetPos = m_dialogueBox.transform.position + new Vector3 (0, 3, 0);
			StartCoroutine(MoveDialogueBox(targetPos));
			isShowingDialogueBox = true;
		}	
	}

		
	void ShowNextDialogue()
	{
		m_dialogueText.GetComponent<Text>().text = "";
		int currentIndex = dialogueCollection.CurrentIndex;
		List<Dialogue> dialogueList = dialogueCollection.DialogueList;
		string message = dialogueList[currentIndex].Text;
		StartCoroutine(TypeText(message));
	}


	void FastCompleteDialogue()
	{
		int currentIndex = dialogueCollection.CurrentIndex;
		List<Dialogue> dialogueList = dialogueCollection.DialogueList;
		string message = dialogueList[currentIndex].Text;
		if(currentIndex <= dialogueList.Count) dialogueCollection.CurrentIndex += 1;
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
		int currentIndex = dialogueCollection.CurrentIndex;
		List<Dialogue> dialogueList = dialogueCollection.DialogueList;
		if(currentIndex <= dialogueList.Count) dialogueCollection.CurrentIndex += 1;
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
