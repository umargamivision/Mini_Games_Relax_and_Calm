using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PopIt
{
    public class Popable : MonoBehaviour
    {
        public bool canPop;
        public bool hasPoped;
        public Collider2D collider2D;
        public UnityEvent onPop;
        private void OnEnable() 
        {
            collider2D = GetComponent<Collider2D>();
            Deactivate();
        }
        public void ResetPopable()
        {
            hasPoped = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        public void Activate()
        {
            collider2D.enabled = true;
            canPop = true;
        }
        public void Deactivate()
        {
            collider2D.enabled = false;
            canPop = false;
        }
        public void Pop()
        {
            if(hasPoped) return;
            hasPoped = true;
            onPop.Invoke();
        }
        private void OnMouseEnter() 
        {
            if(canPop) Pop();
        }
    }
}
