using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using UnityEngine.Events;
namespace Ommy.Tweener
{
    public class Tweener : MonoBehaviour
    {
        [Header("Add InvokeCompleteCallback in your last animation complete")]
        public UIAnimation levelSelectionAnimation;
        public Action onComplete;
        public void PlayAnimation(Action onComplete = null)
        {
            if(onComplete!=null)this.onComplete+=onComplete;
            levelSelectionAnimation.PlayAnimation();
        }
        public void InvokeCompleteCallback()
        {
            onComplete.Invoke();
        }
    }

    [Serializable]
    public class UIAnimation
    {
        #region VariableDeclaration
        public List<AnimationProps> animationProps;
        public List<AnimationProps> animationPropsWithSequance;
        public UnityEvent onSequencesComplete;
        public DG.Tweening.Sequence sequence;
        #endregion

        public void PlayAnimation()
        {
            Debug.Log("Animation Played");
            
            for(int i=0; i<animationProps.Count; i++)
            {
                var item = animationProps[i];
                foreach (var transform in item.itemTransforms)
                {
                    item.GetAnimationTweenToPlay(transform).Play().SetUpdate(true);
                }
            }
           sequence = DOTween.Sequence();
           for(int i=0; i<animationPropsWithSequance.Count; i++)
            {
                sequence.OnComplete(()=>{onSequencesComplete.Invoke();});
                var item = animationPropsWithSequance[i];
                //sequence.AppendInterval(1);
                foreach (var anim in item.itemTransforms)
                {
                    sequence.Append(item.GetAnimationTweenToPlay(anim)).SetUpdate(true);
                }
            }
        }
    }
    [Serializable]
    public class AnimationProps
    {
        #region VariableDeclaration
        public Tween tween;
        public bool activateBeforePlay = true;
        public DOTweenAnimation.AnimationType animationType;
        public Ease ease;
        public bool relative;
        public int loops = 1;
        public LoopType loopType = LoopType.Yoyo;
        public bool setInitialValue;
        public Vector3 initial, final;
        public float duration;
        public float delay;
        public UnityEvent onComplete;
        public Transform[] itemTransforms;
        #endregion

        public Tween GetAnimationTweenToPlay(Transform transform)
        {
            //itemTransform.gameObject.SetActive(!activateBeforePlay);
            switch (animationType)
            {
                case DOTweenAnimation.AnimationType.Scale:
                    //itemTransform.DOScale(Vector3.zero,0);
                    if (setInitialValue) transform.localScale = initial;
                    tween = transform.DOScale(final, duration).SetDelay(delay).SetEase(ease).SetRelative(relative).SetLoops(loops, loopType).OnComplete(()=>onComplete?.Invoke());
                    break;
                case DOTweenAnimation.AnimationType.Rotate:
                    if (setInitialValue) transform.eulerAngles = initial;
                    tween = transform.DORotate(final, duration).SetDelay(delay).SetEase(ease).SetRelative(relative).SetLoops(loops, loopType).OnComplete(()=>onComplete?.Invoke());
                    break;
                case DOTweenAnimation.AnimationType.LocalRotate:
                    if (setInitialValue) transform.localEulerAngles = initial;
                    tween = transform.DOLocalRotate(final, duration).SetDelay(delay).SetEase(ease).SetRelative(relative).SetLoops(loops, loopType).OnComplete(()=>onComplete?.Invoke());
                    break;
                case DOTweenAnimation.AnimationType.Move:
                    if (setInitialValue) transform.position = initial;
                    tween = transform.DOMove(final, duration).SetDelay(delay).SetEase(ease).SetRelative(relative).SetLoops(loops, loopType).OnComplete(()=>onComplete?.Invoke());
                    break;
                case DOTweenAnimation.AnimationType.LocalMove:
                    if (setInitialValue) transform.localPosition = initial;
                    tween = transform.DOLocalMove(final, duration).SetDelay(delay).SetEase(ease).SetRelative(relative).SetLoops(loops, loopType).OnComplete(()=>onComplete?.Invoke());
                    break;
            }
            return tween;
        }
    }
}