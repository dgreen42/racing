using Godot;
using System;

public partial class PlayerRacer : CharacterBody2D
{

		[Signal]
		public delegate void MineCollisionEventHandler();
		public const int DT = 10;
		Vector2 CarLocation;
		float CarHeading; // angle relative to car that the car is moving on
		double Speed;
		double SteerAngle; // and at which the car is turning
		public const float WHEELDISTANCE = 50; // change to the actual distance 
		public int FreezeTime;
		public bool Collided;

		public override void _Ready()
		{
				FreezeTime = 0;
				Hide();
		}

		public override void _PhysicsProcess(double delta)
		{
				// get position of wheels
				CarLocation = Position;
				CarHeading = Rotation;
				Node2D CollisionNode = GetCollision();
				SetFreezeTime(CollisionNode);
				(Position, Rotation, Speed) = GetMovement(CarLocation, CarHeading, Speed, CollisionNode);
				MoveAndSlide();
				GD.Print(GetDirection());
		}

		private (Vector2, float) GetWheels(Vector2 carLoc, float carHead, double Speed)
		{

				Vector2 frontWheel = carLoc + WHEELDISTANCE / 2 * new Vector2((float)Math.Cos(carHead), (float)Math.Sin(carHead));
				Vector2 backWheel = carLoc - WHEELDISTANCE / 2 * new Vector2((float)Math.Cos(carHead), (float)Math.Sin(carHead));
				//move
				SteerAngle = GetRot();
				backWheel += (float)Speed * DT * new Vector2((float)Math.Cos(carHead), (float)Math.Sin(carHead));
				frontWheel += (float)Speed * DT * new Vector2((float)Math.Cos(carHead + SteerAngle), (float)Math.Sin(carHead + SteerAngle));
				// update position
				Vector2 newCarLoc = (frontWheel + backWheel) / 2;
				float newCarHead = (float)Math.Atan2(frontWheel.Y - backWheel.Y, frontWheel.X - backWheel.X);

				return (newCarLoc, newCarHead);
		}


		public (Vector2, float, double) GetMovement(Vector2 carLoc, float carHead, double Speed, Node2D CollisionNode)
		{
				Vector2 newCarLocation = carLoc;
				float newCarHeading = carHead;
				if (FreezeTime > 0)
				{
						//back collision
						if (Input.IsActionPressed("move_back"))
						{
								Speed = -0.8;
						}

						//front collision
						if (Input.IsActionPressed("move_for") || Input.IsActionJustReleased("move_for"))
						{
								Speed = 0.8;
						}

						FreezeTime -= 1;
						(newCarLocation, newCarHeading) = GetWheels(carLoc, carHead, Speed);

				}
				else
				{
						if (Speed == 0)
						{
								Speed = 0;
						}

						if (Input.IsActionPressed("move_for"))
						{
								Speed -= 0.03;
						}
						else if (Speed < 0)
						{
								Speed += 0.05;
						}

						if (Input.IsActionPressed("move_back"))
						{
								Speed += 0.03;
						}
						else if (Speed > 0)
						{
								Speed -= 0.05;
						}

						if (Speed > 2.1)
						{
								Speed = 2;
						}

						if (Speed < -2.1)
						{
								Speed = -2;
						}

						(newCarLocation, newCarHeading) = GetWheels(carLoc, carHead, Speed);
				}
				return (newCarLocation, newCarHeading, Speed);
		}

		private double GetRot()
		{
				double rot = 0;
				if (Input.IsActionPressed("move_right"))
				{
						rot -= 0.2;
				}
				if (Input.IsActionPressed("move_left"))
				{
						rot += 0.2;
				}
				return rot;
		}

		public void Start(Vector2 pos)
		{
				Position = pos;
				Show();
				GetNode<CollisionPolygon2D>("Collision").Disabled = false;
		}

		public Node2D GetCollision()
		{
				Node2D CollisionBody;
				for (int i = 0; i < GetSlideCollisionCount(); i++)
				{
						KinematicCollision2D collision = GetSlideCollision(i);
						CollisionBody = collision.GetCollider() as Node2D;
						return CollisionBody;
				}
				Node2D NullNode = new Node2D();
				//float PlaceHolder = 0; // should probably change this at some point as it may have weird functionality
				NullNode.Name = "NULL";
				return NullNode;

		}

		private void SetFreezeTime(Node2D CollidingBody)
		{
				if (CollidingBody.Name == "Track")
				{
						FreezeTime = 20;
						GetNode<CollisionPolygon2D>("Collision").Disabled = true;
				}
				if (CollidingBody.Name == "ExplosiveMine")
				{
						FreezeTime = 20;
						GetNode<CollisionPolygon2D>("Collision").Disabled = true;
				}
				else
				{
						GetNode<CollisionPolygon2D>("Collision").Disabled = false;
				}
		}

		private Vector2 GetDirection()
		{
				double X = Position.X;
				double Y = Position.Y;
				Vector2 LastMotion = GetLastMotion();
				return LastMotion;
		}
}



