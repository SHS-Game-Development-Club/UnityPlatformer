using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[SerializeField]
	Player playerScript;
    
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

	//Coyote Time + Jump Buffering
	private float coyoteTime = 0.2f;
	private float coyoteTimeCounter;
	private float jumpBufferTime = 0.2f;
	private float jumpBufferCounter;

	//Animation
	string idle = "PIdle";
	string run = "PRun";
	string jump = "Jump";

    private void Start() {
		playerScript = GetComponent<Player>();
    }

    private void Update() {
		if(playerScript.collision.isGrounded())
			coyoteTimeCounter = coyoteTime;
		else
			coyoteTimeCounter -= Time.deltaTime;

		if(Input.GetButtonDown("Jump"))
			jumpBufferCounter = jumpBufferTime;
		else
			jumpBufferCounter -= Time.deltaTime;

		if(coyoteTimeCounter > 0 && jumpBufferCounter > 0f && !isJumping) {
			playerScript.rb.velocity = new Vector2(playerScript.rb.velocity.x, jumpForce);
			jumpBufferCounter = 0f;
			StartCoroutine(JumpCD());
		}
		// if(Input.GetButtonUp("Jump") && playerScript.rb.velocity.y > 0f) {
		// 	playerScript.rb.velocity = new Vector2(playerScript.rb.velocity.x, playerScript.rb.velocity.y * (1 - jumpCutM));
		// 	coyoteTimeCounter = 0f;
		// }
        if(playerScript.rb.velocity.y < 10)
            playerScript.rb.gravityScale = gravityScale * fallGravityM;
        else 
            playerScript.rb.gravityScale = gravityScale;
    }

	void FixedUpdate() {
        Move();
	}

	public void Move() {
		if(Input.GetAxisRaw("Horizontal") != 0) {
			playerScript.sr.flipX = (Input.GetAxisRaw("Horizontal") == 1 ? false : true);
			playerScript.anim.updAnim(run);
		} else {
			playerScript.anim.updAnim(idle);
		}
        float targetSpeed = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float speedDiff = targetSpeed - playerScript.rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPower) * Mathf.Sign(speedDiff);
        playerScript.rb.AddForce(movement * Vector2.right);

		if(playerScript.collision.isGrounded() == true && targetSpeed == 0) {
			float amount = Mathf.Min(Mathf.Abs(playerScript.rb.velocity.x), Mathf.Abs(friction));
			amount *= Mathf.Sign(playerScript.rb.velocity.x);
			playerScript.rb.AddForce(-amount * Vector2.right, ForceMode2D.Impulse);
		}
	}

	public IEnumerator JumpCD() {
		isJumping = true;
		yield return new WaitForSeconds(0.4f);
		isJumping = false;
	}

}
