using Godot;

namespace Game;

public partial class DebugUI : CanvasLayer
{
    private Label fps;

    public override void _Ready()
    {
        fps = GetNode<Label>("FPS");
    }

    public override void _Process(double delta)
    {
        fps.Text = $"FPS: {Engine.GetFramesPerSecond()}";
    }
}
