using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPick : MonoBehaviour
{
    public LetterType letterType;
    public LetterCollector letterCollector;
    public AudioSource letterPickUpAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            letterPickUpAudio.Play();
            letterCollector.CollectedLetters(letterType);
            Destroy(gameObject);
        }
    }
}
