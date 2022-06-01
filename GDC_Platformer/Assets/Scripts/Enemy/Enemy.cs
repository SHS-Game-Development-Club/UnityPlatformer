using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float attackCD;
    public float range;
    public BoxCollider2D box;
    public LayerMask playerLayer;
    private float cdTimer = Mathf.Infinity;
    private Animator anim;

    void Start() {
        box = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        cdTimer += Time.deltaTime;

        if(inRange()) {
            Debug.Log("BRUHBRUHBHR");
            if(cdTimer >= attackCD) {
                cdTimer = 0;
                anim.SetTrigger("Attack");
            }
        }
    }

    private bool inRange() {
        RaycastHit2D hit = 
            Physics2D.BoxCast(box.bounds.center + transform.right * range * transform.localScale.x,
            new Vector3(box.bounds.size.x * range, box.bounds.size.y, box.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(box.bounds.center + transform.right * range * transform.localScale.x, new Vector3(box.bounds.size.x * range, box.bounds.size.y, box.bounds.size.z));
    }

    private void KillPlayer() {
        if(inRange())
            Player.Instance.killed = true;
    }
}