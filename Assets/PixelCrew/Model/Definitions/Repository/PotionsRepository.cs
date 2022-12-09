using System;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository
{
    [CreateAssetMenu(menuName = "Defs/PotionsRepository", fileName = "Potions")]
    public class PotionsRepository : DefRepository<PotionDef>
    {

    }

    [Serializable]
    public struct PotionDef : IHaveId
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private Effect _effect;
        [SerializeField] private float _value;
        [SerializeField] private float _time;
        public string Id => _id;
        public Effect Effect => _effect;
        public float Value => _value;
        public float Time => _time;
    }

    public enum Effect
    {
        AddHp,
        SpeedUp
    }
}