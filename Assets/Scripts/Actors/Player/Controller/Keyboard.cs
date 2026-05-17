using UnityEngine;

namespace Actors.Player.Controller
{
    public class Keyboard: MonoBehaviour
    {
        public bool Activate = false;
        private Input inputMobile;

        private void Awake()
        {
            inputMobile = Input.Instance;

            if (Activate)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void Update()
        {
            if (!Activate) return;
            var horizontal = UnityEngine.Input.GetAxis("Horizontal");
            var vertical = UnityEngine.Input.GetAxis("Vertical");
            inputMobile.moveInput = new Vector2(horizontal, vertical);
            
            float mouseX = UnityEngine.Input.GetAxis("Mouse X");
            float mouseY = UnityEngine.Input.GetAxis("Mouse Y");
            inputMobile.rotateInput =  new Vector2(mouseX, mouseY);
        }
    }
}