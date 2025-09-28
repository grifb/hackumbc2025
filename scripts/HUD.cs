using System.Diagnostics;
using Godot;

namespace Game;

public partial class HUD : CanvasLayer
{
    private ProgressBar healthBar;
    private ProgressBar manaBar;

    public override void _Ready()
    {
        var player = GetTree().GetNodesInGroup("Player")[0];
        var playerHealth = player.GetNode<Health>("Health");
        var playerMana = player.GetNode<Mana>("Mana");

        healthBar.Value = playerHealth.CurrentHealth;
        Debug.Print(playerHealth.CurrentHealth.ToString());
        manaBar.Value = playerMana.CurrentMana;

        playerHealth.HealthChanged += (int health) => {
            healthBar.Value = health;
        };

        playerMana.ManaChanged += (int mana) => {
            manaBar.Value = mana;
        };
    }
}
