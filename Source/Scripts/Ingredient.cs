using Godot;
using System;

public class Ingredient : Label
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    public string name = "";

    Sprite sprite;

    public override void _Ready()
    {
        sprite = (Sprite)GetNode("Sprite");
        Texture texture = (Texture)ResourceLoader.Load("res://Sprites/Food/" + name + ".png");
        sprite.SetTexture(texture);
        SetText(name);
        // Called every time the node is added to the scene.
        // Initialization here
        
    }
}
