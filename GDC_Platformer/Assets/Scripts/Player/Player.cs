using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField]
	internal PlayerMovement move;
	[SerializeField]
	internal PlayerCollision collision;

	public Rigidbody2D rb;
	public SpriteRenderer sr;
	public Animator anim;

    void Start() {
 		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		move = GetComponent<PlayerMovement>();
		collision = GetComponent<PlayerCollision>();
    }
}
