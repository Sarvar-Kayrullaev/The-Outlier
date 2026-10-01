// ActionController.cs

using Actors.Player.Actions;
using Actors.Player.Actions.Parkour;
using Actors.Player.Modules;
using Actors.Player.Motion;
using Actors.Player.Scriptable;
using Interfaces;
using UnityEngine;

namespace Actors.Player.Core
{
    public class ActionController
    {
        public readonly Transform transform;
        public readonly Transform cameraParent;
        public readonly CharacterController characterController;
        public readonly PlayerManager manager;
        public readonly PlayerStats stats;

        public readonly PlayerActionContext actionContext;
        public readonly PlayerActionRunner actionRunner;
        public readonly PlayerMotor motor;
        public readonly ParkourDetector parkourDetector;
        public readonly PlayerCameraController cameraController;

        private IState _currentState;
        private bool slideRearmRequired;
        private float nextDebugLogTime;

        // Yuza modeli/floating-point xatosi tufayli 50° li rampa 49.99° chiqib qolmasligi uchun.
        private const float SlideAngleTolerance = 0.5f;

        public ActionController(PlayerManager manager)
        {
            this.manager = manager;
            stats = manager.playerStats;
            transform = manager.transform;
            cameraParent = manager.cameraParent;
            characterController = manager.characterController;

            actionRunner = manager.actionRunner;
            actionContext = new PlayerActionContext(manager, actionRunner);
            motor = new PlayerMotor(manager, actionContext);
            parkourDetector = new ParkourDetector(manager);
            cameraController = new PlayerCameraController(manager, manager.footstepManager);

            // Player tik qiyalikka (>= slideAutoTriggerAngle) jismonan chiqa olishi kerak, aks holda
            // CharacterController uni "devor" deb bloklaydi va sirpanish hech qachon boshlanmaydi.
            characterController.slopeLimit = stats.characterSlopeLimit.Value;

            ChangeState(new StandingState(this));
        }

        public void ChangeState(IState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
            LogSlideDebug();
        }

        /// <summary>
        /// Player slideAutoTriggerAngle (standart 50°) va undan tik qiyalikda tursa SlidingState'ga o'tkazadi.
        /// Sirpanish "tiqilib qolish" sababli tugagan bo'lsa, player tik qiyalikdan chiqmaguncha
        /// qayta boshlanmaydi (Standing <-> Sliding titrashining oldini oladi).
        /// </summary>
        /// <returns>SlidingState'ga o'tilgan bo'lsa true.</returns>
        public bool TryStartAutoSlide()
        {
            if (!GroundSlopeUtility.TryGetGroundSlope(characterController, transform, stats, out var slopeAngle, out _))
            {
                return false;
            }

            if (slopeAngle < stats.slideAutoTriggerAngle.Value - SlideAngleTolerance)
            {
                slideRearmRequired = false;
                return false;
            }

            if (slideRearmRequired) return false;

            ChangeState(new SlidingState(this, characterController.velocity));
            return true;
        }

        /// <summary>SlidingState tiqilib qolgani uchun tugaganda chaqiriladi.</summary>
        public void RequireSlideRearm()
        {
            slideRearmRequired = true;
        }

        private void LogSlideDebug()
        {
            if (!stats.slideDebugLog || Time.unscaledTime < nextDebugLogTime) return;
            nextDebugLogTime = Time.unscaledTime + 0.5f;

            var state = _currentState?.GetType().Name ?? "null";
            var hasGround = GroundSlopeUtility.TryGetGroundSlope(
                characterController, transform, stats, out var angle, out _, out var ground);

            if (hasGround)
            {
                Debug.Log($"[SlideDebug] state={state} ground='{ground.name}' layer={LayerMask.LayerToName(ground.gameObject.layer)} " +
                          $"angle={angle:F2} trigger>={stats.slideAutoTriggerAngle.Value - SlideAngleTolerance:F1} " +
                          $"rearmBlocked={slideRearmRequired} slopeLimit={characterController.slopeLimit:F0}");
                Debug.Log(
                    $"[SlideDebug] " +
                    $"PlayerStats='{stats.name}' " +
                    $"instanceID={stats.GetInstanceID()} " +
                    $"slideAutoTriggerAngle.BaseValue={stats.slideAutoTriggerAngle.BaseValue} " +
                    $"Value={stats.slideAutoTriggerAngle.Value}"
                );
            }
            else
            {
                Debug.Log($"[SlideDebug] state={state} NO GROUND FOUND (mask={stats.slideGroundMask.value}) " +
                          $"grounded={characterController.isGrounded} slopeLimit={characterController.slopeLimit:F0}");
            }
        }
    }
}