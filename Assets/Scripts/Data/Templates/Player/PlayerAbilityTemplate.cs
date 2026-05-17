using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Templates.Player
{
    [Serializable]
    public class PlayerAbilityTemplate
    {
        [Header("General Info")]
        public int  Id;
        public string Name;
        [TextArea(3, 10)]
        public string Description;
        [Header("Visuals")]
        public Sprite Icon;
    }

    [CreateAssetMenu(fileName = "PlayerAbilityDatabase", menuName = "Player/Databases/Player Abilities")]
    public class PlayerAbilityDatabase: ScriptableObject
    {
        public List<PlayerAbilityTemplate> abilities = new List<PlayerAbilityTemplate>();
        
        public PlayerAbilityTemplate GetAbilityById(int id)
        {
            return abilities.Find(a => a.Id == id);
        }
    }
}