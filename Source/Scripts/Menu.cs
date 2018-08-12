using Godot;
using System;

public class Menu : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    AudioStreamPlayer click;
    public override void _Ready()
    {
        click = (AudioStreamPlayer)GetTree().GetRoot().GetNode("SceneSwitcher/Click");
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
    private void Play()
    {
        click.Play();
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/GameMode.tscn");
    }
    private void Exit()
    {
        click.Play();
        GetTree().Quit();
        // Replace with function body
    }

    private void LearnScreen()
    {
        click.Play();
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/Learn.tscn");
        // Replace with function body
    }

}






