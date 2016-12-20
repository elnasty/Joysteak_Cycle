using System.Collections.Generic;

public class Dialogue
{
	private string charName;
	private string text;
	private int expression;

	public Dialogue(string charName, string text, int expression)
	{
		this.charName = charName;
		this.text = text;
		this.expression = expression;
	}

	public Dialogue()
	{
		this.charName = null;
		this.text = null;
		this.expression = -1;
	}

	public string CharacterName
	{
		get { return this.charName; }
		set { this.charName = value; }
	}

	public string Text
	{
		get { return this.text; }
		set { this.text = value; }
	}

	public int Expression
	{
		get { return this.expression; }
		set { this.expression = value; }
	}
}
