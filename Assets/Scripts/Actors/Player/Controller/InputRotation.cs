using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

namespace Actors.Player.Controller
{
    public class InputRotation : MonoBehaviour
    {
        [Header("Settings")] public float sensitivity = 0.05f; // Mobil uchun odatda pastroq qiymat yaxshi

        private Input inputMobile;
        private int rightFingerId = -1;
        private float screenCenterX;
        private bool isDraggingInEditor = false;

        private void Awake()
        {
            // Ekranning markazini aniqlaymiz (faqat o'ng tomonni sezish uchun)
            inputMobile = Input.Instance;
            screenCenterX = Screen.width / 2;
        }

        void Update()
        {
#if UNITY_EDITOR
            TryWithEditorInput();
#else
            HandleTouchInput();
#endif
        }

        private void HandleTouchInput()
        {
            // Har freymda qiymatni nollashtirmaslik uchun, faqat o'zgarish bo'lganda yangilaymiz
            Vector2 inputDelta = Vector2.zero;

            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                Touch t = UnityEngine.Input.GetTouch(i);

                // 1. Yangi bosishni aniqlash
                if (t.phase == TouchPhase.Began)
                {
                    // Agar o'ng tomonda bo'lsa va hali boshqa barmoq band qilmagan bo'lsa
                    if (t.position.x > screenCenterX && rightFingerId == -1)
                    {
                        rightFingerId = t.fingerId;
                    }
                }

                // 2. Harakatni kuzatish (faqat o'ng barmoq uchun)
                if (t.fingerId == rightFingerId)
                {
                    if (t.phase == TouchPhase.Moved)
                    {
                        inputDelta = t.deltaPosition * sensitivity;
                    }

                    // 3. Barmoq ko'tarilganda ID ni bo'shatish
                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        rightFingerId = -1;
                    }
                }
            }
            
            // Tashqi classlar foydalanishi uchun qiymatni saqlaymiz
            inputMobile.rotateInput = inputDelta;
        }

        private void TryWithEditorInput()
        {
            Vector2 inputDelta = Vector2.zero;
            
            // Sichqoncha va Ekranni olish
            Mouse mouse = Mouse.current;
            if (mouse == null) return;

            Vector2 mousePosition = mouse.position.ReadValue();

            // 1. Sichqoncha chap tugmasi bosilganda
            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (mousePosition.x > screenCenterX)
                {
                    isDraggingInEditor = true;
                }
            }

            // 2. Sichqoncha ushlab turilganda va surilganda
            if (isDraggingInEditor)
            {
                if (mouse.leftButton.isPressed)
                {
                    // Yangi Input System'da sichqoncha delta qiymati
                    Vector2 mouseDelta = mouse.delta.ReadValue();
                    inputDelta = mouseDelta * sensitivity;
                }

                // 3. Tugma qo'yib yuborilganda
                if (mouse.leftButton.wasReleasedThisFrame)
                {
                    isDraggingInEditor = false;
                }
            }
            
            inputMobile.rotateInput = inputDelta;
        }
    }
}