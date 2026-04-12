using UnityEngine;

namespace Components.Skills
{
    [CreateAssetMenu(menuName = "Components/Skill")]
    public class Skill : ScriptableObject
    {
        [SerializeField] private string description;
        [SerializeField] private int level;
        [SerializeField] private SkillLevel[] skillLevels;

        public void SetLevel(int setLevel)
        {
            if (setLevel > skillLevels.Length) return;
            level = setLevel;
        }

        public string Description => description;
        public int Level => level;
        public int Value => skillLevels[level].Value;
        public SkillLevel NextLevel => (level + 1 < skillLevels.Length) ? skillLevels[level + 1] : new SkillLevel();
    }
}
