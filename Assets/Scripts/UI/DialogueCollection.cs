using System.Collections;
using System.Collections.Generic;

public class DialogueCollection {

	private List<Dialogue> dialogueList = new List<Dialogue>();
	private string name;
	private int currentIndex;

	public DialogueCollection (List<Dialogue> dialogueList, string name)
	{
		this.name = name;
		this.dialogueList = dialogueList;
		this.currentIndex = 0;
	}

	public DialogueCollection ()
	{
		this.name = null;
		this.dialogueList = null;
		this.currentIndex = 0;
	}

	public List<Dialogue> DialogueList
	{
		get { return this.dialogueList; }
		set { this.dialogueList = value; }
	}

	public string Name
	{
		get { return this.name; }
		set { this.name = value; }
	}

	public int CurrentIndex
	{
		get { return this.currentIndex; }
		set { this.currentIndex = value; }
	}
}
