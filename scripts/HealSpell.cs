using Game;
using Godot;
using System;
using System.Data;
using System.Diagnostics;


public partial class HealSpell : RigidBody3D, ISpell
{
	Player player;
	public int Duration = 5;
	public int ManaCost()
	{
		return 40;
	}

	public override void _Ready()
	{ 
		SceneTreeTimer timer = GetTree().CreateTimer(Duration);
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
		GlobalPosition = player.GlobalPosition;
		player.GetNode<Health>("Health").Heal(1);

	}

}
