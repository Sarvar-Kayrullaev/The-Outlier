using UnityEngine;

namespace Actors.Player.Controller
{
    public class InputRotation : MonoBehaviour
    {
        [Header("Settings")]
        public float sensitivity = 0.05f; // Mobil uchun odatda pastroq qiymat yaxshi
        
        private Input inputMobile;
        private int rightFingerId = -1;
        private float screenCenterX;

        private void Awake()
        {
            // Ekranning markazini aniqlaymiz (faqat o'ng tomonni sezish uchun)
            inputMobile = Input.Instance;
            screenCenterX = Screen.width / 2;
        }

        void Update()
        {
            HandleTouchInput();
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
    }
}