using Godot;
using System;

public partial class OptionsMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNode<TouchScreenButton>("WindowMode").Pressed += ToggleWindowedMode;

        GetNode<RichTextLabel>("WindowMode/TextSizer/Text").Text = 
            DisplayServer.WindowGetMode().ToString();

        GetNode<TouchScreenButton>("AAMode").Pressed += ToggleAAMode;

        SetAAModeText();
    }

	public void ToggleWindowedMode()
	{
        if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed)
        {
            ProjectSettings.SetSetting("display/window/size/mode", 3);

            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);

            ProjectSettings.SaveCustom("override.cfg");

            GetNode<RichTextLabel>("WindowMode/TextSizer/Text").Text = 
                DisplayServer.WindowGetMode().ToString();
        }
        else
        {
            ProjectSettings.SetSetting("display/window/size/mode", 0);

            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);

            ProjectSettings.SaveCustom("override.cfg");

            GetNode<RichTextLabel>("WindowMode/TextSizer/Text").Text = 
                DisplayServer.WindowGetMode().ToString();
        }

        // Get Global Controller to recalculate the aspect ratio coz it could've changed
        GlobalController.CalcAspectRatioNumber();
    }

    public void ToggleAAMode()
    {
        byte setting = (byte)ProjectSettings.GetSetting("rendering/anti_aliasing/quality/msaa_2d", 0);

        if (setting == 3)
        {
            setting = 0;
        }
        else
        {
            setting++;
        }

        ProjectSettings.SetSetting("rendering/anti_aliasing/quality/msaa_2d", setting);

        ProjectSettings.SaveCustom("override.cfg");

        SetAAModeText();
    }

    void SetAAModeText()
    {
        switch ((byte)ProjectSettings.GetSetting("rendering/anti_aliasing/quality/msaa_2d", 0))
        {
            case 0:
                GetNode<RichTextLabel>("AAMode/TextSizer/Text").Text = "MSAA Off";
                break;

            case 1:
                GetNode<RichTextLabel>("AAMode/TextSizer/Text").Text = "MSAA x2";
                break;

            case 2:
                GetNode<RichTextLabel>("AAMode/TextSizer/Text").Text = "MSAA x4";
                break;

            case 3:
                GetNode<RichTextLabel>("AAMode/TextSizer/Text").Text = "MSAA x8";
                break;
        }
    }
}
