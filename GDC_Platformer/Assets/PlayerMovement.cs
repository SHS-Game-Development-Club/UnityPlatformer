using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    private Vector3 respawnPoint; //creates a respawn point
    public GameObject fallDetector; 
    
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        

    }

    // Update is called once per frame
    private void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

        if (Input.GetButtonDown("Jump")){
            rb.velocity = new Vector2(rb.velocity.x, 10f);
            
        }
        fallDetector.transform.position = new Vector2(transform.position.x , fallDetector.transform.position.y);  
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag  == "FallDetector")
        {
            transform.position = respawnPoint;  //resets player's position to respawn point if they fall
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    }
}
