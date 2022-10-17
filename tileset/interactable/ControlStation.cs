using Godot;
using System;
using System.Collections.Generic;

namespace ADKR.Game
{
    public partial class ControlStation : Interactable
    {
        private Sprite2D _light;

        private static readonly List<ControlStation> _stations = new();

        public override async void _Ready()
        {
            base._Ready();
            _stations.Add(this);
            _light = GetNode<Sprite2D>("Light");

            await ToSignal(GetTree(), "process_frame");
        }

        protected override async void Execute()
        {
            GetTree().Paused = true;

            await ToSignal(GetTree().CreateTimer(1f), "timeout");

            AimIndicator.Instance.Visible = false;

            float originalSpeed = Camera.Instance.SmoothingSpeed;
            Camera.Instance.SmoothingSpeed = 1f;
            Camera.AttachTo(_light);

            await ToSignal(GetTree().CreateTimer(5f), "timeout");

            _light.Visible = false;

            await ToSignal(GetTree().CreateTimer(2f), "timeout");

            Camera.AttachTo(Player.Instance);

            await ToSignal(GetTree().CreateTimer(5f), "timeout");

            AimIndicator.Instance.Visible = true;

            Camera.Instance.SmoothingSpeed = originalSpeed;
            GetTree().Paused = false;
            _stations.Remove(this);

            if (_stations.Count == 0) SetBossObjective();

            QueueFree();
        }

        private static void SetBossObjective()
        {
            BossInteractable _interactable = ResourceLoader.Load<PackedScene>("res://tileset/interactable/BossInteractable.tscn").Instantiate<BossInteractable>();
            Player.Instance.GetParent().AddChild(_interactable);
        }
    }
}