using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColectManager : MonoBehaviour
{
    [Header("Componets Colect")]
    public GameObject[] fallObjects;
    public Transform areaSpawnLeft;
    public Transform areaSpawnRight;
    public float timeSpawns;

    [Header("UI da Barra")]
    public Image barEmpty;
    public Image barFull;
    public float valueColect;

    [Header("Others")]
    public GameObject powerUpTNT;
    public GameObject triggerToBegin;
    public AudioSource victoryAudio;
    public AudioSource pickSound;

    private bool activeColect = false;
    private Coroutine spawnRoutine;

    // Start is called before the first frame update
    void Start()
    {
        barFull.fillAmount = 0f;
        barFull.gameObject.SetActive(false);
        barFull.gameObject.SetActive(false);
        powerUpTNT.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!activeColect && collision.gameObject.CompareTag("Player"))
        {
            activeColect = true;

            barEmpty.gameObject.SetActive(true);
            barFull.gameObject.SetActive(true);

            spawnRoutine = StartCoroutine(SpawnObjects());
        }
    }

    public void ColectObject()
    {
        barFull.fillAmount += valueColect;

        if(barFull.fillAmount >= 1f)
        {
            victoryAudio.Play();
            Finish();
        }
    }

    private void Finish()
    {
        if(spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        powerUpTNT.SetActive(true);

        barFull.gameObject.SetActive(false);
        barEmpty.gameObject.SetActive(false);
    }

    IEnumerator SpawnObjects()
    {
        while(barFull.fillAmount < 1f)
        {
            Vector2 position = new Vector2(Random.Range(areaSpawnLeft.position.x, areaSpawnRight.position.x), areaSpawnLeft.position.y);

            GameObject prefab = fallObjects[Random.Range(0, fallObjects.Length)];
            Instantiate(prefab, position, Quaternion.identity);

            yield return new WaitForSeconds(timeSpawns);
        }

        powerUpTNT.SetActive(true);

        barEmpty.gameObject.SetActive(false);
        barFull.gameObject.SetActive(false);
    }
}
