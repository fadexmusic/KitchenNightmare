using Godot;
using System;

public class RecipeItem : Label
{
    public string name;
    public string[] ingredients;

    Sprite sprite;

    public override void _Ready()
    {
        SetText(name);
        Vector2 startingPos = new Vector2(324,79);
        foreach (string ingredientName in ingredients){
            Sprite sprite = new Sprite();
            Texture texture = (Texture)ResourceLoader.Load("res://Sprites/Food/" + ingredientName + ".png");
            sprite.SetTexture(texture);
            sprite.SetPosition(startingPos);
            AddChild(sprite);
            startingPos.x -= 64;
        }
    }
}
