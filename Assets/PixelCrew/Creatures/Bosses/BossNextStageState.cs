using PixelCrew.Components;
using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Bosses
{
    public class BossNextStageState : StateMachineBehaviour
    {
        [ColorUsage(true, true)]
        [SerializeField] private Color _stageColor;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.TryGetComponent(out SpawnComponent spawner);
            spawner.MultipleSpawn();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.TryGetComponent(out CircularProjectileSpawner spawner);
            spawner.Stage++;

            animator.TryGetComponent(out ChangeLightsComponent changeLight);
            changeLight.SetColor(_stageColor);
        }
    }
}
