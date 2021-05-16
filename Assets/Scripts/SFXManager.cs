using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip[] _clips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaySFX(int clipNumber)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = _clips[clipNumber];
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
