using System;
using System.Collections.Generic;

namespace Data.Models.World
{
    [Serializable]
    public class GameData
    {
        public List<WorldModel> worlds = new List<WorldModel>();
        public int lastSelectedWorldID = 1;
    }
}