using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    public List<GameObject> texts = new List<GameObject>();
    public float timeBetweenTexts = 1;
    private float timeTillNextText = 0;
    public Vector2 direction = new Vector2(-1, 0);
    public float force = 10;
    public float textLife = 3;
    public float textFadeDuration = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeTillNextText -= Time.deltaTime;
        if(timeTillNextText <= 0)
        {
            GameObject obj = Instantiate(texts[Random.Range(0, texts.Count-1)], transform.position, Quaternion.identity);
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            ProjectileText text = obj.GetComponent<ProjectileText>();
            text.life = textLife;
            text.fade = textFadeDuration;
            rigidbody.AddTorque(Random.Range(-200f, 200f));
            rigidbody.AddForce(direction * force);
            timeTillNextText = timeBetweenTexts;
        }
    }

    private int RandomInt(int min, int max)
    {
        int rand = Random.Range(min, max);
        while(rand == max)
        {
            rand = Random.Range(min, max);
        }
        return rand;
    }
}
