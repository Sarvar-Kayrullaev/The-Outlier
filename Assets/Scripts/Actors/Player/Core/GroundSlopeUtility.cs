// GroundSlopeUtility.cs
// SlidingState, ActionController va AirborneState uchun umumiy qiyalik-aniqlash yordamchilari.

using Actors.Player.Scriptable;
using UnityEngine;

namespace Actors.Player.Core
{
    public static class GroundSlopeUtility
    {
        private static readonly RaycastHit[] HitBuffer = new RaycastHit[16];

        /// <summary>
        /// Character oyog'i ostidagi yuza normalini va qiyalik burchagini aniqlaydi.
        /// Trigger kolayderlar va player'ning o'z kolayderlari e'tiborga olinmaydi (aks holda ular
        /// "yer" bo'lib chiqib, normal doim yuqoriga qarab qolardi). Normal SphereCast'ning
        /// qirra/burchak normalidan emas, yuzaning haqiqiy normalidan olinadi.
        /// </summary>
        public static bool TryGetGroundSlope(
            CharacterController character,
            Transform transform,
            PlayerStats stats,
            out float slopeAngle,
            out Vector3 groundNormal)
        {
            return TryGetGroundSlope(character, transform, stats, out slopeAngle, out groundNormal, out _);
        }

        public static bool TryGetGroundSlope(
            CharacterController character,
            Transform transform,
            PlayerStats stats,
            out float slopeAngle,
            out Vector3 groundNormal,
            out Collider groundCollider)
        {
            var origin = transform.position + character.center;
            var castRadius = character.radius * 0.9f;
            var maxDistance = (character.height * 0.5f - character.radius)
                              + stats.slideGroundCheckExtra.Value + 0.1f;

            var count = Physics.SphereCastNonAlloc(
                origin, castRadius, Vector3.down, HitBuffer, maxDistance,
                stats.slideGroundMask, QueryTriggerInteraction.Ignore);

            var found = false;
            var best = default(RaycastHit);
            var bestDistance = float.MaxValue;

            for (var i = 0; i < count; i++)
            {
                var candidate = HitBuffer[i];
                if (candidate.collider == null) continue;
                if (candidate.collider.transform.IsChildOf(transform)) continue; // o'zining kolayderlari
                if (candidate.distance <= 0f) continue;                          // boshidan kesishgan - normal ishonchsiz
                if (candidate.distance >= bestDistance) continue;

                best = candidate;
                bestDistance = candidate.distance;
                found = true;
            }

            if (!found)
            {
                groundNormal = Vector3.up;
                slopeAngle = 0f;
                groundCollider = null;
                return false;
            }

            groundNormal = best.normal;

            // Haqiqiy yuza normali: hit nuqtasidan normal bo'ylab qisqa Raycast.
            var refineRay = new Ray(best.point + best.normal * 0.05f, -best.normal);
            if (best.collider.Raycast(refineRay, out var refined, 0.2f))
            {
                groundNormal = refined.normal;
            }

            slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
            groundCollider = best.collider;
            return true;
        }

        /// <summary>Yuzaning "pastga" (downhill) yo'nalishi - dunyo Y o'qini yuzaga proyeksiya qilish orqali topiladi. Tekis yuzada Vector3.zero.</summary>
        public static Vector3 GetDownhillDirection(Vector3 groundNormal)
        {
            return Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized;
        }
    }
}