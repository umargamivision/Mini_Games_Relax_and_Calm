using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DoReMiSpace
{
    public class PlayerController : MonoBehaviour
    {
        public int currentLevel;
        public float[] hightLevels;
        public string obstacleName;
        public float collisionForce;
        public float jumpForce;
        public float moveSpeed;
        public float maxHight;
        public float maxSpeed;
        public Vector3 gravity = new Vector3(0,-9.8f,0);
        public Rigidbody rb;
        bool jump;
        public void Update()
        {
            Physics.gravity = gravity;
            jump = Input.GetMouseButtonDown(0);
            if(jump)
            {
                rb.AddForce(Vector3.up*jumpForce , ForceMode.Impulse);
            }
        }
        private void FixedUpdate() 
        {
            if(rb.velocity.magnitude<maxSpeed)
            rb.AddForce(Vector3.forward*moveSpeed);

            //rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxHight , maxHight), rb.velocity.z);
        }
        private void OnCollisionEnter(Collision other) 
        {
            if(other.gameObject.name.Contains(obstacleName))
            {
                rb.AddForce(-transform.forward*collisionForce , ForceMode.Impulse);
            }
        }
    }
}