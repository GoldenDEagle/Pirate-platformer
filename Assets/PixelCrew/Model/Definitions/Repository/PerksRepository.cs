using System;
using UnityEngine;
using PixelCrew.Model.Definitions.Repository.Items;

namespace PixelCrew.Model.Definitions.Repository
{
    [CreateAssetMenu(menuName = "Defs/PerksRepository", fileName = "Perks")]
    public class PerksRepository : DefRepository<PerkDef>
    {
    }

    [Serializable]
    public struct PerkDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _info;
        [SerializeField] private ItemWithCount _price;
        [SerializeField] private float _cooldown;

        public string Id => _id;
        public Sprite Icon => _icon;
        public string Info => _info;
        public ItemWithCount Price => _price;
        public float Cooldown => _cooldown;
    }
}