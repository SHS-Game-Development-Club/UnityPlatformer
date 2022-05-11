using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
	[SerializeField]
	Player player;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Vector2 bottom;
    private Vector3 respawnPoint; //creates a respawn point
    public GameObject fallDetector;

    void Start() {
        player = GetComponent<Player>();
        respawnPoint = transform.position;
    }

	void Update() {
        fallDetector.transform.position = new Vector2(transform.position.x , fallDetector.transform.position.y);  
	}

    public bool isGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag  == "FallDetector")
            transform.position = respawnPoint;  //resets player's position to respawn point if they fall
        else if (collision.tag == "Checkpoint")
            respawnPoint = transform.position;
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.tag == "Spike")
            Debug.Log("Collided with spike");
    }
}
