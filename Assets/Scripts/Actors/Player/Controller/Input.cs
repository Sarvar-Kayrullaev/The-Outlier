using UnityEngine;

namespace Actors.Player.Controller
{
    public class Input : MonoBehaviour
    {
        
        public Vector2 moveInput
        {
            get; 
            set;
        }
        public Vector2 rotateInput
        {
            get; 
            set;
        }
        public bool jumpInput
        {
            get
            {
                if (!_jumpInput) return false; // Reset value after use
                _jumpInput = false;
                return true;
            }
            set => _jumpInput = value;
        }
        public bool shootInput
        {
            get
            {
                if (!_shootInput) return false; // Reset value after use
                _shootInput = false;
                return true;
            }
            set => _shootInput = value;
        }

        // YANGI: crouch/slide tugmasi. jumpInput bilan bir xil "bir marta iste'mol qilish" naqshi -
        // StandingState/CrouchingState bir marta o'qiganidan keyin qiymat avtomatik false'ga qaytadi,
        // shuning uchun tugma bosib turilganda ham SlidingState/CrouchingState orasida "chayqalish" bo'lmaydi.
        public bool crouchInput
        {
            get
            {
                if (!_crouchInput) return false;
                _crouchInput = false;
                return true;
            }
            set => _crouchInput = value;
        }
        
        public static Input Instance;
        
        #region Private Members
        private bool _jumpInput;
        private bool _shootInput;
        private bool _crouchInput;

        private Input()
        {
            Instance = this;
        }
        #endregion
    }
}