using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class Background : Sprite2D
{
    List<Sprite2D> stars = new List<Sprite2D>();

    RandomNumberGenerator RNG = new RandomNumberGenerator();

    static public float TargetStarMovespeed;
    float StarMovespeed = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        // Make a pure black image and set it as the background colour
        //Image image = Image.Create(1080, 1920, true, Image.Format.Rgb8);
        //image.Fill(Color.Color8(0, 0, 0));

        //Texture = ImageTexture.CreateFromImage(image);

        //Position = new Vector2(1080 / 2, 1920 / 2);

        Image image = Image.Create(1, 1, false, Image.Format.Rgb8);
        image.Fill(Color.Color8(255, 255, 255));

        for (int i = 0; i < 50; i++)
        {
            Sprite2D star = new Sprite2D();

            star.Texture = ImageTexture.CreateFromImage(image);

            AddChild(star);

            star.Scale *= RNG.RandfRange(0, 5);

            star.GlobalPosition = new Vector2(RNG.RandfRange(0 + (i * 10), 580 + (i * 10)), RNG.RandfRange(-20, DisplayServer.WindowGetSize().Y));

            if (RNG.Randf() > 0.5f)
            {
                star.ZIndex = -99;
            }

            stars.Add(star);
        }

        TargetStarMovespeed = 0;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        StarMovespeed = Mathf.Lerp(StarMovespeed, TargetStarMovespeed, (float)delta / 5f);

        foreach (var star in stars)
        {
            star.Position += Vector2.Down * ((float)delta * StarMovespeed);

            if (star.ZIndex == -99)
            {
                float scale = Mathf.Lerp(star.GlobalScale.X, 7f, (float)delta / 10f);
                star.GlobalScale = new Vector2(scale, scale);

                if (scale > 5f)
                {
                    star.ZIndex = -100;
                }
            }
            else
            {
                float scale = Mathf.Lerp(star.GlobalScale.X, 0f, (float)delta / 10f);
                star.GlobalScale = new Vector2(scale, scale);

                if (scale < 2f)
                {
                    star.ZIndex = -99;
                }
            }

            if (star.GlobalPosition.Y - star.GlobalScale.Y > DisplayServer.WindowGetSize().Y)
            {
                star.GlobalPosition = new Vector2(RNG.RandfRange(0, 1080), RNG.RandfRange(-20 + star.GlobalScale.Y, 0 + star.GlobalScale.Y));
            }
        }
    }
}
