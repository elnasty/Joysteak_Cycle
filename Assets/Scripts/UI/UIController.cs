using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

	public static UIController instance;

	public float letterPause = 0.1f;
	public float textboxspeed;
	public GameObject m_dialogueBox;
	public GameObject m_dialogueText;
	public GameObject m_faceBox;
	public AudioClip typeSound1;
	public AudioClip typeSound2;
	public enum Expression { Neutral, Happy, Sad, Angry, Surprised };
	public bool isShowingDialogueBox = false;
	private bool isTypingDialogue = false;
	private bool isMovingDialogueBox = false;
	private bool isCurrentDialogueCollectionCompleted = true;

	private DialogueCollection currentDialogCollection;


	void Awake()
	{
		if (instance == null) instance = this;
	}


	public void ShowNextDialogue(DialogueCollection currentDialogCollection) 
	{
		this.currentDialogCollection = currentDialogCollection;

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
				{
					m_faceBox.SetActive (false);
					MoveDialogueBox ();
				}
			}
		}
		else
		{
			m_faceBox.SetActive (true);
			MoveDialogueBox ();
			ShowNextDialogueInCurrentCollection ();
		}
	}


	void MoveDialogueBox() {
		if (!isMovingDialogueBox)
		{
			if (isShowingDialogueBox) 
			{ //hide dialogue box
				Vector3 targetPos = m_dialogueBox.transform.position - new Vector3 (0, 3, 0);
				StartCoroutine (MoveDialogueBox (targetPos));
				isShowingDialogueBox = false;
				isCurrentDialogueCollectionCompleted = true;
				m_dialogueText.GetComponent<Text> ().text = "";
			}
			else
			{ //show dialogue box
				Vector3 targetPos = m_dialogueBox.transform.position + new Vector3 (0, 3, 0);
				StartCoroutine (MoveDialogueBox (targetPos));
				isShowingDialogueBox = true;
				isCurrentDialogueCollectionCompleted = false;
			}	
		}
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
