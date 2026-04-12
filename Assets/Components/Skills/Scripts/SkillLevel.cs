using System;
using UnityEngine.Serialization;

namespace Components.Skills
{
    [Serializable]
    public struct SkillLevel
    {
        public int Level;
        public int Value;
        [FormerlySerializedAs("NextLevelCost")] public int Cost;
    }
}
