using UnityEngine;
using PixelCrew.Model.Definitions.Repository;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private ItemsRepository _items;
        [SerializeField] private ThrowablesRepository _throwableItems;
        [SerializeField] private PotionsRepository _potions;
        [SerializeField] private PerksRepository _perks;
        [SerializeField] private PlayerDef _player;

        public ItemsRepository Items => _items;
        public ThrowablesRepository Throwable => _throwableItems;
        public PotionsRepository Potions => _potions;
        public PerksRepository Perks => _perks;
        public PlayerDef Player => _player;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}