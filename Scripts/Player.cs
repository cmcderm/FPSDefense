using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export] private int speed = 10;
	[Export] private int h_acceleration = 6;
	[Export] private float mouseSensitivity = 0.03f;
	[Export] private float gravity = 9.81f;

	private Vector3 direction = Vector3.Zero;
	private Vector3 h_velocity = Vector3.Zero;
	private Vector2 mouseLookDelta = Vector2.Zero;
	private Spatial head;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.head = GetNode<Spatial>("Head");
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("pause")) {
			Input.MouseMode = Input.MouseModeEnum.Visible;
		} else if (Input.MouseMode == Input.MouseModeEnum.Visible && Input.IsActionJustPressed("primary")) {
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent) {
			RotateY(Mathf.Deg2Rad(-mouseEvent.Relative.x * this.mouseSensitivity));
			this.head.RotateX(Mathf.Deg2Rad(-mouseEvent.Relative.y * this.mouseSensitivity));
			this.head.Rotation = new Vector3(
				Mathf.Clamp(this.head.Rotation.x, Mathf.Deg2Rad(-89), Mathf.Deg2Rad(89)),
				this.Rotation.y,
				this.Rotation.z
			);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector3 direction = this.getMovementDir(delta);
		MoveAndSlide(direction * speed, Vector3.Up);
	}

	private Vector3 getMovementDir(float delta) {
		Vector3 direction = new Vector3();

		// Basis globalBasis = GlobalTransform.basis;

		if (Input.IsActionPressed("move_forward")) {
			direction -= Transform.basis.z;
		} else if (Input.IsActionPressed("move_back")) {
			direction += Transform.basis.z;
		}

		if (Input.IsActionPressed("move_left")) {
			direction -= Transform.basis.x;
		} else if (Input.IsActionPressed("move_right")) {
			direction += Transform.basis.x;
		}


		direction = direction.Normalized();
		h_velocity = h_velocity.LinearInterpolate(direction * speed, h_acceleration * delta);
		.z = h_velocity.z;

	}
}
