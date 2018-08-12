using Godot;
using System;
using Object = Godot.Object;

public class Cursor : Area2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    public Food food;

    Slot OverSlot;
    Slot lastSlot;
    AudioStreamPlayer pickupSound;
    AudioStreamPlayer placeSound;
    AudioStreamPlayer placeErrorSound;

    public override void _Ready()
    {
        SetProcess(true);
        pickupSound = (AudioStreamPlayer)(GetNode("PickupSound"));
        placeSound = (AudioStreamPlayer)(GetNode("PlaceSound"));
        placeErrorSound = (AudioStreamPlayer)(GetNode("PlaceErrorSound"));
        // Called every time the node is added to the scene.
        // Initialization here

    }

    public override void _Process(float delta)
    {
        SetPosition(GetGlobalMousePosition());
        if(food!=null){
            food.SetGlobalPosition(Position);
        }

    }

    public override void _Input(InputEvent @event)
    {
        Godot.Array areas = GetOverlappingAreas();
        if (areas.Count > 0)
        {
            
            OverSlot = (Slot)areas[0];
        }else{
            OverSlot = null;
        }
        if (@event.IsActionPressed("ui_mb_left"))
        {
            if (OverSlot != null && !OverSlot.disabled)
                {
                if(OverSlot.place == Slot.PLACE.PAN && food!=null && food.rotten){
                    placeErrorSound.Play();

                }else{


                    if (OverSlot.food != null && food == null)
                    {
                        food = OverSlot.food;
                        OverSlot.food = null;
                        food.place = Slot.PLACE.CURSOR;
                        lastSlot = OverSlot;
                        pickupSound.Play();
                    }

                }
            }


        }
        if (@event.IsActionReleased("ui_mb_left"))
        {
            if (OverSlot != null && !OverSlot.disabled)
            {
                if (OverSlot.place == Slot.PLACE.PAN && food != null && food.rotten)
                {
                    placeErrorSound.Play();
                }
                else
                {
                    if (OverSlot.food == null && food != null)
                    {
                        OverSlot.Drop(food);
                        food = null;
                        if(OverSlot.place!=Slot.PLACE.BIN)
                        {
                            placeSound.Play();
                            
                        }
                    }
                    else if (OverSlot.food == null && food != null)
                    {
                        OverSlot.Drop(food);
                        food = null;
                        if (OverSlot.place != Slot.PLACE.BIN)
                        {
                            placeSound.Play();

                        }
                    }
                }
            }else if(OverSlot!=null && OverSlot.disabled){
                placeErrorSound.Play();
            }

            if(OverSlot == null && lastSlot!=null && lastSlot.food==null && food!=null){
                placeErrorSound.Play();
                lastSlot.Drop(food);
                food = null;
                lastSlot = null;
            }
        }
        //if(@event.IsActionPressed("ui_mb_right")){
        //    if (OverSlot != null && !OverSlot.disabled)
        //    {
        //        if (OverSlot.place == Slot.PLACE.PAN && food != null && food.rotten)
        //        {

        //        }
        //        else
        //        {
        //            if (OverSlot.food != null && food != null)
        //            {
        //                Food temp = food;
        //                food = OverSlot.food;
        //                food.place = Slot.PLACE.CURSOR;
        //                lastSlot = OverSlot;
        //                OverSlot.Drop(temp);

        //            }
        //        }
        //    }
           
        //}
    }
   
}



