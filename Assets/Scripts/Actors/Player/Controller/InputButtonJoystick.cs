using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Actors.Player.Controller
{
    public class InputButtonJoystick : InputButtonBase
    {
        [Header("Joystick Parts")]
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;

        [Header("Settings")]
        [SerializeField] private float handleLimit = 1f;
        [Range(1f, 25f)] 
        [SerializeField] private float smoothSpeed = 15f; // Higher = more responsive, Lower = heavier/smoother

        public UnityEvent<Vector2> onMove;
        
        private Vector2 rawInput = Vector2.zero; // Where the finger is
        private Vector2 smoothedInput = Vector2.zero; // The interpolated value
        private bool isDragging = false;

        private void Update()
        {
            // Calculate the target: rawInput when touching, zero when released
            Vector2 targetInput = isDragging ? rawInput : Vector2.zero;

            // Interpolate the smoothedInput towards the target
            smoothedInput = Vector2.MoveTowards(smoothedInput, targetInput, smoothSpeed * Time.deltaTime);

            // Update the handle position visually based on smoothed input
            UpdateHandleVisuals(smoothedInput);

            // Send the smooth value to listeners
            onMove?.Invoke(smoothedInput);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            OnDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out var pos))
            {
                // Normalize position based on background size
                Vector2 halfSize = background.sizeDelta / 2f;
                Vector2 inputVector = new Vector2(pos.x / halfSize.x, pos.y / halfSize.y);

                // Clamp magnitude to 1 for circular movement
                rawInput = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
            rawInput = Vector2.zero;
        }

        private void UpdateHandleVisuals(Vector2 inputState)
        {
            float radius = background.sizeDelta.x / 2f;
            handle.anchoredPosition = inputState * (radius * handleLimit);
        }
    }
}