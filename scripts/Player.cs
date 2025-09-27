using Godot;

namespace Game;

public partial class Player : CharacterBody3D
{
	private Node3D cameraPivot;
	private Camera3D camera;
<<<<<<< Updated upstream
	private SpringArm3D springArm;
=======

	private SpringArm3D springArm;

>>>>>>> Stashed changes
	[Export] private CanvasLayer pauseMenu;
	private readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
	[Export(PropertyHint.Range, "0,180,radians_as_degrees")] private float TiltAboveMax = Mathf.DegToRad(75f);
	[Export(PropertyHint.Range, "0,180,radians_as_degrees")] private float TiltBelowMax = Mathf.DegToRad(50);
	[Export] private float MoveSpeed = 10f;
<<<<<<< Updated upstream
=======

	[Export] private float SprintMultiplier = 1.15f;

>>>>>>> Stashed changes
	[Export] private float SprintSpeed = 60f;
	[Export] private float JumpForce = 50f;
	[Export] private float Mass = 50f;
	[Export] private float MouseSensitivity = 0.01f;

	public override void _Ready()
	{
		cameraPivot = GetNode<Node3D>("CameraPivot");
<<<<<<< Updated upstream
=======
		camera = cameraPivot.GetNode<Camera3D>("SpringArm3D/Camera3D");

		if (camera.Current)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}



	public override void _PhysicsProcess(double delta)
	{
		var direction = new Vector3(Input.GetAxis("Left", "Right"), 0, Input.GetAxis("Up", "Down")).Rotated(Vector3.Up, cameraPivot.Rotation.Y);
		var velocity = direction * MoveSpeed;

		bool isSprinting = Input.IsActionPressed("Sprint");

		MoveSpeed = isSprinting ? MoveSpeed * SprintMultiplier : MoveSpeed;

		if (IsOnFloor() == false)
		{
			velocity.Y = Velocity.Y - Mass * Gravity * (float)delta;
		}
		else if (Input.IsActionJustPressed("Jump"))
		{
			velocity.Y = JumpForce;
		}

		Velocity = velocity;
		MoveAndSlide();

>>>>>>> Stashed changes
		springArm = cameraPivot.GetNode<SpringArm3D>("SpringArm3D");
		camera = springArm.GetNode<Camera3D>("Camera3D");

		springArm.AddExcludedObject(GetRid());

		if (camera.Current)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	public override void _UnhandledInput(InputEvent ev)
	{
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
<<<<<<< Updated upstream

	public override void _PhysicsProcess(double delta)
	{
		var direction = new Vector3(Input.GetAxis("Left", "Right"), 0, Input.GetAxis("Up", "Down")).Rotated(Vector3.Up, cameraPivot.Rotation.Y);
		var velocity = direction * MoveSpeed;

		bool isSprinting = Input.IsActionPressed("Sprint");

		MoveSpeed = isSprinting ? SprintSpeed : 10f;

		if (IsOnFloor() == false)
		{
			velocity.Y = Velocity.Y - Mass * Gravity * (float)delta;
		}
		else if (Input.IsActionPressed("Jump"))
		{
			velocity.Y = JumpForce;
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
=======
>>>>>>> Stashed changes
}
