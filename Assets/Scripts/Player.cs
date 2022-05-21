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
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        distToGround = collider.bounds.extents.y;
        LayerMask mask = ~LayerMask.GetMask("Player");
        Debug.Log(mask);
    }

    // Update is called once per frame
    void Update()
    {
        HandleControls();
    }

    private void FixedUpdate()
    {
        if (left)
        {
            rigidbody.AddForce(new Vector2(-acceleration, 0));
            if(rigidbody.velocity.x < -maxSpeed)
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

        MovingPlatform platform = GetMovingPlatformPlayerIsOn();
        if (platform != null)
        {
            transform.parent = platform.transform;
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            transform.parent = null;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (OnGround())
        {
            if (jump)
            {
                if (!jumpBurstUsed)
                {
                    jumpBurstUsed = true;
                    rigidbody.AddForce(new Vector2(0, jumpForce));
                }
            }
            jumpBurstUsed = false;
            
        }
        else
        {
            transform.parent = null;
            GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    private MovingPlatform GetMovingPlatformPlayerIsOn()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, distToGround + 0.3f, ~LayerMask.GetMask("Player"));
        if (!hit)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(-width / 2, 0, 0), Vector3.down, distToGround + 0.3f, ~LayerMask.GetMask("Player"));
        }
        if (!hit)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(width / 2, 0, 0), Vector3.down, distToGround + 0.3f, ~LayerMask.GetMask("Player"));
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
        return Physics2D.Raycast(transform.position, Vector3.down, distToGround + 0.1f, ~LayerMask.GetMask("Player"))
            || Physics2D.Raycast(transform.position + new Vector3(-width / 2, 0, 0), Vector3.down, distToGround + 0.1f, ~LayerMask.GetMask("Player"))
            || Physics2D.Raycast(transform.position + new Vector3(width / 2, 0, 0), Vector3.down, distToGround + 0.1f, ~LayerMask.GetMask("Player"));
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
