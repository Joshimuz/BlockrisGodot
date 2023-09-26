using Godot;
using System;

public partial class main_menu : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		GetNode<TouchScreenButton>("TouchScreenButton").Pressed += OnButoootan;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnButoootan()
	{
		GetNode<GlobalController>("/root/GlobalController")
			.ChangeGameState(GlobalController.GameState.Gameplay);
    }
}

//TODO: Add options menu and add the other control scheme