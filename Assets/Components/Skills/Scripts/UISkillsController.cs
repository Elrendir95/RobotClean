using System.Collections.Generic;
using System.Linq;
using Components.EventSystem;
using Components.SaveSystem;
using UnityEngine;

namespace Components.Skills
{
    public class UISkillsController : MonoBehaviour
    {
        [SerializeField] private UISkillCard skillCard;
        [SerializeField] private GameObject content;
        [SerializeField] private float cardOffset = 20f;

        private SaveData _saveData;
        private Dictionary<string, Skill> _skills = new();
        private Dictionary<string, UISkillCard> _skillCards = new();

        private int GetSkillLevelFromSaveData(string skillName)
        {
            // Look for the skill Level in the SaveData, return 0 if not found, the level otherwise
            return _saveData.skills.FirstOrDefault(s => s.Name == skillName)?.Level ?? 0;
        }

        private SaveSkill GetSkillFromSaveData(string skillName)
        {
            var skill = _saveData.skills.FirstOrDefault(s => s.Name == skillName);
            if (skill == null)
            {
                skill = new SaveSkill();
                skill.Name = skillName;
                skill.Level = 0;
            }
            return skill;
        }

        private void Start()
        {
            Events.BuySkill += BuySkill;

            _saveData = SaveService.Load();
            var skillList = ScriptableObjectDatabase.GetAll<Skill>();
            foreach (var skill in skillList)
            {
                skill.SetLevel(GetSkillLevelFromSaveData(skill.name));
                var card = Instantiate(skillCard, content.transform, false);
                card.Setup(skill);
                _skills.Add(skill.name,skill);
                _skillCards.Add(skill.name, card);
            }
        }

        public void BuySkill(string skillName)
        {
            var skillToImprove = ScriptableObjectDatabase.Get<Skill>(skillName);
            if (_saveData.electronicsComponents > skillToImprove.NextLevel.Cost)
            {
                var skill = GetSkillFromSaveData(skillName);
                _saveData.skills.Remove(skill);
                skill.Level++;
                _saveData.skills.Add(skill);
                _saveData.electronicsComponents -= skillToImprove.NextLevel.Cost;
                skillToImprove.SetLevel(skill.Level);
                SaveService.Save(_saveData);
                _skillCards[skillName].Setup(skillToImprove);
            }
        }
    }
}
