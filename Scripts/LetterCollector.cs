using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LetterType
{
    T1,
    N,
    T2
}

public class LetterCollector : MonoBehaviour
{
    [Header("References")]
    public GameObject canvasBGLetters;
    private CanvasGroup canvasGroup;

    [Header("Images Letters References")]
    public Image[] collectedLetterImage;
    public AudioSource victoryAudio;

    [Header("Reference PowerUp Vital")]
    public GameObject powerupVital;

    private bool[] collected = new bool[3];

    public float fadeDuration;

    private void Awake()
    {
        canvasGroup = canvasBGLetters.GetComponent<CanvasGroup>();
    }

    public void EnableLetterChallenger()
    {
        canvasBGLetters.SetActive(true);
        StartCoroutine(FadeBackground(0f, 1f));

        for (int i = 1; i < collectedLetterImage.Length; i++)
        {
            collected[i] = false;
            collectedLetterImage[i].enabled = false;
        }
    }

    public void CollectedLetters(LetterType type)
    {
        int index = (int)type;

        Debug.Log("Coletou letra: " + type);

        if (!collected[index])
        {
            collected[index] = true;
            collectedLetterImage[index].enabled = true;

            Debug.Log("Imagem da letra " + type + " ativada!");

            if (AllLettersCollected())
            {
                Debug.Log("Todas as letras coletadas. Desativando fundo.");

                powerupVital.SetActive(true);
                Invoke(nameof(ActiveSoundVictory), 1.5f);
                StartCoroutine(FadeBackgroundDisable());
            }
        }
    }

    private bool AllLettersCollected()
    {
        foreach (bool c in collected)
        {
            if (!c)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator FadeBackground(float start, float end)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, t / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = end;
    }

    IEnumerator FadeBackgroundDisable()
    {
        yield return FadeBackground(1f, 0f);
        canvasBGLetters.SetActive(false);
    }

    private void ActiveSoundVictory()
    {
        victoryAudio.Play();
    }
}