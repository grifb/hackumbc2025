using Godot;

namespace Game;

[GlobalClass]
public partial class Mana : Node
{
    [Signal] public delegate void ManaChangedEventHandler(int mana);

    [Export] public int MaxMana { get; set; }
    public int CurrentMana { get; private set; } 

    public override void _Ready()
    {
        CurrentMana = MaxMana;
    }

    public void Restore(int amount)
    {
        if (amount > MaxMana)
        {
            CurrentMana = MaxMana;
        } else {
            CurrentMana += Mathf.Abs(amount);
        }

        EmitSignal(SignalName.ManaChanged, CurrentMana);
    }

    public void Consume(int amount)
    {
        if (amount > CurrentMana)
        {
            CurrentMana = 0;
        } else {
            CurrentMana -= Mathf.Abs(amount);
        }

        EmitSignal(SignalName.ManaChanged, CurrentMana);
    }
}
