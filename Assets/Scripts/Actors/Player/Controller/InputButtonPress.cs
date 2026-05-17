using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Actors.Player.Controller
{
    public class InputButtonPress : InputButtonBase
    {
        public bool cancelOnExit;
        public UnityEvent<bool> onPress;
        
        private Coroutine _pressCoroutine;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            _pressCoroutine = StartCoroutine(PressRoutine());
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            StopPressRoutine();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (cancelOnExit) StopPressRoutine();
        }
        
        private IEnumerator PressRoutine()
        {
            while (true)
            {
                onPress?.Invoke(true);
                yield return null;
            }
        }
        
        private void StopPressRoutine()
        {
            if (_pressCoroutine == null) return;
            
            StopCoroutine(_pressCoroutine);
            _pressCoroutine = null;
            onPress?.Invoke(false);
        }
    }
}
