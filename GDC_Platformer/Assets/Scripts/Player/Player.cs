using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public Rigidbody2D rb;
	public SpriteRenderer sr;
	public Animator anim;

	[Header("Movement")]
    public float moveSpeed;
    public float acceleration;
    public float deceleration;
    public float velocityPower;
    public float friction;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCutM;
    public float fallGravityM;
    public float gravityScale;
	private bool isJumping;
	private bool wasGrounded;

	[Header("Collision")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    private Vector3 respawnPoint;
    public GameObject fallDetector;
    public bool killed;

	//Coyote Time + Jump Buffering
	private float coyoteTime = 0.2f;
	private float coyoteTimeCounter;
	private float jumpBufferTime = 0.2f;
	private float jumpBufferCounter;

	//Singleton Instantiation
	private static Player instance;
	public static Player Instance {
		get {
			if(instance == null)
				instance = GameObject.FindObjectOfType<Player>();
			return instance;
		}
	}

    void Start() {
 		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

        respawnPoint = transform.position;
    }

	void Update() {
        fallDetector.transform.position = new Vector2(transform.position.x , fallDetector.transform.position.y);
        if(killed) {
            killed = false;
            transform.position = respawnPoint;
        }

		Attack();
		if(isGrounded()) {
			coyoteTimeCounter = coyoteTime;
			if(Input.GetAxisRaw("Horizontal") != 0)
				anim.SetBool("Run", true);
			else
				anim.SetBool("Run", false);
		} else {
			coyoteTimeCounter -= Time.deltaTime;
		}

		//Coyote Time + Jump Buffering
		if(isGrounded() && !wasGrounded)
			anim.SetBool("Jump", false);

		if(Input.GetButtonDown("Jump")) {
			anim.SetBool("Jump", true);
			jumpBufferCounter = jumpBufferTime;
		} else
			jumpBufferCounter -= Time.deltaTime;

		if(coyoteTimeCounter > 0 && jumpBufferCounter > 0f && !isJumping) {
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			jumpBufferCounter = 0f;
		}
		if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * (1 - jumpCutM));
			StartCoroutine(JumpCD());
			coyoteTimeCounter = 0f;
		}

		//Gravity
        if(rb.velocity.y < 10)
            rb.gravityScale = gravityScale * fallGravityM;
        else 
            rb.gravityScale = gravityScale;
	}

	void FixedUpdate() {
        Move();
		wasGrounded = isGrounded();
	}

	public void Move() {
		//Direction of the player
		if(Input.GetAxisRaw("Horizontal") != 0)
			sr.flipX = (Input.GetAxisRaw("Horizontal") == 1 ? false : true);

		//In charge of acceleration, top speed, and deceleration
        float targetSpeed = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPower) * Mathf.Sign(speedDiff);
        rb.AddForce(movement * Vector2.right);

		//Friction
		if(isGrounded() == true && targetSpeed == 0) {
			float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(friction));
			amount *= Mathf.Sign(rb.velocity.x);
			rb.AddForce(-amount * Vector2.right, ForceMode2D.Impulse);
		}
	}

    public bool isGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
	}

	public void Attack() {
		if(Input.GetButtonDown("Fire1"))
			StartCoroutine(AttackCD());
	}

	//Coroutines
	public IEnumerator JumpCD() {
		isJumping = true;
		yield return new WaitForSeconds(0.4f);
		isJumping = false;
	}

	public IEnumerator AttackCD() {
		anim.SetBool("Attack", true);
		yield return new WaitForSeconds(0.2f);
		anim.SetBool("Attack", false);
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag  == "FallDetector")
            transform.position = respawnPoint;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.tag == "Spike" || collision.collider.tag == "Enemy")
			transform.position = respawnPoint;
    }
}
