using System.Collections.Generic;
using UnityEngine;

namespace Data.Templates.Player
{
    [CreateAssetMenu(fileName = "InitialWorldConfig", menuName = "Config/Initial World Config")]
    public class InitialWorldConfig : ScriptableObject
    {
        [Header("Player Starter Data")]
        public Vector3 startPosition = new Vector3(0, 1f, 0);
        public float startRotationAngle = 0f;

        [Header("Economy Starter Data")]
        public int startBalance = 500;
        public int startSkillPoints = 3;
        
        [Header("Skill Starter Data")]
        public List<SkillNode> allGameSkills; // Loyihadagi barcha skill templatelari

        [Header("World Map Outposts")]
        // O'yindagi mavjud barcha outpostlar ID ro'yxati
        public List<int> defaultOutpostIds = new List<int> { 1, 2, 3, 4, 5 }; 
    }
}