using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    Jump,
    Focus,
    Vital
}

public class PowerUpsControl : MonoBehaviour
{
    public PowerUpType powerUpType;

    public AudioClip collectSound;

    public string powerUpNameTrigger;

    public LetterCollector letterCollector;

    [Header("Dialogues")]
    public PowerUpDialogControl dialogControl;
    public string actorNamePowerup;
    public List<string> powerUpDialog = new List<string>();
    public GameObject animatedButton;

    [Header("Camera")]
    public Camera playerCamera;
    public float zoomAmount;
    public float zoomDuration;
    public float zoomHoldDuration;
    public float moveOffset;
    public float moveDuration;

    // Start is called before the first frame update
    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerPlatform>();
            
            if(player != null)
            {
                ApplyPowerUp(player);
            }

            if(powerUpType == PowerUpType.Focus && letterCollector != null)
            {
                
                letterCollector.EnableLetterChallenger();
            }

            if (powerUpType == PowerUpType.Focus)
            {
                player.StartCoroutine(player.ZoomCameraEffect(playerCamera, player, zoomAmount, zoomDuration, zoomHoldDuration, moveOffset, moveDuration));
            }

            if (dialogControl != null && powerUpDialog.Count > 0)
            {
                dialogControl.ShowDialog(actorNamePowerup, powerUpDialog, animatedButton);
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
    }

    private void ApplyPowerUp(PlayerPlatform player)
    {
        if (!string.IsNullOrEmpty(powerUpNameTrigger))
        {
            player.PlayPowerUpAnimation(powerUpNameTrigger);
        }

        switch (powerUpType)
        {
            case PowerUpType.Jump:
                player.EnableJump();
                break;

            case PowerUpType.Focus:
                player.EnableFocus();
                break;

            case PowerUpType.Vital:
                player.EnableVital();
                break;
        }
    }

    IEnumerator DelayDialogFocus(float delay, string actor, List<string> sentences)
    {
        yield return new WaitForSeconds(delay);
        dialogControl.ShowDialog(actor, sentences, animatedButton);
    }
}
