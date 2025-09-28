using System.Collections.Generic;
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
	ManaRegen
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
		{ Spell.ManaRegen, "Mana Regen" }
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

		},
		{
			[
				SpellInput.Up,
				SpellInput.Down,
				SpellInput.Up,
				SpellInput.Up,
				
			],
			Spell.SuperJump
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

	

}
