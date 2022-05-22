using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Vector2 activeCheckpoint;
    public Player player;
    public List<Vector2> checkpoints = new List<Vector2>();
    MusicPlayer music;
    public float baseIncreasePerCheckpoint = 0.2f;
    public float melodyDecreasePerCheckpoint = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        activeCheckpoint = transform.GetChild(0).position;
        player = GameObject.Find("Player").GetComponent<Player>();
        for(int i = 0; i < transform.childCount; i++)
        {
            checkpoints.Add(transform.GetChild(i).position);
        }
        music = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < checkpoints.Count; i++)
        {
            if(((Vector2)player.transform.position - checkpoints[i]).magnitude < 10)
            {
                if(activeCheckpoint != checkpoints[i] && music != null)
                {
                    music.ChangeVolumeSmoothly(music.originalMelodyVolume - melodyDecreasePerCheckpoint * i, music.originalBaseVolume + baseIncreasePerCheckpoint * i);
                }
                activeCheckpoint = checkpoints[i];
                
            }
        }
    }
}
