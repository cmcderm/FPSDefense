using Godot;
using System;

public class rotateY : KinematicBody
{
	public override void _Ready() {}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		this.RotateY(delta);
	}
}
