using Godot;

namespace Game;

public enum NPCState
{
    Patrol,
    Turning,
    Attack,
    Jump
};

public partial class JumpingNPC : CharacterBody3D
{
    private readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    [Export] private float Mass = 10f;
    [Export] private float MoveSpeed = 5f;
    [Export] private float DetectionRadius = 30f;
    [Export] private float JumpForce = 25f;
    public Area3D HitBox { get; private set; }
    private Area3D targetRadius;
    private Timer attackTimer;
    private NPCState currentState;
    private RandomNumberGenerator rng;
    private Node3D playerBody = null;

    public override void _Ready()
    {
        HitBox = GetNode<Area3D>("Hitbox");
        targetRadius = GetNode<Area3D>("TargetRadius");
        attackTimer = GetNode<Timer>("AttackTimer");
        rng = new RandomNumberGenerator();

        attackTimer.Timeout += () => {
            if (currentState == NPCState.Patrol) {
                currentState = NPCState.Turning;
            } else if (currentState == NPCState.Attack) {
                currentState = NPCState.Jump; 
            }

            attackTimer.WaitTime = rng.RandfRange(2f, 4f);
        };


        targetRadius.BodyEntered += (Node3D body) => {
            if (body.IsInGroup("Player"))
            {
                playerBody = body;
                currentState = NPCState.Attack;
            }
        };

        targetRadius.BodyExited += (Node3D body) => {
            if (body.IsInGroup("Player"))
            {
                playerBody = null;
                currentState = NPCState.Patrol;
            }
        };

        currentState = NPCState.Patrol;
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (IsOnFloor() == false) {
            velocity.Y = Velocity.Y - Mass * Gravity * (float)delta;
        }

        switch (currentState)
        {
            case NPCState.Patrol:
                break;
            case NPCState.Turning:
                velocity.X = rng.Randf() * MoveSpeed;
                velocity.Z = rng.Randf() * MoveSpeed;
                currentState = NPCState.Patrol;
                break;
            case NPCState.Attack:
                if (playerBody == null) {
                    currentState = NPCState.Patrol;
                    return;
                }

                var dir = GlobalPosition.DirectionTo(playerBody.GlobalPosition);
                velocity = new Vector3(dir.X * MoveSpeed, velocity.Y, dir.Z * MoveSpeed);
                break;
            case NPCState.Jump:
                velocity.Y = JumpForce;
                currentState = NPCState.Attack;
                break;
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
