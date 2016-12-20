using System.Collections.Generic;

public class Dialogue
{
	private string text;
	private int expression;

	public Dialogue(string text, int expression)
	{
		this.text = text;
		this.expression = expression;
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
