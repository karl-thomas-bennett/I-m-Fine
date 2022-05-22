using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<string> leftControls = new List<string> {"left", "a" };
    public List<string> rightControls = new List<string> { "right", "d" };
    public List<string> jumpControls = new List<string> { "space", "w", "up" };
    public bool left = false;
    public bool right = false;
    public bool jump = false;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    public float maxSpeed = 10;
    public float decelerationAboveMaxSpeed = 0.1f;
    public float acceleration = 10;
    public float jumpForce = 300;
    public bool jumpBurstUsed = false;
    public float distToGround;
    public float width = 1;
    public float height = 1;
    private LayerMask mask;
    AudioSource jumpSound;
    MusicPlayer music;
    public Sprite jumpSprite;
    public Sprite idleSprite;

    private List<Collider2D> respawnTriggers = new List<Collider2D>();
    private Checkpoints checkpoints;
    private Transform sprite;
    public float timeBetweenJumps = 1;
    public float timeTillNextJump = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        distToGround = collider.bounds.extents.y;
        mask = ~LayerMask.GetMask("Player");
        checkpoints = GameObject.Find("Checkpoints").GetComponent<Checkpoints>();
        for(int i = 0; i < transform.childCount; i++)
        {
            respawnTriggers.Add(transform.GetChild(i).GetComponent<Collider2D>());
        }
        jumpSound = GetComponent<AudioSource>();
        sprite = transform.Find("Capybara");
        music = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleControls();
    }

    private void FixedUpdate()
    {
        HandleMove();
        HandleJump();
        HandleSprite();
    }

    private void HandleSprite()
    {
        Animator animator = sprite.GetComponent<Animator>();

        if (left)
        {
            sprite.localScale = new Vector3(-Math.Abs(sprite.localScale.x), sprite.localScale.y, sprite.localScale.z);
        }
        if (right)
        {
            sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x), sprite.localScale.y, sprite.localScale.z);
        }
        if (!OnGround())
        {
            animator.enabled = false;
            SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
            renderer.sprite = jumpSprite;
        }
        else
        {
            if(Math.Abs(rigidbody.velocity.x) < 0.1f)
            {
                animator.enabled = false;
                SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
                renderer.sprite = idleSprite;
            }
            else
            {
                animator.enabled = true;
            }
            
        }
        
    }

    public void Respawn()
    {
        rigidbody.position = checkpoints.activeCheckpoint;
    }

    private void HandleMove()
    {
        if (left)
        {
            rigidbody.AddForce(new Vector2(-acceleration, 0));
            if (rigidbody.velocity.x < -maxSpeed)
            {
                rigidbody.velocity = new Vector2(Mathf.Min(-maxSpeed, rigidbody.velocity.x + decelerationAboveMaxSpeed), rigidbody.velocity.y);
            }
        }

        if (right)
        {
            rigidbody.AddForce(new Vector2(acceleration, 0));
            if (rigidbody.velocity.x > maxSpeed)
            {
                rigidbody.velocity = new Vector2(Mathf.Max(maxSpeed, rigidbody.velocity.x - decelerationAboveMaxSpeed), rigidbody.velocity.y);
            }
        }
    }

    private void HandleJump()
    {
        if(timeTillNextJump > 0)
        {
            timeTillNextJump -= Time.deltaTime;
        }
        MovingPlatform platform = GetMovingPlatformPlayerIsOn();
        if (platform != null)
        {
            transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }
        if (OnGround())
        {
            if (jump)
            {
                if (!jumpBurstUsed && timeTillNextJump <= 0)
                {
                    jumpBurstUsed = true;
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                    rigidbody.AddForce(new Vector2(0, jumpForce));
                    jumpSound.Play();
                    timeTillNextJump = timeBetweenJumps;
                }
            }
            jumpBurstUsed = false;

        }
        else
        {
            transform.parent = null;
        }
    }

    private MovingPlatform GetMovingPlatformPlayerIsOn()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, distToGround + 0.3f, mask);
        if (!hit)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(-width / 2, 0, 0), Vector3.down, distToGround + 0.3f, mask);
        }
        if (!hit)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(width / 2, 0, 0), Vector3.down, distToGround + 0.3f, mask);
        }
        if (!hit)
        {
            return null;
        }
        if(hit.transform.TryGetComponent(out MovingPlatform platform))
        {
            return platform;
        }
        return null;
    }

    private bool OnGround() {
        Debug.Log(distToGround);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 0.6f, mask);
        if (!hit)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(-width / 2, 0, 0), Vector3.down, 0.6f, mask);
        }
        if (!hit)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(width / 2, 0, 0), Vector3.down, 0.6f, mask);
        }
        if(!hit)
        {
            return false;
        }
        if (!hit.collider.isTrigger)
        {
            return true;
        }
        return false;
    }

    private void HandleControls()
    {
        HandlePress();
        HandleRelease();
    }

    private void HandlePress()
    {
        if (IsPressed(leftControls))
        {
            left = true;
        }
        if (IsPressed(rightControls))
        {
            right = true;
        }
        if (IsPressed(jumpControls))
        {
            jump = true;
        }
    }

    private void HandleRelease()
    {
        if (IsReleased(leftControls))
        {
            left = false;
        }
        if (IsReleased(rightControls))
        {
            right = false;
        }
        if (IsReleased(jumpControls))
        {
            jump = false;
        }
    }

    private bool IsPressed(List<string> controls)
    {
        for(int i = 0; i < controls.Count; i++)
        {
            if (Input.GetKeyDown(controls[i])){
                return true;
            }
        }
        return false;
    }

    private bool IsReleased(List<string> controls)
    {
        for (int i = 0; i < controls.Count; i++)
        {
            if (Input.GetKeyUp(controls[i]))
            {
                return true;
            }
        }
        return false;
    }
}
