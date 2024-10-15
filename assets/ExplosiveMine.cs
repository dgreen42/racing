using Godot;

public partial class ExplosiveMine : Area2D
{
		[Signal]
		public delegate void PlayerCollisionEventHandler();

		public override void _Ready()
		{
				AnimatedSprite2D MineAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
				string[] AnimationNames = MineAnimation.SpriteFrames.GetAnimationNames();
				MineAnimation.Play("Flash");
		}

		public override void _Process(double delta)
		{
		}

		public void OnBodyEntered(Node2D body)
		{
				AnimatedSprite2D MineAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
				string[] AnimationNames = MineAnimation.SpriteFrames.GetAnimationNames();
				MineAnimation.Play("Explode");
				GetNode<Timer>("AnimationTimer").Start();
		}

		public void OnAnimationTimerTimeout()
		{
				QueueFree();
		}

		public void RegisterHit()
		{
				GD.Print("MineHit");

		}

}
