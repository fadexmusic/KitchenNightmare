using Godot;
using System;

public class Learn : Node2D
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
    private void Back()
    {
        click.Play();
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/Menu.tscn");
    }

    private void Recipes()
    {
        click.Play();
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/RecipeList.tscn");
        // Replace with function body
    }


    private void Ingredients()
    {
        click.Play();
        // Replace with function body
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/Ingredients.tscn");
    }


    private void Tutorial()
    {
        click.Play();
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/Tutorial.tscn");
    }

}




