using UnityEngine;

namespace Components.Skills
{
    [CreateAssetMenu(menuName = "Components/Skill")]
    public class Skill : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private int level;
        [SerializeField] private SkillLevel[] skillLevels;

        public void SetLevel(int setLevel)
        {
            if (setLevel > skillLevels.Length) return;
            level = setLevel;
        }

        public string DisplayName => displayName;
        public string Description => description;
        public int Level => level;
        public float Value => skillLevels[level].Value;
        public string DisplayValue => skillLevels[level].DisplayValue;
        public SkillLevel NextLevel => (level + 1 < skillLevels.Length) ? skillLevels[level + 1] : new SkillLevel();
    }
}
