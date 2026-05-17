using System;
using System.Collections;
using UI.Widgets;
using UnityEngine;
using UnityEngine.Events;

namespace Handlers
{
    public static class AnimationHandler
    {
        public static void ScaleY(this MonoBehaviour mono, RectTransform targetRectTransform, float targetValue, float animationDuration, Action onComplete = null)
        {
            mono.StartCoroutine(Animate());
            return;

            IEnumerator Animate()
            { 
                var startScaleY = targetRectTransform.localScale.y;
                var elapsedTime = 0f;
            
                while (elapsedTime < animationDuration)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    var t = elapsedTime / animationDuration;
                    var newScale = Mathf.Lerp(startScaleY, targetValue, t);
                
                    var currentScale = targetRectTransform.localScale;
                    targetRectTransform.localScale = new Vector3(currentScale.x, newScale, currentScale.z);
                
                    yield return null;
                }
                var finalScale = targetRectTransform.localScale;
                targetRectTransform.localScale = new Vector3(finalScale.x, targetValue, finalScale.z);
                onComplete?.Invoke();
            }
        }
        public static void Fade(this MonoBehaviour mono, CanvasGroup canvasGroup, float targetValue, float animationDuration, Action onComplete = null)
        {
            mono.StartCoroutine(Animate());
            return;

            IEnumerator Animate()
            { 
                var startFade = canvasGroup.alpha;
                var elapsedTime = 0f;
            
                while (elapsedTime < animationDuration)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    canvasGroup.alpha =  Mathf.Lerp(startFade, targetValue, elapsedTime / animationDuration);
                    yield return null;
                }

                canvasGroup.alpha = targetValue;
                onComplete?.Invoke();
            }
        }
    }
}
