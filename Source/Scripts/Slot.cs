using Godot;
using System;


public class Slot : Area2D
{
    public enum PLACE
    {
        FRIDGE, TABLE, PAN, BIN, CURSOR
    }
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public Food food;
    public PLACE place;
    public bool disabled = false;

    public double throwMultiplier = .5;

    Area2D left;
    Area2D right;



    public override void _Ready()
    {
        left = (Area2D)GetNode("Left");
        right = (Area2D)GetNode("Right");

        SetProcess(true);
        // Called every time the node is added to the scene.
        // Initialization here
        
    }


    public override void _Process(float delta)
    {
        if(food!=null){
            if(!food.rotten){
                float rotMultiplier = 1;
                Godot.Array overlappingLeft = left.GetOverlappingAreas();

                if (overlappingLeft.Count > 0)
                {
                    Slot slotLeft = (Slot)overlappingLeft[0];
                    if (slotLeft.food != null)
                    {
                        if (slotLeft.food.rotten)
                        {

                            rotMultiplier = 2;

                        }
                    }
                }
                Godot.Array overlappingRight = right.GetOverlappingAreas();

                if (overlappingRight.Count > 0)
                {
                    Slot slotRight = (Slot)overlappingRight[0];
                    if (slotRight.food != null)
                    {
                        if (slotRight.food.rotten)
                        {

                            rotMultiplier = 2;

                        }
                    }
                }
                if (food != null)
                {
                    food.rotMultiplier = rotMultiplier;
                }
            }
        }


       

    }

    public void Drop(Food f)
    {
        food = f;
        food.place = place;
        food.SetGlobalPosition(new Vector2(GetGlobalPosition().x-1,GetGlobalPosition().y-1));
        if(place == PLACE.BIN){
            Throw();
        }
    }

    public void Throw()
    {
        if(food!=null){
            Kitchen kitchen = (Kitchen)GetNode("../../../Kitchen");
            kitchen.Money -= food.foodInfo.Price * throwMultiplier;
            food.QueueFree();
            food = null;
            Bin bin = (Bin)GetNode("../../Bin");
            bin.ThrowOut();
        }
    }
    public float Sell()
    {
        if (food != null)
        {
            Kitchen kitchen = (Kitchen)GetNode("../../../Kitchen");
            float price = food.foodInfo.Price;
            food.QueueFree();
            food = null;
            return price;
        }
        return 0;
    }
}

