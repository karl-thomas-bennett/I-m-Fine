using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public Vector2 activeCheckpoint;
    public Player player;
    public List<Vector2> checkpoints = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        activeCheckpoint = transform.GetChild(0).position;
        player = GameObject.Find("Player").GetComponent<Player>();
        for(int i = 0; i < transform.childCount; i++)
        {
            checkpoints.Add(transform.GetChild(i).position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < checkpoints.Count; i++)
        {
            if(((Vector2)player.transform.position - checkpoints[i]).magnitude < 10)
            {
                activeCheckpoint = checkpoints[i];
            }
        }
    }
}
