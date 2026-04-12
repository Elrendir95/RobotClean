using Components.EventSystem;
using Components.Skills;
using UnityEngine;

namespace Components.Collectible
{
    /// <summary>
    /// Standard Collectable Trash, that give back some life
    /// </summary>
    public class TrashCollectable : Collectable
    {
        [SerializeField] private float lifeBonus = 4f;

        private Skill _healthBonus;

        private void Start()
        {
            _healthBonus = ScriptableObjectDatabase.Get<Skill>("HealthSkill");
        }

        public override void OnCollect(GameObject collector)
        {
            Debug.Log($"Collected TrashCollectable: {collector.name} give {lifeBonus} + {_healthBonus.Value} life");
            Events.UpdateLife?.Invoke(lifeBonus + _healthBonus.Value);
            base.OnCollect(collector);
        }
    }
}
