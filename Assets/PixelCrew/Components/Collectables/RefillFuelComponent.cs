using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class RefillFuelComponent : MonoBehaviour
    {
        public void Refill()
        {
            GameSession.Instance.Data.Fuel.Value = GameSession.Instance.StatsModel.GetValue(StatId.Fuel);
        }
    }
}