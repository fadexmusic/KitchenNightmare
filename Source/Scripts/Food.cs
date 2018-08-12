using Godot;
using System;

public class Food : Node2D
{
    
    
    Sprite sprite;
    public FoodInfo foodInfo;
    float rotProgress = 0;
    public bool rotten = false;
    public Slot.PLACE place;
    public float placeMultiplier = 1;
    public float rotMultiplier = 1;
    string SpritePath = "res://Sprites/Food/";
    Kitchen k;

    ProgressBar rotBar;
    public override void _Ready()
    {
        k = (Kitchen)GetNode("../../../Kitchen");
        rotBar = (ProgressBar)GetNode("ProgressBar");
        sprite = (Sprite)GetNode("Sprite");
        sprite.Texture = (Texture)ResourceLoader.Load(SpritePath + foodInfo.type + ".png");

        SetProcess(true);
    }
    public override void _Process(float delta)
    {
        SetZIndex(1);
        switch(place){
            case Slot.PLACE.FRIDGE:
                placeMultiplier = 1;
                //rotBar.Show();

                break;
            case Slot.PLACE.TABLE:
                placeMultiplier = 2;
                //rotBar.Show();

                break;
            case Slot.PLACE.PAN:
                placeMultiplier = 0;
                //rotBar.Hide();

                break;
            case Slot.PLACE.CURSOR:
                placeMultiplier = 1;
                SetZIndex(5);
                //rotBar.Show();

                break;
            default:
               //rotBar.Show();
                
                break;
        }
        if (!rotten)
        {
            rotProgress += (float).8 * delta * placeMultiplier * rotMultiplier * (float)k.hardnessMultiplier;

                rotBar.SetValue(rotProgress / foodInfo.RotTime * 100);
            if (rotProgress >= foodInfo.RotTime)
            {
                rotten = true; 
                sprite.SelfModulate = new Color((float)(72.0/255.0), (float)(191.0/255.0),(float)(132.0/255.0));
                GetNode("ProgressBar").QueueFree();
            }
        }

    }

}

public class FoodInfo{
    public string type;
    public Sprite sprite;
    public int RotTime;
    public float Price;
    public FoodInfo(string t, int r, float p){
        type = t;
        RotTime = r;
        Price = p;
    }
}