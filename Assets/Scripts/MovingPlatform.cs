using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector2 start;
    Vector2 end;
    bool increasing = true;
    float pos = 0;
    public float speed = 0.1f;
    Vector2 original;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.Find("Start").position;
        end = transform.Find("End").position;
        original = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = original + (end - start) * pos;
        if (increasing)
        {
           
            pos += speed * Time.deltaTime;
        }
        else
        {
            pos -= speed * Time.deltaTime;
        }
        if(pos >= 1)
        {
            increasing = false;
        }
        if (pos <= 0)
        {
            increasing = true;
        }

    }
}
