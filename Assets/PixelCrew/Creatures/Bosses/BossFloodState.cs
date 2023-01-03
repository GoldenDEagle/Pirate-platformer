using PixelCrew.Components.Audio;
using UnityEngine;

namespace PixelCrew.Creatures.Bosses
{
    public class BossFloodState : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.TryGetComponent(out FloodController floodController);
            floodController.StartFlooding();
            animator.TryGetComponent(out PlaySoundsComponent sounds);
            sounds.Play("Aggro");
        }
    }
}