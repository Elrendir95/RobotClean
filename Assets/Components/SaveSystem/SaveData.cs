using System;
using System.Collections.Generic;

namespace Components.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public int highScore;
        public int electronicsComponents;
        public List<SaveSkill> skills = new ();
    }
}
