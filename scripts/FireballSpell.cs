using Game;
using Godot;
using System;
using System.Data;
using System.Diagnostics;







public partial class FireballSpell : RigidBody3D, ISpell
{
	public int manaCost = 30;
	public int castTime = 1;
	public int ManaCost()
	{
		return manaCost;
	}

	public int CastTime()
	{
		return 1;
	}



	public void setRotation(Vector3 direction)
	{
		Rotation = direction;
	}

	public void setPosition(Vector3 position)
	{
		GlobalPosition = position;
	}

	public void Launch(int speed)
	{
		LinearVelocity = new Vector3(0, 0, -speed) * GlobalTransform.Basis.Inverse();

	}

	public override void _PhysicsProcess(double delta)
	{

		if (GetCollidingBodies().Count > 0)
		{
			Debug.Print("Collision Detected");
			QueueFree();
		}
	}
	
	
	
}
