using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // Start is called before the first frame update
    private float WantedHeight;
    private bool _jumping = false;
    public LayerMask FloorMask;
    private float _horizontal;
    private bool _grounded = false;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float Speed = 5f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public float JumpForceCalculator(float wantedHeight, float weight, float g)
    {
        return weight * Mathf.Sqrt(-2 * wantedHeight * g);
    }



    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump pressed");
            if (_grounded)
                _jumping = true;
        }

        _grounded = GetComponent<CircleCollider2D>().IsTouchingLayers(FloorMask);

        if (!_grounded)
        {
            Debug.Log("On the air");
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallMultiplier;
                //Debug.Log("Down gravity");
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = lowJumpMultiplier;
                //Debug.Log("Up gravity");
            }
        }
        else
        {
            //Debug.Log("grounded");
            rb.gravityScale = 1;
        }


    }


    private void FixedUpdate() 
    {

        if (_jumping) 
        {
            float requieredForce = JumpForceCalculator(WantedHeight, rb.mass, Physics2D.gravity.y * rb.gravityScale);
            rb.AddForce(new Vector2(0, requieredForce), ForceMode2D.Impulse);
        
        }

        transform.position += new Vector3(_horizontal, 0, 0) * Time.fixedDeltaTime * Speed;
    }
    
    


}
