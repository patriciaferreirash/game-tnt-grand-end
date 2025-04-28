using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePowerUpFocus : MonoBehaviour
{
    public GameObject powerupFocus;
    public AudioSource victorySound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            powerupFocus.SetActive(true);
            victorySound.Play();
        }
    }
}
