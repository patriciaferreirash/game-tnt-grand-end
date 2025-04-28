using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneFinalControl : MonoBehaviour
{
    public PlayerPlatform player;
    public DialogControl dialogueControl;

    [Header("NPC Settings")]
    public GameObject npc;
    public Animator npcAnimator;
    public Transform npcTarget;
    public Transform npcStartPosition;

    public float npcSpeed;

    private bool cutsceneStarted = false;
    private float originalScaleX;

    // Start is called before the first frame update
    void Start()
    {
        npc.transform.position = npcStartPosition.position;
        npc.SetActive(false);
        originalScaleX = npc.transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!cutsceneStarted && collision.gameObject.CompareTag("Player"))
        {
            cutsceneStarted = true;
            StartCoroutine(PlayFinalCutscene());
        }
    }

    IEnumerator PlayFinalCutscene()
    {
        // Trocar a música logo no começo da cutscene
        FindObjectOfType<AudioController>().MudarMusica(1);

        player.isDialogueActive = true;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("iswalk", false);
        player.stepSound.Stop();

        npc.SetActive(true);
        npcAnimator.SetBool("iswalk", true);

        while (Vector2.Distance(npc.transform.position, npcTarget.position) > 0.1f)
        {
            npc.transform.position = Vector2.MoveTowards(npc.transform.position, npcTarget.position, npcSpeed * Time.deltaTime);

            Vector3 scale = npc.transform.localScale;
            scale.x = (npcTarget.position.x > npc.transform.position.x) ? Mathf.Abs(originalScaleX) : -Mathf.Abs(originalScaleX);
            npc.transform.localScale = scale;

            yield return null;
        }

        npcAnimator.SetBool("iswalk", false);

        List<string> finalSentences = new List<string>()
    {
        "Não acredito, você me venceu. A TNT é incrível!",
        "Com ela você pode ter energia e superar os desafios do dia a dia.",
        "Parabéns pela vitória Zane! Abriu, partiu pro play!"
    };

        dialogueControl.ShowDialog("Rival", finalSentences);

        yield return new WaitUntil(() => dialogueControl.isPlaying == false);

        npc.GetComponent<Animator>().SetTrigger("happy");
        player.GetComponent<Animator>().SetTrigger("happy");

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

}
