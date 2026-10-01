// ParkourType.cs
// YANGI FAYL
 
namespace Actors.Player.Core
{
    /// <summary>
    /// PlayerActions.CheckParkourObstacle() aniqlagan to'siq turi.
    /// </summary>
    public enum ParkourType
    {
        None,
        Vault,  // Belgacha bo'lgan past to'siq - sakrab o'tiladi
        Climb   // Belidan baland to'siq - tirmashib chiqiladi
    }
}