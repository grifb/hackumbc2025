using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace Game;

public enum SpellInput
{
	Up,
	Down,
	Left,
	Right
}

public enum Spell
{
	Invalid,
	SpeedBoost,
	SuperJump,
	Hover,
	Shield,
	LightningStrike,
	Healing,
	ManaRegen,
	Fireball
}

[GlobalClass]
public partial class SpellManager : Node
{
	public static SpellManager Instance { get; private set; }

	private readonly Dictionary<Spell, string> SpellNames = new() {
		{ Spell.Invalid, "Invalid" },
		{ Spell.SpeedBoost, "Speed Boost" },
		{ Spell.SuperJump, "Super Jump" },
		{ Spell.Hover, "Hover" },
		{ Spell.Shield, "Shield" },
		{ Spell.LightningStrike, "Lightning Strike" },
		{ Spell.Healing, "Healing" },
		{ Spell.ManaRegen, "Mana Regen" },
		{ Spell.Fireball, "Fireball"}

	};

	private readonly Dictionary<SpellInput[], Spell> _spells = new() {
		{
			[
				SpellInput.Up,
				SpellInput.Right,
				SpellInput.Left,
				SpellInput.Down
			],
			Spell.SpeedBoost



		}, {
			 [
				SpellInput.Up,
				SpellInput.Up
			],
			Spell.Fireball
		},
		{
			[
				SpellInput.Up,
				SpellInput.Down,
				SpellInput.Up,
				SpellInput.Up,

			],
			Spell.SuperJump
		},
		{
			[
				SpellInput.Down,
				SpellInput.Down,

			],
				Spell.Shield
		},
		{
			[
				SpellInput.Left,
				SpellInput.Left,

			],
				Spell.Healing
		}
	};

	public readonly Dictionary<string, Spell> SpellTable = [];

	public override void _Ready()
	{
		Instance = this;
		GenerateSpellTable();
	}

	private string InputSequenceToString(SpellInput[] inputs)
	{
		string output = "";

		foreach (var key in inputs)
		{
			output += key switch
			{
				SpellInput.Up => "Up",
				SpellInput.Down => "Down",
				SpellInput.Left => "Left",
				SpellInput.Right => "Right",
				_ => "Invalid"
			};
		}

		return output;
	}

	private void GenerateSpellTable()
	{
		foreach (var spell in _spells)
		{
			SpellTable.Add(InputSequenceToString(spell.Key), spell.Value);
		}
	}


	public string GetSpell(string inputSequence)
	{
		return SpellNames[SpellTable.GetValueOrDefault(inputSequence, Spell.Invalid)];
	}

	public void Fireball(Vector3 direction, Vector3 position)
	{
		var fireball = GD.Load<PackedScene>("res://fireball_spell.tscn");
		Debug.Print("Fireball loaded");
		Node fireball_node = fireball.Instantiate();
		GetTree().Root.AddChild(fireball_node);
		Debug.Print("Fireball instantiated");
		RigidBody3D fireball_rigidBody = (RigidBody3D)fireball_node;
		FireballSpell fireballSpellActual = (FireballSpell)fireball_node;
		Debug.Print("Rigidbody set");
		Debug.Print("Fireball name: {0}", fireball_rigidBody.Rotation);
		fireballSpellActual.setPosition(position);
		fireballSpellActual.setRotation(direction);
		fireballSpellActual.Launch(50);
	}

	public void Shield(Player player)
	{
		var shield = GD.Load<PackedScene>("res://shield_spell.tscn");
		Debug.Print("Shield loaded");
		Node shield_node = shield.Instantiate();
		GetTree().Root.AddChild(shield_node);

		Debug.Print("Shield instantiated");
		Shield shieldActual = (Shield)shield_node;
		Debug.Print("Shield set");
		Debug.Print("Shield name: {0}", shieldActual.Name);
		shieldActual.Cast(player);

	}
	
	public void Heal(Player player)
	{
		var heal = GD.Load<PackedScene>("res://heal_spell.tscn");
		Debug.Print("Heal loaded");
		Node heal_node = heal.Instantiate();
		GetTree().Root.AddChild(heal_node);

		Debug.Print("Heal instantiated");
		HealSpell healActual = (HealSpell)heal_node;
		Debug.Print("Heal set");
		Debug.Print("Heal name: {0}", healActual.Name);
		healActual.Cast(player);

	}


}

public interface ISpell
{
	int ManaCost();

	int CastTime();

	

 }
