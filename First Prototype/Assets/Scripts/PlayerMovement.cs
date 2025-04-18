using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    float horizontal;

    public float maxSpeed;
    public float acceltime;
    public float deceltime;
    public float airAccel;
    public float jumpForgiveness;
    
    private bool m_Grounded;

    public UnityEvent OnLandEvent;

    // Tweening Variables
    private float oldHorizontal;
    float start;
    float diff;
    float progress;
    float rate;
    float time;

    // Holding that strange vertical velocity bug accountable
    float oldy;

    // collision momentum killing
    float oldx;
    float oldtime;
    float expectedAcceleration;

    bool resetFlag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (OnLandEvent == null) {
		    OnLandEvent = new UnityEvent();
        }
        OnLandEvent.AddListener(Landed);
        horizontal = 0;
        oldHorizontal = 0;
        oldx = transform.position.x;
        oldy = transform.position.y;
        oldtime = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {   
        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal < 0 && m_Grounded) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (horizontal > 0 && m_Grounded) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        animator.SetFloat("RawHorizontal", horizontal);

        if (Input.GetKeyDown("space") && animator.GetBool("grounded"))
        {
            rb2D.AddForce(Vector2.up * 500);
            animator.SetBool("grounded", false);
            m_Grounded = false; 
        }
        if (animator.GetBool("grounded") && Mathf.Abs(rb2D.linearVelocity.y) > jumpForgiveness) {
            animator.SetBool("grounded", false);
            m_Grounded = false;
        }
        
    }

    void FixedUpdate()
    {
        var dtime = Time.fixedTime - oldtime;
        float oldvx;

        if(dtime != 0) oldvx = (oldx - transform.position.x) / dtime;
        else oldvx = rb2D.linearVelocity.x;

        var discrepancyx = oldvx - rb2D.linearVelocity.x;

        oldx = transform.position.x;
        oldtime = Time.fixedTime;

        if(oldHorizontal != Mathf.Ceil(Mathf.Abs(horizontal)) * Mathf.Sign(horizontal) || Mathf.Abs(discrepancyx) < 0.5 || resetFlag) {

            resetFlag = false;

            start = rb2D.linearVelocity.x;
            diff = maxSpeed * horizontal - start;
            float speedPercent;
            if(horizontal == 0f) speedPercent = Mathf.Abs(rb2D.linearVelocity.x / maxSpeed);
            else speedPercent = Mathf.Abs((maxSpeed * horizontal - rb2D.linearVelocity.x) / maxSpeed * horizontal);
            if(horizontal == 0f) time = speedPercent * deceltime;
            else time = speedPercent * acceltime;
            if(time == 0f) rate = 0;
            else rate = 1 / time;
            progress = 0;
        }
        if(progress < 1f) {
            progress = progress + rate * Time.deltaTime * (m_Grounded ? 1 : airAccel);
            float x;
            if(progress >= 1f) x = 1;
            else if (progress * progress == 0f) x = 0;
            else x = 1 + (Mathf.Sqrt(1 - ((1 - progress) * (1 - progress))) - 1);
            if(float.IsNaN(x)) Debug.Log("");
            
            float newVelocity = start + x * diff;
            expectedAcceleration = (newVelocity - rb2D.linearVelocity.x);


            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x + expectedAcceleration, rb2D.linearVelocity.y);
            if(time == 0f) rate = 0;
            else rate = 1 / time;
        } else if (rb2D.linearVelocity.x != maxSpeed * horizontal) {
            resetFlag = true;
        }


		bool wasGrounded = m_Grounded;

        m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCapsuleAll(transform.position - new Vector3(0, 1.01f, 0), new Vector2(0.313f, 0.1f), CapsuleDirection2D.Horizontal, 90f, 1);
		for (int i = 0; i < colliders.Length; i++)
		{
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    Debug.Log(colliders[i].name);
                    OnLandEvent.Invoke();
            }
        }
        
        float oldvy;

        oldvy = (transform.position.y - oldy) / Time.deltaTime;

        animator.SetFloat("Horizontal", rb2D.linearVelocity.x);
        animator.SetFloat("Vertical", oldvy);

        oldy = transform.position.y;

        oldHorizontal = Mathf.Ceil(Mathf.Abs(horizontal)) * Mathf.Sign(horizontal);
	}


    public void Landed() {
        animator.SetBool("grounded", true);
    }
}