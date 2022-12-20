using Godot;
using System;

public class Turret : KinematicBody
{

	private Spatial head;
	private Spatial rotator;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		this.head = GetNode<Spatial>("Body/Rotator/Head");
		this.rotator = GetNode<Spatial>("Body/Rotator");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta) {
		
	}

	private Node findTarget() {
		IntersectShape(new PhysicsShapeQueryParameters());
	}

	private void turretLook(Vector3 target) {

	}
}
