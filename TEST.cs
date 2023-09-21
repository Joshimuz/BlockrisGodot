using Godot;
using System;

public partial class TEST : Sprite2D
{
	[Export] RichTextLabel testText;

    Vector2 TouchPosition;

    [Export] float MovementSpeed = 50;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            TouchPosition = GetGlobalMousePosition();

            if (TouchPosition.X < 540)
            {
                Position += new Vector2(-1, 0) * (MovementSpeed * (float)delta);
            }
            else
            {
                Position += new Vector2(1, 0) * (MovementSpeed * (float)delta);
            }

            testText.Text = Position.ToString();
        }
	}

    public override void _Input(InputEvent @event)
    {
        //testText.Text = @event.AsText();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
                GetTree().Quit();
    }
}
