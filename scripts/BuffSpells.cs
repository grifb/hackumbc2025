using Godot;
using System;

namespace Game;

public static class BuffSpells
{


	public static void ApplySpellEffects(string spell, Player player)
	{
		switch (spell)
		{
			case "Speed Boost":
				ApplySpeedBoost(player);
				break;

			case "Super Jump":
				ApplyJumpBoost(player);
				break;
		}
	}


	private static void ApplySpeedBoost(Player player)
	{

		GD.Print("ACTIVATING SPEED BOOST!");

		float originalSpeed = player.getMoveSpeed();

		player.setMoveSpeed(originalSpeed + 20f);

		var timer = player.GetTree().CreateTimer(15.0f);

		timer.Timeout += () =>
		{
			player.setMoveSpeed(originalSpeed);
			GD.Print("Speed boost over!");

		};



	}

	private static void ApplyJumpBoost(Player player)
	{
		GD.Print("ACTIVATING SUPER JUMP!");
		player.SuperJumpTriggered = true;
	}

} 
