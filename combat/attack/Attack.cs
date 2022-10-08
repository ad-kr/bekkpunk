using Godot;
using System;

namespace ADKR.Game
{
    public class Attack
    {
        private readonly AttackOptions _options;
        private readonly Combatable[] _targets;

        public Attack(AttackOptions options, params Combatable[] targets)
        {
            _options = options;
            _targets = targets;
        }

        public void Execute(float delta = 1f)
        {
            GD.Randomize();
            foreach (Combatable target in _targets)
            {
                float damage = (float)GD.RandRange(_options.MinDamage, _options.MaxDamage);
                target.Health -= damage * delta;
                _options.OnHit?.Invoke(damage);
            }
        }
    }

    public struct AttackOptions
    {
        public float MinDamage { get; set; }
        public float MaxDamage { get; set; }
        public Action<float> OnHit { get; set; }
    }
}