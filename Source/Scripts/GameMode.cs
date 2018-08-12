using Godot;
using System;

public class GameMode : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    Label timedRecord;
    Label infiniteRecord;
    AudioStreamPlayer click;
    public override void _Ready()
    {
        click = (AudioStreamPlayer)GetTree().GetRoot().GetNode("SceneSwitcher/Click");
        timedRecord = (Label)GetNode("ReferenceRect/TimedRecord");
        infiniteRecord = (Label)GetNode("ReferenceRect/InfiniteRecord");
        // Called every time the node is added to the scene.
        // Initialization here
        timedRecord.SetText("record: " + GetMoneyRecord());
        TimeSpan t = TimeSpan.FromSeconds(GetTimeRecord());
        string str = t.ToString(@"mm\:ss");
        infiniteRecord.SetText("record: " + str);
        
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
        // Replace with function body
    }


    private void Timed()
    {
        click.Play();
        Global global = (Global)GetTree().GetRoot().GetNode("global");
        global.gameMode = 0;
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.SwitchMusic(1);
        switcher.ChangeScene("res://Scenes/Kitchen.tscn");
        // Replace with function body
    }


    private void Infinite()
    {
        click.Play();
        Global global = (Global)GetTree().GetRoot().GetNode("global");
        global.gameMode = 1;
        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.ChangeScene("res://Scenes/Kitchen.tscn");
        switcher.SwitchMusic(1);
        // Replace with function body
    }
    double GetTimeRecord()
    {
        File RecordFile = new File();
        RecordFile.Open("res://Data/TimeRecord", 1);
        double record = 0;
        record = RecordFile.GetDouble();
        RecordFile.Close();
        return record;
    }

    double GetMoneyRecord()
    {
        File RecordFile = new File();
        RecordFile.Open("res://Data/MoneyRecord", 1);
        double record = 0;
        record = RecordFile.GetDouble();
        RecordFile.Close();
        return record;
    }
}


