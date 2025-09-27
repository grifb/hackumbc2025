using Godot;

namespace Game;

public partial class DebugUI : CanvasLayer
{
	private Label fps;
	private Label velocity;

	public override void _Ready()
	{
		fps = GetNode<Label>("FPS");
		velocity = GetNode<Label>("Velocity");
	}

	public override void _Process(double delta)
	{
		fps.Text = $"FPS: {Engine.GetFramesPerSecond()}";
		velocity.Text = GetNode<CharacterBody3D>("/root/Node3D/Player").Velocity.ToString();
	}
}
