using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrew.Creatures;

namespace PixelCrew
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void OnMovement(InputAction.CallbackContext context)  // Считывание кнопки и задание направления
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
            if (direction.y < 0)
                _hero.IsCrawling = true;
            else
                _hero.IsCrawling = false;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.StartThrowing();
            }

            if (context.canceled)
            {
                _hero.UseInventory();
            }
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.NextItem();
            }
        }

        public void OnUsePerk(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.UsePerk();
            }
        }

        public void OnToggleFlashlight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.ToggleFlashlight();
            }
        }
    }

}
