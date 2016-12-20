using System.Collections;
using System.Collections.Generic;

public class DialogueCollection {

	private List<Dialogue> dialogueList = new List<Dialogue>();
	private string eventName;
	private int currentIndex;

	public DialogueCollection (string eventName, List<Dialogue> dialogueList)
	{
		this.eventName = eventName;
		this.dialogueList = dialogueList;
		this.currentIndex = 0;
	}

	public DialogueCollection ()
	{
		this.eventName = null;
		this.dialogueList = null;
		this.currentIndex = 0;
	}

	public List<Dialogue> DialogueList
	{
		get { return this.dialogueList; }
		set { this.dialogueList = value; }
	}

	public string EventName
	{
		get { return this.eventName; }
		set { this.eventName = value; }
	}

	public int CurrentIndex
	{
		get { return this.currentIndex; }
		set { this.currentIndex = value; }
	}
}
