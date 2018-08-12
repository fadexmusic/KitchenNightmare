using Godot;
using System;
using System.Collections.Generic;
public class IngredientList : Node2D
{
    public List<FoodInfo> FoodData = new List<FoodInfo>();
    PackedScene ingredientPrefab;
    Vector2 offset = new Vector2(0, 0);
    NinePatchRect paper;
    ReferenceRect holder;
    AudioStreamPlayer click;
    public override void _Ready()
    {
        click = (AudioStreamPlayer)GetTree().GetRoot().GetNode("SceneSwitcher/Click");
        paper = (NinePatchRect)GetNode("ReferenceRect/NinePatchRect");

        holder = (ReferenceRect)GetNode("ReferenceRect");
        ingredientPrefab = (PackedScene)ResourceLoader.Load("res://Instances/Ingredient.tscn");

        GetData();
        Vector2 startingPos = new Vector2(53, 192);
        foreach (FoodInfo food in FoodData)
        {
            Ingredient ingredient = (Ingredient)ingredientPrefab.Instance();
            ingredient.name = food.type;
            ingredient.SetPosition(startingPos);
            GetNode("ReferenceRect").AddChild(ingredient);
            startingPos.y += 96;
        }
        Button button = (Button)GetNode("ReferenceRect/Button");
        startingPos.x = 29;
        startingPos.y += 30;
        button.SetPosition(startingPos);
        NinePatchRect ninePatchRect = (NinePatchRect)GetNode("ReferenceRect/NinePatchRect");
        ninePatchRect.SetSize(new Vector2(ninePatchRect.GetSize().x, startingPos.y+102));

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
        if (@event.IsActionPressed("ui_exit"))
        {
            click.Play();
            SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
            switcher.ChangeScene("res://Scenes/Learn.tscn");
        }
    }
    void GetData()
    {
        File FoodDataFile = new File();
        FoodDataFile.Open("res://Data/FoodData.txt", 1);
        string line = FoodDataFile.GetLine();
        while (line != "")
        {
            string[] Data = line.Split(";");
            FoodInfo foodInfo = new FoodInfo(Data[0], Int32.Parse(Data[1]), float.Parse(Data[2]));
            FoodData.Add(foodInfo);
            line = FoodDataFile.GetLine();
        }
        FoodDataFile.Close();
    }

    private void Back()
    {
        click.Play();
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/Learn.tscn");
    }

}

