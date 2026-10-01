using Actors.Player.Core;
using Actors.Player.Scriptable;
using UnityEngine;

namespace Actors.Player.Actions
{
    /// <summary>
    /// IPlayerAction va harakat bilan bog'liq modullar (Motor, Camera) uchun umumiy
    /// bog'liqlik konteyneri (SOLID - Dependency Inversion). Har bir modul to'g'ridan-to'g'ri
    /// PlayerManager'ga emas, shu yengil Context'ga bog'lanadi.
    /// </summary>
    public class PlayerActionContext
    {
        public readonly Transform transform;
        public readonly CharacterController character;
        public readonly PlayerStats stats;
        public readonly PlayerActionRunner runner;

        /// <summary>Umumiy vertikal/harakat tezligi - Motor va Parkour harakatlari shu qiymatni o'qiydi/yozadi.</summary>
        public Vector3 velocity;

        /// <summary>Joriy frame uchun input asosidagi harakat intensivligi (0..1+). Camera effektlari shundan foydalanadi.</summary>
        public float moveIntensity;

        public PlayerActionContext(PlayerManager manager, PlayerActionRunner runner)
        {
            transform = manager.transform;
            character = manager.characterController;
            stats = manager.playerStats;
            this.runner = runner;
        }
    }
}