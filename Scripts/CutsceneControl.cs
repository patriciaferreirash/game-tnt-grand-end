using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneControl : MonoBehaviour
{
    public PlayerPlatform player;
    public DialogControl dialogControl;
    public GameObject npc;
    public Animator npcAnim;
    public Transform walkTarget;

    public float speed;
    public bool isWalking = false;

    private float originalScaleX;

    // Start is called before the first frame update
    void Start()
    {
        player.isDialogueActive = true;
        npcAnim.SetBool("iswalk", false);
        originalScaleX = npc.transform.localScale.x;
        StartCoroutine(PlayerIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            Vector3 scale = npc.transform.localScale;
            scale.x = (walkTarget.position.x > npc.transform.position.x) ? Mathf.Abs(originalScaleX) : -Mathf.Abs(originalScaleX);
            npc.transform.localScale = scale;
        }
    }

    IEnumerator PlayerIntro()
    {
        List<string> frases = new List<string>()
        {
            "Fala aí, Zane. Eu vou mostrar que tenho mais energia que você!",
            "Aceite meu desafio. Abriu, partiu pro play!"
        };

        dialogControl.ShowDialog("Rival", frases);

        yield return new WaitUntil(() => dialogControl.isPlaying == false);

        npcAnim.SetBool("iswalk", true);
        isWalking = true;

        while (Vector2.Distance(npc.transform.position, walkTarget.transform.position) > 0.1f)
        {
            npc.transform.position = Vector2.MoveTowards(npc.transform.position, walkTarget.position, speed * Time.deltaTime);
            yield return null;
        }

        npcAnim.SetBool("iswalk", false);
        npc.SetActive(false);

        player.isDialogueActive = false;
    }
}
