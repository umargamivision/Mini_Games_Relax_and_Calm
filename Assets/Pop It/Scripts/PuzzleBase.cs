using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
namespace PopIt
{
    public class PuzzleBase : MonoBehaviour
    {
        public List<PuzzlePiece> puzzlePieces;
        public UnityEvent OnStart, OnComplete;
        public bool hasComplete => HasComplete();

        private void OnEnable() 
        {
            puzzlePieces.ForEach(f=>f.OnJoin.AddListener(OnJoinPiece));    
            Setup();
            OnStart.Invoke();
        }
        private void OnDisable() 
        {
            puzzlePieces.ForEach(f=>f.OnJoin.RemoveListener(OnJoinPiece));    
        }
        public void Setup()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1,1);
            puzzlePieces.ForEach(f=>f.Setup());
        }
        private bool HasComplete()
        {
            foreach (var item in puzzlePieces)
            {
                if(!item.hasJoined)
                {
                    return false;
                }
            }
            return true;
        }
        public void OnJoinPiece()
        {
            if(hasComplete)
            {
                OnComplete.Invoke();
                //puzzlePieces.ForEach(f=>f.GetComponent<Collider2D>().enabled=false);
                puzzlePieces.ForEach(f=>f.gameObject.SetActive(false));
            }
        }
    }
}