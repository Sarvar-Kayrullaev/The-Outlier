namespace Actors.Player.Animation
{
    /// <summary>
    /// Baza yoki ustki animatsiya qatlamini (Layer/State transition) boshqaruvchi
    /// modullar uchun umumiy shartnoma.
    /// </summary>
    public interface IAnimationLayerController
    {
        void SyncWithMovement(float moveIntensity, bool isGrounded);
    }
}