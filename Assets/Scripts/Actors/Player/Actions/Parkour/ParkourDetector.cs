// FILE: Assets/Scripts/Actors/Player/Actions/Parkour/ParkourDetector.cs
using Actors.Player.Core;
using Actors.Player.Scriptable;
using UnityEngine;

namespace Actors.Player.Actions.Parkour
{
    /// <summary>
    /// Oldinda parkour qilsa bo'ladigan to'siq bor-yo'qligini va uning turini (Vault/Climb)
    /// aniqlaydi. Harakatning o'zini bajarmaydi - faqat aniqlash mas'uliyatini oladi
    /// (Single Responsibility). Natija VaultAction/ClimbAction'ga uzatiladi.
    /// </summary>
    public class ParkourDetector
    {
        private readonly Transform transform;
        private readonly PlayerStats stats;
        private readonly PlayerManager manager;

        public ParkourDetector(PlayerManager manager)
        {
            this.manager = manager;
            transform = manager.transform;
            stats = manager.playerStats;
        }

        public bool CheckObstacle(out ParkourType type, out Vector3 handlingBodyPosition, out Vector3 targetPosition)
        {
            type = ParkourType.None;
            targetPosition = Vector3.zero;
            handlingBodyPosition = Vector3.zero;
            var feetPosition = transform.position;
            var wallCheckOrigin = feetPosition + Vector3.up * stats.parkourCheckHeight.Value;

            if (!Physics.Raycast(wallCheckOrigin, transform.forward, out var wallHit, stats.parkourCheckDistance.Value, stats.parkourObstacleMask))
            {
                return false;
            }

            var topCheckOrigin = wallHit.point + transform.forward * 0.15f + Vector3.up * stats.climbHeightMax.Value;
            if (!Physics.Raycast(topCheckOrigin, Vector3.down, out var topHit, stats.climbHeightMax.Value + 0.5f, stats.parkourObstacleMask))
            {
                return false;
            }

            var obstacleHeight = topHit.point.y - feetPosition.y;

            Vector3 handlingPosition = wallHit.point + Vector3.up * obstacleHeight;
            handlingBodyPosition = handlingPosition - (transform.forward * manager.backOffset) - (transform.up * manager.downOffset);

            if (obstacleHeight < stats.vaultHeightMin.Value)
            {
                return false;
            }

            if (obstacleHeight <= stats.vaultHeightMax.Value)
            {
                type = ParkourType.Vault;

                var landingProbeOrigin = topHit.point + transform.forward * stats.vaultForwardOffset.Value + Vector3.up * 0.5f;
                targetPosition = Physics.Raycast(landingProbeOrigin, Vector3.down, out var landingHit, 3f)
                    ? landingHit.point
                    : new Vector3(topHit.point.x, feetPosition.y, topHit.point.z) + transform.forward * stats.vaultForwardOffset.Value;

                return true;
            }

            if (obstacleHeight <= stats.climbHeightMax.Value)
            {
                type = ParkourType.Climb;
                targetPosition = topHit.point + Vector3.up * 0.05f;
                return true;
            }

            return false;
        }
    }
}