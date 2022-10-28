using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{

    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTranform;

        public void Teleport(GameObject target)
        {
            target.transform.position = _destTranform.position;
        }

    }
}