using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using RedLightMiniGameSpace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
namespace DoReMiSpace
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        public class levelVoiceInfo
        {
            public float minLoudness;
            public float maxLoudness;
            public float sensitivity;
        }
        public bool voiceControll;
        public MicroInput microInput;
        public LevelCreator levelCreator;
        public levelVoiceInfo[] levelVoiceInfos;
        public bool stoped;
        public bool hasReached;
        public int currentLevelHight => levelCreator.currentPlatform.level;
        public int currentPlayerHight;
        public float[] hightLevels;
        public string obstacleName;
        public float collisionForce;
        public float jumpForce;
        public float moveSpeed;
        public float maxSpeed;
        public float gravity;
        public Rigidbody rb;
        [Header("Sounds")]
        public AudioSource soundSource;
        public List<AudioClip> audioClips;
        public UnityEvent onReachFinishPoint;
        public UnityEvent onComplete;
        public void StartGame(bool useVoiceControl)
        {
            if(useVoiceControl)microInput.SetupMic();
            voiceControll = useVoiceControl;
            stoped = false;
        }
        public void PlaySound()
        {
            soundSource.volume = 1;
            soundSource.clip = audioClips[currentPlayerHight-1];
            soundSource.Play();
        }
        public void Update()
        {
            if (voiceControll)
                AsistiveVoiceController();
                //VoiceController();
            else
                TapController();
        }
        public void VoiceController()
        {
            Debug.LogWarning("current platform hight: "+ currentLevelHight);
            Debug.LogWarning("current player hight: "+ currentPlayerHight);
            Debug.LogWarning("current sensitivity: "+ microInput.sensitivity);
            microInput.sensitivity = levelVoiceInfos[currentLevelHight].sensitivity;
            for (int i = 0; i < levelVoiceInfos.Length; i++)
            {
                if(microInput.loudness > levelVoiceInfos[i].minLoudness && microInput.loudness < levelVoiceInfos[i].maxLoudness)
                {
                    currentPlayerHight = i;
                }
            }
            if(microInput.loudness > 0) SetHight();
            else AddGravity();
        }
        public float minimumLoudnessRequire;
        public void OnSilderChange(Single value)
        {
            minimumLoudnessRequire = value;
        }
        public void AsistiveVoiceController()
        {
            Debug.LogWarning("minimumLoudnessRequire  : "+ minimumLoudnessRequire);
            //microInput.sensitivity = levelVoiceInfos[currentLevelHight].sensitivity;
            var clampedL = Mathf.Clamp(microInput.loudness, 0 , 1000);
            var normalizeLoudness = clampedL/1000;
            if(normalizeLoudness > minimumLoudnessRequire)
            {
                currentPlayerHight = 1+currentLevelHight;
                SetHight();
            }
            else
            {
                AddGravity();
            }
        }
        public void TapController()
        {
            if (Input.GetMouseButtonDown(0))
            {
                UpdateCurrentLevel();
                PlaySound();
            }
            if (Input.GetMouseButton(0))
                SetHight();
            else
            {
                AddGravity();
                soundSource.volume = math.lerp(soundSource.volume,0,0.1f);
            }
        }
        public void UpdateCurrentLevel()
        {
            currentPlayerHight++;
            if (currentPlayerHight >= hightLevels.Length)
            {
                currentPlayerHight = hightLevels.Length - 1;
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
            var HightPoint = new Vector3(rb.position.x, hightLevels[currentPlayerHight], rb.position.z);
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
                    currentPlayerHight = i;
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
            if (other.transform.name==("End Point"))
            {
                hasReached = true;
                rb.useGravity = true;
                rb.drag = 0.5f;
                onReachFinishPoint.Invoke();
            }
            if(other.transform.CompareTag("Finish"))
            {
                DoubleJump();
                if(completeDely==null)completeDely=StartCoroutine(GameCompleteDely());
            }
        }
        private void OnTriggerEnter(Collider other) 
        {
            if(other.name.Contains("Pass"))
            {
                var platform = other.GetComponentInParent<PlatformDoReMe>();
                if(platform != null)
                {
                    platform.OnPlayerPassed();
                }
            }
            if(other.transform.CompareTag("Finish"))
            {
                DoubleJump();
                //if(completeDely==null)completeDely=StartCoroutine(GameCompleteDely());
            }
        }
        public void DoubleJump()
        {
            rb.isKinematic=false;
            rb.useGravity = true;
            //rb.AddForce(Vector3.down*10, ForceMode.Impulse);
            enabled = false;
            //rb.DOJump(transform.position, 10, 1,1);
            //enabled = false;
        }
        public Coroutine completeDely;
        public IEnumerator GameCompleteDely()
        {
            yield return new WaitForSeconds(3);
            onComplete.Invoke();
        }
    }
}