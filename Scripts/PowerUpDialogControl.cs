using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpDialogControl : MonoBehaviour
{
    public GameObject dialogueBackground;
    public Text actorName;
    public Text dialogText;

    public float totalDisplayTime;
    public float typingSpeed;
    public bool isPlaying = false;

    private Coroutine currentDialog;
    private GameObject currentAnimatedObj;

    public void ShowDialog(string actorName, List<string> sentences, GameObject animatedObj)
    {
        if (currentDialog != null)
        {
            StopCoroutine(currentDialog);
        }

        currentDialog = StartCoroutine(PlayDialog(actorName, sentences, animatedObj));
    }

    IEnumerator PlayDialog(string actorName, List<string> sentences, GameObject animatedObj)
    {
        isPlaying = true;
        dialogueBackground.SetActive(true);
        this.actorName.text = actorName;

        if (animatedObj != null)
        {
            currentAnimatedObj = animatedObj;
            currentAnimatedObj.SetActive(true);
        }

        int totalChars = 0;
        foreach (string sentence in sentences)
        {
            totalChars += sentence.Length;
        }

        float totalTypingTime = totalChars * typingSpeed;
        float totalPauseTime = Mathf.Max(0, totalDisplayTime - totalTypingTime);
        float pauseBetweenSentences = sentences.Count > 1 ? totalPauseTime / (sentences.Count - 1) : totalPauseTime;

        for (int i = 0; i < sentences.Count; i++)
        {
            yield return StartCoroutine(TypingSentences(sentences[i]));

            if (i < sentences.Count - 1)
            {
                yield return new WaitForSeconds(pauseBetweenSentences);
            }
        }

        if (sentences.Count == 1)
        {
            yield return new WaitForSeconds(totalPauseTime);
        }

        dialogueBackground.SetActive(false);

        if (currentAnimatedObj != null)
        {
            currentAnimatedObj.SetActive(false);
            currentAnimatedObj = null;
        }

        isPlaying = false;
    }

    IEnumerator TypingSentences(string sentence)
    {
        dialogText.text = "";

        foreach (char c in sentence)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }





}
