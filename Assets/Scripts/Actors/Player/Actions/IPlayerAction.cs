using System;

namespace Actors.Player.Actions
{
    /// <summary>
    /// Har qanday alohida o'yinchi harakati (Vault, Climb, kelajakda Takedown va h.k.)
    /// shu umumiy shartnomaga amal qiladi. PlayerActionRunner barcha harakatlarni
    /// shu interfeys orqali bir xil tarzda ishga tushiradi va yakunlaydi.
    /// </summary>
    public interface IPlayerAction
    {
        /// <summary>Harakat davomida CharacterController o'chirilishi kerakmi (Lerp/root-motion uchun).</summary>
        bool LocksCharacterController { get; }

        /// <summary>Harakatni boshlaydi. Yakunlanganda onComplete chaqiriladi.</summary>
        void Enter(PlayerActionContext context, Action onComplete);

        /// <summary>Har frame chaqiriladi (Coroutine ishlatmaydigan kelajakdagi harakatlar uchun ham mos).</summary>
        void Tick(PlayerActionContext context);

        /// <summary>Harakat muddatidan oldin to'xtatilsa (masalan, boshqa State majburan kirsa) chaqiriladi.</summary>
        void Cancel(PlayerActionContext context);
    }
}