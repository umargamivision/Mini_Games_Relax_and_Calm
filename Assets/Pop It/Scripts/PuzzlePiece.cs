using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
namespace PopIt
{
    public class PuzzlePiece : MonoBehaviour
    {
        public bool hasJoined;
        public bool hasPicked;
        public float joinThreshould;
        public float joinSpeed;
        public Transform JoinPoint;
        public float goBackSpeed;
        public Transform initialPoint;
        public float scaleSpeed;
        public Vector3 initalSize, normalSize;
        public UnityEvent OnJoin;
        private void Start() 
        {
            //BackToInitialPoint();    
        }
        public void Setup()
        {
            hasJoined = false;
            hasPicked = false;
            GetComponent<Collider2D>().enabled=true;
            transform.localScale = Vector3.zero;
            transform.DOMove(initialPoint.position,0).SetEase(Ease.Linear);
            transform.DOScale(initalSize,scaleSpeed).SetEase(Ease.Linear);
        }
        private void OnMouseDrag()
        {
            if(hasJoined) return;

            OnPick();
            var mouseWorldPointPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPointPosition.x,mouseWorldPointPosition.y,0);
            CanJoin();
        }
        private void OnMouseUp() 
        {
            if(!hasJoined)BackToInitialPoint();
        }
        public void CanJoin()
        {
            if(Vector2.Distance(transform.position,JoinPoint.position) < joinThreshould)
            {
                JoinIt();
            }
        }
        public void JoinIt()
        {
            hasJoined = true;
            transform.DOMove(JoinPoint.position,joinSpeed).SetEase(Ease.Linear);
            OnJoin.Invoke();
        }
        public void OnPick()
        {
            hasPicked = true;
            transform.DOScale(normalSize,scaleSpeed).SetEase(Ease.Linear);
        }
        public void BackToInitialPoint()
        {
            hasPicked = false;
            transform.DOMove(initialPoint.position,goBackSpeed).SetEase(Ease.Linear);
            transform.DOScale(initalSize,scaleSpeed).SetEase(Ease.Linear);
        }
    }
}