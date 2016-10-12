 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;
 
 public class TextTyper : MonoBehaviour {
 
    public float letterPause = 0.1f;
    public AudioClip typeSound1;
    public AudioClip typeSound2;
 
    string message;
    private Text textComp;
    private int repeat = 0;

    // Use this for initialization
    void OnEnable()
    {
        textComp = GetComponent<Text>();
        switch (repeat)
        {
            case 0:
                message = "... ...";
                break;
            case 1:
                message = "I don't want to deal with this right now.";
                break;
            default:
                message = "No!";
                break;
        }
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