using Godot;

namespace ADKR.Game
{
    public class EnemyChargeState : CharacterState<Enemy>
    {
        private bool _isCancelled = false;

        public async override void Start()
        {
            base.Start();
            _isCancelled = false;

            Char.Sprite.Frame = 0;
            Char.Sprite.Animation = "charge";
            Char.Sprite.Playing = true;

            await Char.ToSignal(Char.Sprite, "animation_finished");

            if (!_isCancelled && !Char.IsDead) Char.State = Char.AttackState;
        }

        public override void Update(double delta)
        {
            base.Update(delta);

            if (Char.Target == null || Char.Target.IsDead)
            {
                Char.State = Char.DeactivateState;
                return;
            }

            float dist = Char.Position.DistanceTo(Char.Target.Position);
            Char.IsFlipped = IsFlipped(Char.Position - Char.Target.Position);

            if (dist <= Char.AttackRadius) return;

            Char.State = Char.RunState;
            _isCancelled = true;
        }

        private bool IsFlipped(Vector2 dir)
        {
            if (dir.x == 0) return Char.Sprite.FlipH;
            if (dir.x > 0f) return false;
            return true;
        }
    }
}