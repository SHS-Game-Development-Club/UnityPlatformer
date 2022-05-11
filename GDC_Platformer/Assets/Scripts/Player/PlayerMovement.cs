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

    private void Start() {
		playerScript = GetComponent<Player>();
    }

    private void Update() {
		
    }

	void FixedUpdate() {
		Jump();
        Move();
		//Fall Gravity
        if(playerScript.rb.velocity.y < 10)
            playerScript.rb.gravityScale = gravityScale * fallGravityM;
        else 
            playerScript.rb.gravityScale = gravityScale;
	}

	public void Move() {
		if(Input.GetAxisRaw("Horizontal") != 0)
			playerScript.sr.flipX = (Input.GetAxisRaw("Horizontal") == 1 ? false : true);

        float targetSpeed = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float speedDiff = targetSpeed - playerScript.rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPower) * Mathf.Sign(speedDiff);
        playerScript.rb.AddForce(movement * Vector2.right);

		//anim.SetFloat("Speed", Mathf.Abs(targetSpeed));
		
		if(playerScript.collision.isGrounded() == true && targetSpeed == 0) {
			float amount = Mathf.Min(Mathf.Abs(playerScript.rb.velocity.x), Mathf.Abs(friction));
			amount *= Mathf.Sign(playerScript.rb.velocity.x);
			playerScript.rb.AddForce(-amount * Vector2.right, ForceMode2D.Impulse);
		}
	}
    public void Jump() {
		if(Input.GetButtonDown("Jump") && playerScript.collision.isGrounded()) {
			playerScript.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}
		//if(playerScript.rb.velocity.y < 0.01f && playerScript.collision.isGrounded()) {
		//	anim.SetBool("Jumping", false);
		//}
		//if(Input.GetButtonUp("Jump") && playerScript.rb.velocity.y > 0f) {
			//playerScript.rb.AddForce(Vector2.down * playerScript.rb.velocity.y * (1 - jumpCutM), ForceMode2D.Impulse);
			//jumpReleased = 0;
		//}
	}
}
