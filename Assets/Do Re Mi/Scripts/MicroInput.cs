using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MicroInput : MonoBehaviour
{
    public void SensChange(Single value)
    {
        sensitivity = value;
    }
    public void SmoothnessChange(Single value)
    {
        loudnessSmoothness = value;
    }
    public bool stopMic;
    public float sensitivity = 100f;
    public Single loudnessSmoothness = 0.1f;
    public float loudness = 0f;

    private AudioClip microphoneClip;
    private int sampleWindow = 128;

    public void SetupMic()
    {
        //if(stopMic) return;
        if (Microphone.devices.Length > 0)
        {
            microphoneClip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
            StartCoroutine(WaitForMicrophone());
        }
        else
        {
            Debug.LogError("No microphone device found!");
        }
    }

    IEnumerator WaitForMicrophone()
    {
        // Wait until the microphone is ready
        while (!(Microphone.GetPosition(null) > 0))
        {
            yield return null;
        }
        Debug.Log("Microphone started successfully.");
    }

    void Update()
    {
        if (!stopMic)
        {
            loudness = Mathf.Lerp(loudness, GetMicrophoneLoudness(), loudnessSmoothness*Time.deltaTime);
            //loudness = GetMicrophoneLoudness() * sensitivity;
            Debug.Log("Loudness: " + loudness);
        }
        // if (canJump && isGrounded && (loudness > jumpLoudnessThreshold || Input.GetKeyDown(KeyCode.Space)))
        // {
        //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //     canJump = false;
        //     Debug.Log("Jump triggered by voice or spacebar");
        // }
    }

    // void FixedUpdate()
    // {
    //     isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, whatIsGround);

    //     if (isGrounded)
    //     {
    //         canJump = true;
    //     }
    // }

    float GetMicrophoneLoudness()
    {
        if (microphoneClip == null) return 0;

        int micPosition = Microphone.GetPosition(null) - sampleWindow;
        if (micPosition < 0) return 0;

        float[] data = new float[sampleWindow];
        microphoneClip.GetData(data, micPosition);

        float total = 0f;
        foreach (float sample in data)
        {
            total += Mathf.Abs(sample);
        }

        return total / sampleWindow;
    }
}