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

        private SaveData _saveData;
        private readonly Dictionary<string, UISkillCard> _skillCards = new();

        private void Start()
        {
            Events.BuySkill += BuySkill;

            _saveData = SaveService.Load();

            var skillList = ScriptableObjectDatabase.GetAll<Skill>();
            foreach (var skill in skillList)
            {
                // Initialize Skill levels from saved data
                skill.SetLevel(GetSkillLevelFromSaveData(skill.name));
                // Create Skill card for UI
                var card = Instantiate(skillCard, content.transform, false);
                // Setup the card
                card.Setup(skill);
                // Keep track of the card to update it later
                _skillCards.Add(skill.name, card);
            }
        }

        private void OnDestroy()
        {
            Events.BuySkill -= BuySkill;
        }

        private int GetSkillLevelFromSaveData(string skillName)
        {
            // Look for the skill Level in the SaveData, return 0 if not found, the level otherwise
            return _saveData.skills.FirstOrDefault(s => s.Name == skillName).Level;
        }

        private SaveSkill GetSkillFromSaveData(string skillName)
        {
            // Get the first skill matching the name from the save
            var skill = _saveData.skills.FirstOrDefault(s => s.Name == skillName);

            // If we did not find the skill, initialize the data
            if (string.IsNullOrEmpty(skill.Name))
            {
                skill.Name = skillName;
                skill.Level = 0;
            }

            return skill;
        }

        public void BuySkill(string skillName)
        {
            // Retrieve the reference data from the database
            var skillToImprove = ScriptableObjectDatabase.Get<Skill>(skillName);

            // Check if the player can afford the next level
            if (_saveData.electronicsComponents < skillToImprove.NextLevel.Cost) return;

            // Update the save data
            var skill = GetSkillFromSaveData(skillName);
            _saveData.skills.Remove(skill);
            skill.Level++;
            _saveData.skills.Add(skill);

            // Deduct the cost of the skill and save
            _saveData.electronicsComponents -= skillToImprove.NextLevel.Cost;
            SaveService.Save(_saveData);

            // Update runtime objects and trigger refresh UI
            skillToImprove.SetLevel(skill.Level);
            _skillCards[skillName].Setup(skillToImprove);
            Events.UpdateElectronicsUI?.Invoke(_saveData.electronicsComponents);
        }
    }
}
