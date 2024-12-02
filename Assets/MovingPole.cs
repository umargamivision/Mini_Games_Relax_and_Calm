using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPole : MonoBehaviour
{
    public bool move;
    public Rigidbody2D rb;
    public Vector2 endPoint;
    public float moveSpeed;
    private void FixedUpdate() 
    {
        if(move)
        rb.velocity = (endPoint);
    }
}
