using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileText : MonoBehaviour
{
    public float life;
    public float fade;
    float fadeTime;
    new SpriteRenderer renderer;
    TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        fadeTime = fade;
    }

    // Update is called once per frame
    void Update()
    {
        if(life > 0)
        {
            life -= Time.deltaTime;
        }
        else
        {
            if(fadeTime > 0)
            {
                fadeTime -= Time.deltaTime;
                renderer.color = new Color(1, 1, 1, fadeTime / fade);
                text.color = new Color(0, 0, 0, fadeTime / fade);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
