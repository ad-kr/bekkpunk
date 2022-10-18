using Godot;
using System;

namespace ADKR.Game
{
    public class StationsObjective : Objective
    {
        public override void Start()
        {
            Instruction = "Deaktiver alle stasjonene på Skuret.";

            AimIndicator.Instance.Visible = true;
            HealthBar.Instance.Visible = true;
            ControlStation.Stations.ForEach(station => station.Visible = true);

            Player.Instance.State = new PlayerIdleState();
        }
    }
}