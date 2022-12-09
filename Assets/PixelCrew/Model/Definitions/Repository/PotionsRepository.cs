using System;
using System.Collections;
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
        [SerializeField] private float _value;
        [SerializeField] private float _time;
        public string Id => _id;
        public float Value => _value;
        public float Time => _time;
    }
}