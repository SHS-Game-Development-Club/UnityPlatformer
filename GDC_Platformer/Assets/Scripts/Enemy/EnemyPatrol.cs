using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    private Transform left;
    private Transform right;

    private bool movingLeft = true;
    private float dir = -1;
    public float speed;
    public Transform enemy;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if(movingLeft) {
            
        }
    }

    private void ChangeDir() {
        ;
    }
}
