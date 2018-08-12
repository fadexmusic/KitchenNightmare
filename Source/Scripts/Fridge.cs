using Godot;
using System;
using System.Collections.Generic;

public class Fridge : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    PackedScene SlotPrefab;
    PackedScene FoodPrefab;

    Random random = new Random();

    List<Slot> Slots = new List<Slot>();


    public override void _Ready()
    {
        
        SlotPrefab = (PackedScene)ResourceLoader.Load("res://Instances/Slot.tscn");
        FoodPrefab = (PackedScene)ResourceLoader.Load("res://Instances/Food.tscn");

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                Slot NewSlot = (Slot)SlotPrefab.Instance();
                NewSlot.SetPosition(new Vector2((x * 64) + 32, (y * 64) + 32));
                NewSlot.place = Slot.PLACE.FRIDGE;
                Food NewFood = (Food)FoodPrefab.Instance();
                AddChild(NewSlot);
            }
        }

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Slot NewSlot = (Slot)SlotPrefab.Instance();
                NewSlot.SetPosition(new Vector2((x * 64) + 32, (y * 64) + 352 + 8));
                NewSlot.place = Slot.PLACE.FRIDGE;
                Food NewFood = (Food)FoodPrefab.Instance();
                AddChild(NewSlot);
            }
        }

        SetProcess(true);
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

    public override void _Process(float delta)
    {
       
        // Called every frame. Delta is time since last frame.
        // Update game logic here.
        
    }

}