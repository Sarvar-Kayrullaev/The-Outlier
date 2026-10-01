using Actors.Player.Actions;

namespace Actors.Player.Motion
{
    /// <summary>
    /// Kamera uchun har bir alohida effekt (Bob, Sway, Rotation) shu umumiy
    /// abstraksiyaga amal qiladi - PlayerCameraController ularni tartib bilan ishga tushiradi.
    /// </summary>
    public interface ICameraMotionEffect
    {
        void Tick(PlayerActionContext context);
    }
}