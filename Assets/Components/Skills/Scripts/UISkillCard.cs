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


        public void Setup(Skill skill)
        {
            skillName.text = skill.name;
            skillDescription.text = skill.Description;
            skillLevel.text = $"Level : {skill.Level}";
            skillValue.text = $"Bonus : {skill.Value}";
            nextSkillLevel.text = $"Next Level: {skill.NextLevel.Level}";
            nextSkillValue.text = $"Next Bonus: {skill.NextLevel.Value}";
            cost.text = $"Cost: {skill.NextLevel.Cost}";
            nextInfoWindow.SetActive(skill.NextLevel.Cost > 0);
        }

        public void BuySkill()
        {
            Events.BuySkill(skillName.text);
        }
    }
}
