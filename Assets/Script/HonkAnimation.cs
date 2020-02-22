using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonkAnimation : MonoBehaviour
{
    public Animator honk;
    public GameObject m_honk;

    public AudioSource m_play;
    public AudioClip[] m_sfx;

    void Start()
    {
        honk = gameObject.GetComponent<Animator>();

        AudioSource m_play = gameObject.GetComponent<AudioSource>();
    }

    public void Honk()
    {
        honk.SetTrigger("Active");
        PlaySFX();
    }

    private void PlaySFX()
    {
        int random = Random.Range(0, m_sfx.Length);

        m_play.clip = m_sfx[random];
        m_play.Play();
      
    }
}
