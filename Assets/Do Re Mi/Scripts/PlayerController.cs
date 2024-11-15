using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
namespace DoReMiSpace
{
    public class PlayerController : MonoBehaviour
    {
        public bool voiceControll;
        public MicroInput microInput;
        public float[] loudnessOfLevels;
        public bool stoped;
        public bool hasReached;
        public int currentHightLevel;
        public float[] hightLevels;
        public string obstacleName;
        public float collisionForce;
        public float jumpForce;
        public float moveSpeed;
        public float maxSpeed;
        public float gravity;
        public Rigidbody rb;
        public UnityEvent onReachFinishPoint;
        public void StartGame(bool useVoiceControl)
        {
            voiceControll = useVoiceControl;
            stoped = false;
        }
        public void Update()
        {
            if (voiceControll)
                VoiceController();
            else
                TapController();
        }
        public void VoiceController()
        {
            for (int i = 0; i < loudnessOfLevels.Length; i++)
            {
                if(microInput.loudness>loudnessOfLevels[i]) 
                {
                    currentHightLevel = i;
                }
            }
            if(microInput.loudness > loudnessOfLevels[0]) SetHight();
            else AddGravity();
        }
        public void TapController()
        {
            if (Input.GetMouseButtonDown(0))
            {
                UpdateCurrentLevel();
            }
            if (Input.GetMouseButton(0))
                SetHight();
            else
                AddGravity();
        }
        public void UpdateCurrentLevel()
        {
            currentHightLevel++;
            if (currentHightLevel >= hightLevels.Length)
            {
                currentHightLevel = hightLevels.Length - 1;
            }
        }
        private void FixedUpdate()
        {
            if (rb.velocity.z < maxSpeed && !hasReached && !stoped)
                rb.AddForce(Vector3.forward * moveSpeed);
        }
        public void SetHight()
        {
            if (hasReached) return;
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
            var HightPoint = new Vector3(rb.position.x, hightLevels[currentHightLevel], rb.position.z);
            rb.position = Vector3.Lerp(rb.position, HightPoint, jumpForce * Time.deltaTime);
        }
        public void AddGravity()
        {
            if (hasReached) return;
            rb.velocity = new Vector3(0, gravity, rb.velocity.z);
            rb.position = new Vector3(rb.position.x, Mathf.Clamp(rb.position.y, -10, 100), rb.position.z);

            for (int i = 0; i < hightLevels.Length; i++)
            {
                if (rb.position.y <= hightLevels[i])
                {
                    currentHightLevel = i;
                    break;
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains(obstacleName))
            {
                rb.AddForce(-transform.forward * collisionForce, ForceMode.Impulse);
                //rb.AddForce(-transform.forward * collisionForce, ForceMode.VelocityChange);
            }
            if (other.transform.CompareTag("Finish"))
            {
                hasReached = true;
                rb.useGravity = true;
                rb.drag = 0.5f;
                onReachFinishPoint.Invoke();
            }
        }
    }
}