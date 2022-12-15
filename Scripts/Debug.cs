using Godot;
using System;
using System.Text;

public class Debug : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		this.AddColorOverride("font_color", new Color(0, .5f, 0.1f, 1));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta) {
		StringBuilder labelBuilder = new StringBuilder();
		labelBuilder.Append(getFpsLabel());
		
		this.Text = labelBuilder.ToString();
	}
	
	private string getFpsLabel() {
		string fps = Engine.GetFramesPerSecond().ToString();
		return $"fps: {fps}";
	}
}
