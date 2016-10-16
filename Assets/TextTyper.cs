using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TextTyper : MonoBehaviour {

    public enum Expression { Neutral, Happy, Sad, Angry, Surprised };
    [System.Serializable]
    public class DialogueObject
    {
        public class Dialogue
        {
            public string text;
            public int expression;

            public Dialogue(string newText, int newExpression)
            {
                text = newText;
                expression = newExpression;
            }
        }
        public string name;
        public int dialogueState;
        public List<Dialogue> dialogueLines = new List<Dialogue>();
    }
 
    public float letterPause = 0.1f;
    public AudioClip typeSound1;
    public AudioClip typeSound2;
 
    string message;
    private Text textComp;

    DialogueObject Test = new DialogueObject();

    void Awake()
    {
        Test.name = "Example";
        Test.dialogueState = 0;
        Test.dialogueLines.Add(new DialogueObject.Dialogue("It's broken.", 0));
        Test.dialogueLines.Add(new DialogueObject.Dialogue("I don't want to deal with this right now.", 2));
        Test.dialogueLines.Add(new DialogueObject.Dialogue("No!", 3));

        string json = JsonUtility.ToJson(Test);

        Debug.Log(json);
    }

    /*void Awake()
    {
        TextAsset dialogueCatalog = Resources.Load("DialogueCatalog") as TextAsset;
        Test = JsonUtility.FromJson<DialogueObject>(dialogueCatalog.text);
        string json = JsonUtility.ToJson(Test);

        Debug.Log(json);
    }*/

    // Use this for initialization
    void OnEnable()
    {
        textComp = GetComponent<Text>();
        message = Test.dialogueLines[Test.dialogueState].text;
        if(Test.dialogueState <= Test.dialogueLines.Count) Test.dialogueState += 1;
        textComp.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            /*             if (typeSound1 && typeSound2)
                             SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);*/
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
    }
 }