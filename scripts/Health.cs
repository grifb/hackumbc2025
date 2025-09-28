using Godot;

namespace Game;

[GlobalClass]
public partial class Health : Node
{
    [Signal] public delegate void DiedEventHandler();
    [Signal] public delegate void HealthChangedEventHandler(int health);

    [Export] public int MaxHealth { get; set; }
    public int CurrentHealth { get; private set; }

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void Heal(int amount)
    {
        if (amount > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        } else {
            CurrentHealth += Mathf.Abs(amount);
        }

        EmitSignal(SignalName.HealthChanged, CurrentHealth);
    }

    public void Damage(int amount)
    {
        if (amount > CurrentHealth)
        {
            CurrentHealth = 0;
        } else {
            CurrentHealth -= Mathf.Abs(amount);
        }

        if (CurrentHealth == 0)
        {
            EmitSignal(SignalName.Died);
        }

        EmitSignal(SignalName.HealthChanged, CurrentHealth);
    }
}
