using Godot;
using System;
using System.Collections.Generic;

public class Bin : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    PackedScene SlotPrefab;
    List<Slot> Slots = new List<Slot>();
    ProgressBar progress;
    bool takingOut = false;
    float takingOutCounter = 0;
    float takeOutTime = 5;
    Sprite binLid;
    int fullness = 0;
    int capacity = 10;
    ProgressBar fullBar;
    Kitchen kitchen;
    AudioStreamPlayer trashOut;
    AudioStreamPlayer trashDone;
    AudioStreamPlayer throwOut;
    public override void _Ready()
    {
        trashOut = (AudioStreamPlayer)GetNode("TrashOut");
        trashDone = (AudioStreamPlayer)GetNode("TrashDone");
        throwOut = (AudioStreamPlayer)GetNode("ThrowOut");
        kitchen = (Kitchen)GetNode("../../Kitchen");
        binLid = (Sprite)GetNode("BinLid");
        SlotPrefab = (PackedScene)ResourceLoader.Load("res://Instances/Slot.tscn");
        progress = (ProgressBar)GetNode("ProgressBar");
        fullBar = (ProgressBar)GetNode("FullnessBar");
        // Called every time the node is added to the scene.
        // Initialization here
        Slot NewSlot = (Slot)SlotPrefab.Instance();
        NewSlot.SetPosition(new Vector2(32,32));
        NewSlot.place = Slot.PLACE.BIN;
        Slots.Add(NewSlot);
        AddChild(NewSlot);
        SetProcess(true);
    }
    private void TakeOut()
    {
        if(!takingOut){
            trashOut.Play();
            foreach (Slot s in Slots)
            {
                s.disabled = true;
            }
            takingOut = true;
            binLid.Show();
            fullBar.SetValue(0); 
        }

        // Replace with function body
    }

    public override void _Process(float delta)
    {
        if (takingOut){
            takingOutCounter += 1 * delta * (float)kitchen.hardnessMultiplier;
            if(takingOutCounter>=takeOutTime){
                foreach(Slot s in Slots){
                    s.disabled = false;
                }
                fullness = 0;
                fullBar.SetValue(0);
                takingOutCounter = 0;
                takingOut = false;
                trashDone.Play();
                binLid.Hide();
            }
            progress.SetValue(takingOutCounter / takeOutTime * 100);
        }
        // Called every frame. Delta is time since last frame.
        // Update game logic here.
        
    }
    public void ThrowOut(){

        fullness += 1;
        fullBar.SetValue((float)fullness / (float)capacity * 100);
        throwOut.Play();
        if(fullness>=capacity){
            foreach (Slot s in Slots)
            {
                s.disabled = true;
            }
        }
    }
}


