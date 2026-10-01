using System;
using System.Collections.Generic;

namespace Data.Models.World
{
    [Serializable]
    public class WorldModel
    {
        public int worldID;
        public PlayerModel player;
        public FundModel funds;
        public List<PlayerSkillModel> skills;
        public List<OutpostModel> outposts;
    }
}