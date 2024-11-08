using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCameraFollow : MonoBehaviour
{
    public float smoothness;
    public Transform target;
    private void LateUpdate() 
    {
        var targetScreenPoint = Camera.main.WorldToScreenPoint(target.position);
        if(targetScreenPoint.y>Screen.height/2)
        {
            var cameraTargetPoint = new Vector3(transform.position.x, target.position.y , transform.position.z);    
            transform.position = Vector3.Lerp(transform.position, cameraTargetPoint,smoothness*Time.deltaTime);
        }
    }
}
