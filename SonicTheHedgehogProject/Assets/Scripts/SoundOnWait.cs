using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnWait : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    public float wait = 1f;
    [SerializeField] private bool start;


    private void Start()
    {
        if (start)
            StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        yield return new WaitForSeconds(wait);
        m_AudioSource.Play();
    }
}
