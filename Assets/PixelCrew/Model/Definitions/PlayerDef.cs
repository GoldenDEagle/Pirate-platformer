using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _inventorySize;
        [SerializeField] private StatDef[] _stats;
        public int InventorySize => _inventorySize;
        public StatDef[] Stats => _stats;

        public StatDef GetStat(StatId id) => _stats.FirstOrDefault(x => x.Id == id);
    }
}