using Godot;
using System;

public partial class Main : Node
{
		[Export]
		public PackedScene ExplosiveMineScene { get; set; }

		public int Laps;
		public const int TotalLaps = 5;

		public override void _Ready()
		{
				NewGame();
		}

		public override void _Process(double delta)
		{
		}

		public void NewGame()
		{
				PlayerRacer Player = GetNode<PlayerRacer>("PlayerRacer");
				Marker2D StartPos = GetNode<Marker2D>("StartPos");
				Laps = 0;

				Player.Start(StartPos.Position);
				GetNode<Timer>("ExplosiveMineTimer").Start();
		}

		public void GameOver()
		{
				GetNode<Timer>("ExplosiveMineTimer").Stop();
		}

		private void OnExplosiveMineTimerTimeout()
		{
				ExplosiveMine ExplosiveMine = ExplosiveMineScene.Instantiate<ExplosiveMine>();
				PathFollow2D ExplosiveMineSpawnLocation = GetNode<PathFollow2D>("ExplosiveMinePath/ExplosiveMineSpawn");

				ExplosiveMineSpawnLocation.ProgressRatio = GD.Randf();
				ExplosiveMine.Position = ExplosiveMineSpawnLocation.Position;

				AddChild(ExplosiveMine);

		}

		public void MineEvent()
		{
				GD.Print("MineEvent");
				GetNode<ExplosiveMine>("ExplosiveMine").Hide();
		}

}
