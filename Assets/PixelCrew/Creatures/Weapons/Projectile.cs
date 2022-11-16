using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        protected override void Start()
        {
            base.Start();

            var force = new Vector2(Direction * Speed, 0);        // Движение для dynamic rigidbody
            Rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        //private void FixedUpdate()
        //{
        //    var position = _rigidbody.position;       // Движение для kinematic rigidbody
        //    position.x += _direction * _speed;
        //    _rigidbody.MovePosition(position);
        //}
    }
}
