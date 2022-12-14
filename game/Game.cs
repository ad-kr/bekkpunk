using Godot;
using System;

namespace ADKR.Game
{
    public partial class Game : Node2D
    {
        public static Game Instance { get; set; }

        public Game()
        {
            Instance = this;
        }

        public static void LoadMainMenu()
        {
            Instance.GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
        }

        public static void LoadRandomizer()
        {
            Instance.GetTree().ChangeSceneToFile("res://scenes/RandomizePlayer.tscn");
        }

        public static void LoadWorld()
        {
            Instance.GetTree().ChangeSceneToFile("res://scenes/World.tscn");
        }

        public static SignalAwaiter Wait(float seconds)
        {
            return Instance.ToSignal(Instance.GetTree().CreateTimer(seconds), "timeout");
        }
    }
}