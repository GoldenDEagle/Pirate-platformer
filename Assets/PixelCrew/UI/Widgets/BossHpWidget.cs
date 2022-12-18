using PixelCrew.Components;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class BossHpWidget : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private ProgressBarWidget _hpBar;
        [SerializeField] private CanvasGroup _canvas;

        private float _maxHealth;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _maxHealth = _health.Health;
            _trash.Retain(_health._onChange.Subscribe(OnHpChanged));
            _trash.Retain(_health._onDie.Subscribe(HideUI));
        }

        private void SetAlpha(float alpha)
        {
            _canvas.alpha = alpha;
        }

        [ContextMenu("Show")]
        public void ShowUI()
        {
            this.LerpAnimated(0, 1, 1, SetAlpha);
        }

        [ContextMenu("Hide")]
        public void HideUI()
        {
            this.LerpAnimated(1, 0, 1, SetAlpha);
        }

        private void OnHpChanged(int hp)
        {
            _hpBar.SetProgress(hp / _maxHealth);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}