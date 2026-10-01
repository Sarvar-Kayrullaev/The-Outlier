using Interfaces;
using UnityEngine;

namespace Data.Templates.Player
{
    [CreateAssetMenu(fileName = "NewSkillNode", menuName = "SkillTree/Skill Node")]
    public class SkillNode : ScriptableObject
    {
        public string id;
        public string skillName;
        [TextArea] public string description;
        [Header("Behavior")]
        public SkillEffectDefinition effect; // ScriptableObject - "nima qiladi"
        
        [Header("Visuals")]
        public Sprite lockedIcon;
        public Sprite unlockableIcon;
        public Sprite unlockedIcon;
        public int cost = 1;

        [Header("Tree Structure")]
        public bool isRoot; 
        public SkillNode parentNode;
        public AbilityNodeDirection directionFromParent = AbilityNodeDirection.Right;
    }
}