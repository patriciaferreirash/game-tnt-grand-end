using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource AudioSoucerMusicaDeFundo;
    public AudioClip[] MusicasDeFundo;
    void Start()
    {
        AudioClip musicaPrincipal = MusicasDeFundo[0];
        AudioSoucerMusicaDeFundo.clip = musicaPrincipal;
        AudioSoucerMusicaDeFundo.Play();
    }

    public void MudarMusica(int indice)
    {
        if (indice >= 0 && indice < MusicasDeFundo.Length)
        {
            AudioSoucerMusicaDeFundo.clip = MusicasDeFundo[indice];
            AudioSoucerMusicaDeFundo.Play();
        }
    }
}
