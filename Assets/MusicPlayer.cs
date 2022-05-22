using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> melodies;
    public List<AudioClip> bases;
    private AudioSource melodyPlayer;
    private AudioSource basePlayer;
    int song = 0;
    public float melodyVolume = 0.3f;
    public float baseVolume = 0.1f;
    public float originalMelodyVolume = 0.3f;
    public float originalBaseVolume = 0.1f;
    public float transitionTime = 3;
    public float timeElapsed = 3;
    public float melodyGoalVolume = 0.3f;
    public float baseGoalVolume = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        melodyPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        basePlayer = transform.GetChild(1).GetComponent<AudioSource>();
        melodyPlayer.volume = melodyVolume;
        basePlayer.volume = baseVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!melodyPlayer.isPlaying)
        {
            melodyPlayer.clip = melodies[song];
            basePlayer.clip = bases[song];
            melodyPlayer.Play();
            basePlayer.Play();
            song++;
            if(song > 1)
            {
                song = 0;
            }
        }
        if(timeElapsed < transitionTime)
        {
            timeElapsed += Time.deltaTime;
            float melodyDirection = melodyGoalVolume - originalMelodyVolume;
            float baseDirection = baseGoalVolume - originalBaseVolume;

        }
        
    }

    public void ChangeVolumeSmoothly(float melodyVolume, float baseVolume)
    {
        melodyGoalVolume = melodyVolume;
        baseGoalVolume = baseVolume;
        originalMelodyVolume = this.melodyVolume;
        originalBaseVolume = this.baseVolume;
        timeElapsed = 0;
    }
}
