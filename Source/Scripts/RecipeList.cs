using Godot;
using System;
using System.Collections.Generic;

public class RecipeList : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    Vector2 offset = new Vector2(0, 0);
    NinePatchRect paper;
    ReferenceRect holder;
    AudioStreamPlayer click;
    public List<Recipe> recipes = new List<Recipe>();
    PackedScene riPrefab;

    public override void _Ready()
    {
        click = (AudioStreamPlayer)GetTree().GetRoot().GetNode("SceneSwitcher/Click");
        paper = (NinePatchRect)GetNode("ReferenceRect/NinePatchRect");

        holder = (ReferenceRect)GetNode("ReferenceRect");
        riPrefab = (PackedScene)ResourceLoader.Load("res://Instances/RecipeItem.tscn");

        GetRecipes();
        Vector2 pos = new Vector2(53, 192);
        foreach(Recipe r in recipes){
            RecipeItem recipeItem = (RecipeItem)riPrefab.Instance();
            recipeItem.name = r.name;
            recipeItem.ingredients = r.ingredients;
            recipeItem.SetPosition(pos);
            GetNode("ReferenceRect").AddChild(recipeItem);
            pos.y += 139;
        }
        Button button = (Button)GetNode("ReferenceRect/Button");
        pos.x = 29;
        pos.y += 30;
        button.SetPosition(pos);
        NinePatchRect ninePatchRect = (NinePatchRect)GetNode("ReferenceRect/NinePatchRect");
        ninePatchRect.SetSize(new Vector2(ninePatchRect.GetSize().x, pos.y + 102));

    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsAction("ui_wheel_up"))
        {
            offset.y = 20;
            holder.SetPosition(holder.GetPosition() + offset);
        }
        if (@event.IsAction("ui_wheel_down"))
        {
            offset.y = -20;
            holder.SetPosition(holder.GetPosition() + offset);
        }
        if(@event.IsActionPressed("ui_exit")){
            click.Play();
            SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
            switcher.ChangeScene("res://Scenes/Learn.tscn"); 
        }
    }
    void GetRecipes()
    {

        File RecipesFile = new File();
        RecipesFile.Open("res://Data/Recipes.txt", 1);
        string line = RecipesFile.GetLine();
        while (line != "")
        {
            string[] Data = line.Split("=");
            string[] Info = Data[0].Split(";");
            string[] ingredients = Data[1].Split(";");
            Recipe NewRecipe = new Recipe(Info[0], ingredients, float.Parse(Info[1]));
            recipes.Add(NewRecipe);
            line = RecipesFile.GetLine();
        }
        RecipesFile.Close();

    }
    void Back()
    {
        click.Play();
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/Learn.tscn");
    }
}