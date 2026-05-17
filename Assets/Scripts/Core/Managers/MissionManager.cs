using Interfaces;
using UnityEngine;

namespace Core.Managers
{
    public class MissionManager : MonoBehaviour, ISingle
    {
        public bool IsMissionActive { get; private set; }

        public void StartMission() => IsMissionActive = true;
        public void EndMission() => IsMissionActive = false;
    }
}
