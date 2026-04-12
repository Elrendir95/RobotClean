using System;
using UnityEngine.Serialization;

namespace Components.Skills
{
    [Serializable]
    public struct SkillLevel
    {
        public int Level;
        public float Value;
        public string DisplayValue;
        [FormerlySerializedAs("NextLevelCost")] public int Cost;
    }
}
