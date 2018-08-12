using Godot;
using System;

public class Tutorial : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    NinePatchRect paper;
    ReferenceRect holder;

    Vector2 offset = new Vector2(0, 0);
    AudioStreamPlayer click;
    public override void _Ready()
    {
        click = (AudioStreamPlayer)GetTree().GetRoot().GetNode("SceneSwitcher/Click");
        paper = (NinePatchRect)GetNode("ReferenceRect/NinePatchRect");

        holder = (ReferenceRect)GetNode("ReferenceRect");
        // Called every time the node is added to the scene.
        // Initialization here
        
    }
    public override void _Input(InputEvent @event)
    {
        if(@event.IsAction("ui_wheel_up")){
            offset.y = 20;
            holder.SetPosition(holder.GetPosition() + offset);
        }
        if (@event.IsAction("ui_wheel_down"))
        {
                offset.y = -20;
                holder.SetPosition(holder.GetPosition() + offset);

        }
        if (@event.IsActionPressed("ui_exit"))
        {
            click.Play();
            SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
            switcher.ChangeScene("res://Scenes/Learn.tscn");
        }
    }
    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
    private void Back()
    {
        click.Play();
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/Learn.tscn");
        // Replace with function body
    }
}



