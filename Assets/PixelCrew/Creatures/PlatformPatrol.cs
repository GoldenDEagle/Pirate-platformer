using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class PlatformPatrol : Patrol
    {
        private Creature _creature;
        private Vector2 _direction = Vector2.one;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            _direction.y = 0f;

            while (enabled)
            {
                if (_creature.IsPlatformAhead())
                {
                    _creature.SetDirection(_direction);
                }
                else
                {
                    _direction *= (-1f);
                    _creature.SetDirection(_direction);
                    yield return null;
                }

                yield return null;
            }

        }
    }
}
