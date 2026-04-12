using Components.EventSystem;
using TMPro;
using UnityEngine;

namespace Components.Skills
{
    public class UISkillCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text skillName;
        [SerializeField] private TMP_Text skillDescription;
        [SerializeField] private TMP_Text skillLevel;
        [SerializeField] private TMP_Text skillValue;
        [SerializeField] private TMP_Text nextSkillLevel;
        [SerializeField] private TMP_Text nextSkillValue;
        [SerializeField] private TMP_Text cost;
        [SerializeField] private GameObject nextInfoWindow;

        private Skill _skill;

        public void Setup(Skill skill)
        {
            _skill = skill;
            skillName.text = skill.DisplayName;
            skillDescription.text = skill.Description;
            skillLevel.text = $"Level : {skill.Level}";
            skillValue.text = $"Bonus : {skill.DisplayValue}";
            nextSkillLevel.text = $"Next Level: {skill.NextLevel.Level}";
            nextSkillValue.text = $"Next Bonus: {skill.NextLevel.DisplayValue}";
            cost.text = $"Cost: {skill.NextLevel.Cost}";
            nextInfoWindow.SetActive(skill.NextLevel.Cost > 0);
        }

        public void BuySkill()
        {
            Events.BuySkill(_skill.name);
        }
    }
}
