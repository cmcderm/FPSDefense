using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody
{
	[Export] private int speed = 7;
	const int ACCEL_DEFAULT = 7;
	const int ACCEL_AIR = 1;
	[Export] private int accel = ACCEL_DEFAULT;
	[Export] private float mouseSensitivity = 0.03f;
	[Export] private float gravity = 9.81f;
	[Export] private float jump = 5f;

	private Vector3 snap;

	private Vector3 velocity = new Vector3();
	private Vector3 gravityVel = new Vector3();
	private Vector3 movement = new Vector3();

	private Spatial head;
	private Camera camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.head = GetNode<Spatial>("Head");
		this.camera = GetNode<Camera>("Head/Camera");

		this.accel = ACCEL_DEFAULT;

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		// Unlock the Mouse on 'pause'
		if (Input.IsActionJustPressed("pause")) {
			Input.MouseMode = Input.MouseModeEnum.Visible;
		} else if (Input.MouseMode == Input.MouseModeEnum.Visible && Input.IsActionJustPressed("primary")) {
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent) {
			Vector3 lookDelta = new Vector3(
				Mathf.Clamp(Mathf.Deg2Rad(-mouseEvent.Relative.y * this.mouseSensitivity), Mathf.Deg2Rad(-89), Mathf.Deg2Rad(89)),
				Mathf.Deg2Rad(-mouseEvent.Relative.x * this.mouseSensitivity),
				0
			);

			this.RotateY(lookDelta.y);
			this.head.RotateX(lookDelta.x);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector3 directionSpeed = this.getMovementDir(delta);
		MoveAndSlideWithSnap(directionSpeed, snap, Vector3.Up);
	}

	private Vector3 getMovementDir(float delta) {
		Vector3 direction = Vector3.Zero;

		float hRot = GlobalTransform.basis.GetEuler().y;
		float fInput = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward");
		float hInput = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

		direction = new Vector3(hInput, 0, fInput).Rotated(Vector3.Up, hRot).Normalized();

		if (IsOnFloor()) {
			this.snap = -GetFloorNormal();
			this.accel = ACCEL_DEFAULT;
			this.gravityVel = Vector3.Zero;
		} else {
			this.snap = Vector3.Down;
			this.accel = ACCEL_AIR;
			this.gravityVel += Vector3.Down * gravity * delta;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
			this.snap = Vector3.Zero;
			gravityVel = Vector3.Up * jump;
		}

		// Move
		velocity = velocity.LinearInterpolate(direction * speed, accel * delta);
		movement = velocity + gravityVel;

		return movement;
	}
}
