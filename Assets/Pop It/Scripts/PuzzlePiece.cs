using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
using JetBrains.Annotations;
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
            gameObject.SetActive(true);
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
            GetComponent<SpriteRenderer>().sortingOrder = 3;
            hasJoined = true;
            transform.DOMove(JoinPoint.position,joinSpeed).SetEase(Ease.Linear);
            GetComponent<Collider2D>().enabled=false;
            OnJoin.Invoke();
        }
        int defOrder;
        public void OnPick()
        {
            GetComponent<SpriteRenderer>().sortingOrder = 5;
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