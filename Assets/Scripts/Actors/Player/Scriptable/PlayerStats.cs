// FILE: Assets/Scripts/Actors/Player/Scriptable/PlayerStats.cs
using System;
using Other;
using UnityEngine;

namespace Actors.Player.Scriptable
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
    [Serializable]
    public class PlayerStats : ScriptableObject
    {
        [Header("Movement")]
        public ModifiableStat walkSpeed = new(5f);
        public ModifiableStat jumpHeight = new(2f);
        public ModifiableStat gravity = new(-9.81f);

        [Header("Camera - Input")]
        public ModifiableStat mouseSensitivity = new(10f);
        public ModifiableStat mouseVerticalSensitivity = new(10f);
        public ModifiableStat mouseHorizontalSensitivity = new(20f);

        [Header("Camera - Sway")]
        public ModifiableStat swaySpeed = new(0.8f);
        public ModifiableStat swayAmount = new(0.3f);

        [Header("Camera - Bob")]
        public ModifiableStat bobSpeedMultiplier = new(1.5f);
        public ModifiableStat bobAmount = new(0.1f);
        public ModifiableStat defaultCameraHeight = new(0.8f);

        [Header("Parkour - Detection")]
        [Tooltip("To'siqni aniqlash uchun Raycast qaysi balandlikdan (oyoq ostidan) otilishi (ko'krak balandligi)")]
        public ModifiableStat parkourCheckHeight = new(1f);
        [Tooltip("Oldinga qarab to'siqni aniqlash uchun Raycast masofasi")]
        public ModifiableStat parkourCheckDistance = new(0.8f);
        [Tooltip("Faqat shu Layer'lardagi obyektlar parkour to'sig'i sifatida hisoblanadi")]
        public LayerMask parkourObstacleMask = ~0;

        [Header("Parkour - Low Vault (Belgacha to'siq)")]
        [Tooltip("Vault uchun minimal to'siq balandligi (m)")]
        public ModifiableStat vaultHeightMin = new(0.3f);
        [Tooltip("Vault uchun maksimal to'siq balandligi (m). Shundan baland bo'lsa - Climb hisoblanadi")]
        public ModifiableStat vaultHeightMax = new(1.2f);
        [Tooltip("Vault paytida to'siqdan naryga qarab qo'shimcha masofa (qo'nish nuqtasi)")]
        public ModifiableStat vaultForwardOffset = new(0.6f);
        [Tooltip("Vault harakati necha soniyada bajarilishi")]
        public ModifiableStat vaultDuration = new(0.45f);

        [Header("Parkour - High Climb (Belidan baland to'siq)")]
        [Tooltip("Climb uchun maksimal to'siq balandligi (m). Shundan baland bo'lsa - parkour ishlamaydi")]
        public ModifiableStat climbHeightMax = new(2.2f);
        [Tooltip("Tirmashib chiqish tezligi (m/s) - masofaga qarab davomiylik shundan hisoblanadi")]
        public ModifiableStat climbSpeed = new(3f);
        [Tooltip("climbSpeed 0 yoki manfiy bo'lganda ishlatiladigan zaxira davomiylik (soniya)")]
        public ModifiableStat climbDuration = new(0.6f);

        // ==================================================================
        // SLIDING (Sirpanish)
        // ==================================================================

        [Header("Sliding - Kirish Shartlari")]
        [Tooltip("O'yinchi crouch/slide tugmasini bosganda SlidingState'ga kirishi uchun minimal " +
                 "gorizontal tezlik (m/s). Shundan past tezlikda oddiy CrouchingState'ga o'tiladi.")]
        public ModifiableStat slideMinEntrySpeed = new(4f);

        [Tooltip("Qiyalik shu burchakka yetganda (yoki oshganda) o'yinchi avtomatik SlidingState'ga o'tadi. " +
                 "Standart: 50°.")]
        public ModifiableStat slideAutoTriggerAngle = new(50f);

        [Tooltip("Faqat shu Layer'lardagi kolayderlar sirpanish uchun 'yer/qiyalik' sifatida hisoblanadi.")]
        public LayerMask slideGroundMask = ~0;

        [Tooltip("Qiyalik normalini aniqlash uchun pastga yo'naltirilgan SphereCast'ning qo'shimcha masofasi.")]
        public ModifiableStat slideGroundCheckExtra = new(0.3f);

        [Tooltip("CharacterController.slopeLimit o'yin boshida shu qiymatga o'rnatiladi. Player tik qiyalikka " +
                 "(masalan 50°) jismonan chiqib, sirpanish boshlana olishi uchun slideAutoTriggerAngle'dan " +
                 "katta bo'lishi kerak. 90°dan kichik bo'lsin.")]
        public ModifiableStat characterSlopeLimit = new(89f);

        [Header("Sliding - Debug")]
        [Tooltip("Yoqilsa, har 0.5 soniyada Console'ga joriy holat, yer kolayderi, qiyalik burchagi va " +
                 "slopeLimit yoziladi (sirpanish nega boshlanmayotganini aniqlash uchun).")]
        public bool slideDebugLog = true;

        [Header("Sliding - Fizika")]
        [Tooltip("Qiyalik bo'ylab tezlanish = |gravity| * sin(burchak) * shu ko'paytiruvchi.")]
        public ModifiableStat slopeAccelerationMultiplier = new(1f);

        [Tooltip("Sirpanishning maksimal tezligi (m/s).")]
        public ModifiableStat slideMaxSpeed = new(14f);

        [Header("Sliding - Boshqaruv")]
        [Tooltip("Sirpanish paytida input bilan harakatlanish tezligi = walkSpeed * shu qiymat (0.1 = 10%). " +
                 "Bu sirpanish tezligiga ta'sir qilmaydi.")]
        public ModifiableStat slideControlSpeedMultiplier = new(0.1f);

        [Header("Sliding - Chiqish")]
        [Tooltip("Qiyalik shu burchakdan past bo'lganda sirpanish to'xtatila boshlanadi. Standart: 15°.")]
        public ModifiableStat slideExitAngleThreshold = new(15f);

        [Tooltip("To'xtatish boshlangandan keyin sirpanish tezligi nolga tushishi uchun QAT'IY vaqt (soniya). " +
                 "SmoothStep ishlatiladi, shuning uchun to'xtash yumshoq, lekin cho'zilmaydi.")]
        public ModifiableStat slideExitSmoothDuration = new(0.35f);

        [Tooltip("Sirpanish tezligi shu qiymatdan past bo'lsa (m/s): silliq to'xtash paytida sirpanish tugadi " +
                 "deb hisoblanadi; oddiy sirpanish paytida esa 'harakatsiz qolish' aniqlanadi.")]
        public ModifiableStat slideStopSpeedThreshold = new(0.3f);

        [Tooltip("Sirpanish shu vaqt (soniya) davomida harakatsiz bo'lib qolsa (to'siqqa urilgan va h.k.), " +
                 "darhol oddiy yurish holatiga (StandingState) o'tiladi.")]
        public ModifiableStat slideStuckDetectTime = new(0.1f);
    }
}