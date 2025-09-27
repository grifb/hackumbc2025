using Godot;

namespace Game;

public partial class PauseMenu : CanvasLayer
{
    private Button resume;
    private Button quit;

    public override void _Ready()
    {
        resume = GetNode<Button>("Background/Menu/Resume");
        quit = GetNode<Button>("Background/Menu/Quit");

        resume.Pressed += Resume;
        quit.Pressed += Quit;
    }

    private void Resume()
    {
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        Visible = false;
    }

    private void Quit()
    {
        GetTree().Quit();
    }
}
