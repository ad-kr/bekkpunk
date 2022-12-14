using Godot;
using System;

namespace ADKR.Game
{
    public partial class Player : Human
    {

        public static Player Instance { get; set; }

        private Weapon _equippedWeapon = null;

        public Vector2 PrevDir { get; set; } = Vector2.Zero;

        public Weapon EquippedWeapon
        {
            get => _equippedWeapon;
            set
            {
                _equippedWeapon?.QueueFree();
                _equippedWeapon = value;
                _equippedWeapon.Player = this;
                _equippedWeapon.TopHand = TopHand;
                _equippedWeapon.BottomHand = BottomHand;
                TopHand.HandSprite.AddChild(value);
            }
        }

        public Player()
        {
            if (Instance != null)
            {
                SkinID = Instance.SkinID;
                HairID = Instance.HairID;
                ClothesID = Instance.ClothesID;
            }
            Instance = this;
        }

        public override async void _Ready()
        {
            base._Ready();
            MaxHealth = 1000;
            Health = 1000;
            State = new PlayerEmptyState();
            Faction = Faction.Robot;

            if (IsInstanceValid(HealthBar.Instance)) HealthBar.Instance?.SetMinMax(0, (int)MaxHealth);
            if (IsInstanceValid(HealthBar.Instance)) HealthBar.Instance?.SetValue(Health);

            await ToSignal(GetTree().CreateTimer(1f), "timeout");
        }

        public override void _Input(InputEvent e)
        {
            base._Input(e);
            if (EquippedWeapon != null && e.IsActionPressed("attack") && State is not PlayerEmptyState && State is not PlayerWalkTowardsState)
            {
                if (State is PlayerAttackState) return;

                float dirLength = InputHandler.GetJoyDir().Length();
                PrevDir = dirLength <= 0f ? PrevDir : InputHandler.GetJoyDir();
                State = new PlayerAttackState(PrevDir);
            }
        }

        public override void OnHealthChange(float health, float prevHealth)
        {
            base.OnHealthChange(health, prevHealth);
            if (IsInstanceValid(HealthBar.Instance)) HealthBar.Instance?.SetValue(health);
        }

        public override async void Die()
        {
            base.Die();
            Position = Respawn.Instance.Position;
            World.Combatables.Add(this);
            Health = MaxHealth;

            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

            Tween tween = CreateTween().SetLoops(10);
            tween.TweenProperty(Sprite, "modulate", new Color(Colors.White, 0.25f), 0.1f);
            tween.TweenProperty(Sprite, "modulate", Colors.White, 0.1f);
        }
    }
}