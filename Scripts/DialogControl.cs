using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour
{
    public GameObject dialogBackground;
    public Text actorName;
    public Text dialogText;

    public float timeBetweenPhases;
    public float typingSpeed;
    public bool isPlaying = false;

    private Coroutine currentDialog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDialog(string actorName, List<string> sentences)
    {
        if(currentDialog != null)
        {
            StopCoroutine(currentDialog);
        }

        currentDialog = StartCoroutine(PlayDialog(actorName, sentences));
    }

    IEnumerator PlayDialog(string actorName, List<string> sentences)
    {
        isPlaying = true;
        dialogBackground.SetActive(true);
        this.actorName.text = actorName;

        foreach(string s in sentences)
        {
            yield return StartCoroutine(TypingSentence(s));
            yield return new WaitForSeconds(timeBetweenPhases);
        }

        dialogBackground.SetActive(false);
        isPlaying = false;
    }

    IEnumerator TypingSentence(string sentence)
    {
        dialogText.text = "";

        foreach( char c in sentence)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
