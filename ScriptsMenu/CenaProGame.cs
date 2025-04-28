using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CenaProGame : MonoBehaviour
{
    
    void OnEnable()
    {
        SceneManager.LoadScene(2);
    }

    //game cena 0, menu cena 1, essa cutscene cena 2
}
