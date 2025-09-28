using Godot;
using System.Collections.Generic;

namespace Game;

public partial class Player : CharacterBody3D
{
    private Node3D cameraPivot;
    private Camera3D camera;
    private SpringArm3D springArm;
    private Label3D spellLabel;
    [Export] private CanvasLayer pauseMenu;
    private readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    [Export(PropertyHint.Range, "0,180,radians_as_degrees")] private float TiltAboveMax = Mathf.DegToRad(75f);
    [Export(PropertyHint.Range, "0,180,radians_as_degrees")] private float TiltBelowMax = Mathf.DegToRad(50);
    [Export] private float MoveSpeed = 10f;
    [Export] private float SprintSpeed = 40f;
    [Export] private float JumpForce = 50f;
    [Export] private float Mass = 50f;
    [Export] private float MouseSensitivity = 0.01f;
    private bool castingSpell = false;
    private string currentSpell = "";
    private SceneTreeTimer clearLabelTimer = null;
	
	//Stuff to make superJump work.
	public bool SuperJumpTriggered = false;
	public float SuperJumpStrength = 350f;
	
    public override void _Ready()
    {
        cameraPivot = GetNode<Node3D>("CameraPivot");
        springArm = cameraPivot.GetNode<SpringArm3D>("SpringArm3D");
        camera = springArm.GetNode<Camera3D>("Camera3D");
        spellLabel = GetNode<Label3D>("SpellLabel");

        springArm.AddExcludedObject(GetRid());

        if (camera.Current) {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    public override void _UnhandledInput(InputEvent ev)
    {
        if (ev is InputEventMouseButton mouseClickEvent && mouseClickEvent.ButtonIndex == MouseButton.Right)
        {
            if (mouseClickEvent.Pressed)
            {
                spellLabel.Modulate = Colors.White;
                spellLabel.Text = "";
                currentSpell = "";
                castingSpell = true;
            }
            else
            {
                castingSpell = false;
                spellLabel.Text = "";

                var spell = SpellManager.Instance.GetSpell(currentSpell);

                if (spell == "Invalid")
                {
                    clearLabelTimer?.Dispose();
                    spellLabel.Modulate = Colors.DarkSlateGray;
                    spellLabel.Text = "You cast like a raging barbarian";
                }
                else if (spell == "Fireball")
                {
                    spellLabel.Modulate = Colors.LimeGreen;
                    spellLabel.Text = spell;

                    Vector3 cameraPosition = camera.GlobalTransform.Origin;
                    Vector3 cameraGlobalForward = camera.GlobalTransform.Basis.Z;
                    Vector3 cameraForward = camera.Transform.Basis.Z;
                    Vector3 spawn = cameraPosition + cameraGlobalForward * -6;
                    spawn.Y = GlobalPosition.Y;
                    SpellManager.Instance.Fireball(camera.GlobalRotation, spawn);
                }

                else
                {
                    spellLabel.Modulate = Colors.LimeGreen;
                    spellLabel.Text = spell;
					BuffSpells.ApplySpellEffects(spell,this); //Call script that has speedboost and superjump
                }

                clearLabelTimer = GetTree().CreateTimer(3.0f);

                clearLabelTimer.Timeout += () => {
                    if (castingSpell == false) {
                        spellLabel.Modulate = Colors.White;
                        spellLabel.Text = "";
                    }
                };
            }
        }

        if (ev is InputEventMouseMotion mouseEvent)
        {
            var xRotation = cameraPivot.Rotation.X - mouseEvent.Relative.Y * MouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, -TiltAboveMax, TiltBelowMax);

            cameraPivot.Rotation = new Vector3(
                    xRotation,
                    cameraPivot.Rotation.Y - mouseEvent.Relative.X * MouseSensitivity,
                    0f
                    );
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var direction = new Vector3(Input.GetAxis("Left", "Right"), 0, Input.GetAxis("Up", "Down")).Rotated(Vector3.Up, cameraPivot.Rotation.Y);
        
        bool isSprinting = Input.IsActionPressed("Sprint");

		var currentSpeed = isSprinting ? SprintSpeed + MoveSpeed: MoveSpeed;

        var velocity = direction * currentSpeed;

        if (IsOnFloor() == false)
        {
            velocity.Y = Velocity.Y - Mass * Gravity * (float)delta;
        }
        else if (Input.IsActionPressed("Jump"))
        {
            velocity.Y = JumpForce;
        }
		if (SuperJumpTriggered)
		{
			velocity.Y = SuperJumpStrength;
			SuperJumpTriggered = false; // reset so it's one-time
			GD.Print("SUPER JUMP!");
		}
        if (castingSpell == true)
        {
            velocity = Vector3.Zero;
            CaptureSpellInputs();
        }

        Velocity = velocity;
        MoveAndSlide();

        if (Input.IsActionJustPressed("Pause"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            GetTree().Paused = true;
            pauseMenu.Visible = true;
        }
    }

    private void CaptureSpellInputs()
    {
        if (Input.IsActionJustPressed("Up"))
        {
            currentSpell += "Up";
            spellLabel.Text += "Up ";
        }
        else if (Input.IsActionJustPressed("Down"))
        {
            currentSpell += "Down";
            spellLabel.Text += "Down ";
        }
        else if (Input.IsActionJustPressed("Left"))
        {
            currentSpell += "Left";
            spellLabel.Text += "Left ";
        }
        else if (Input.IsActionJustPressed("Right"))
        {
            currentSpell += "Right";
            spellLabel.Text += "Right ";
        }
    }
	public float getMoveSpeed()
	{
		return MoveSpeed;
	}

	public void setMoveSpeed(float newSpeed)
	{
		MoveSpeed = newSpeed;
	}
}
