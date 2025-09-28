using Game;
using Godot;
using System;
using System.Data;
using System.Diagnostics;





public partial class ShieldAttempt2 : RigidBody3D, ISpell
{
	Player player;
	SceneTreeTimer timer;
	public int ManaCost()
	{
		return 20;
	}

	public void Time()
	{
		timer = GetTree().CreateTimer(5);
		timer.Timeout += () => { QueueFree(); };
	  
		
	}

	public int CastTime()
	{
		return 1;
	}

	public void Cast(Player Player)
	{ 
		player = Player;
	}

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = player.GlobalPosition + new Vector3(0, 2, 0);
		Debug.Print("Time left: {0}", timer.TimeLeft);
		


	}


}
