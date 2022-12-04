using System;
using UnityEngine;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;


namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        public InventoryData Inventory => _inventory;

        public IntProperty Hp = new IntProperty();

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}
