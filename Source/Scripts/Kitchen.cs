using Godot;
using System;
using System.Collections.Generic;

public class Kitchen : Node2D
{

    private double money;
    public double Money{
        get{
            return this.money;
        }
        set{
            this.money = value;
            Label moneyLabel = (Label)GetNode("Money");
            moneyLabel.SetText(Math.Floor(money).ToString());

        }
    }
    public List<FoodInfo> FoodData = new List<FoodInfo>();
    public List<Recipe> recipes = new List<Recipe>();


    public int gameMode;
    public float time;
    public float endTime = 5*60;
    public double hardnessMultiplier = .8;
    public float timePlayed = 0;

    ProgressBar timeLeft;

    ReferenceRect pauseMenu;
    ReferenceRect endMenu;

    AudioStreamPlayer click;
    AudioStreamPlayer shiftEnd;
    AudioStreamPlayer pause;

    Label timeElapsed;

    public override void _EnterTree()
    {
        GetData();
        GetRecipes();
    }


    public override void _Ready()
    {
       
        File file = new File();
        shiftEnd = (AudioStreamPlayer)GetNode("ShiftEnd");
        click = (AudioStreamPlayer)GetTree().GetRoot().GetNode("SceneSwitcher/Click");
        pause = (AudioStreamPlayer)GetNode("Pause");
        if(!file.FileExists("res://Data/MoneyRecord")){
            file.Open("res://Data/MoneyRecord", 2);
            file.StoreDouble((float)0);
            file.Close();
        }
        if (!file.FileExists("res://Data/TimeRecord"))
        {
            file.Open("res://Data/TimeRecord", 2);
            file.StoreDouble((float)0);
            file.Close();
        }

        Global global = (Global)GetTree().GetRoot().GetNode("global");
        gameMode = global.gameMode;

        timeElapsed = (Label)GetNode("TimeElapsed");
        timeLeft = (ProgressBar)GetNode("ProgressBar");
        time = endTime;
        pauseMenu = (ReferenceRect)GetNode("PauseMenu");
        endMenu = (ReferenceRect)GetNode("EndMenu");
        Money = 100;
        switch(gameMode){
            case 0:
                timeLeft.Show();
                break;
            case 1:
                timeElapsed.Show();
                break;

        }
        SetProcess(true);
        // Called every time the node is added to the scene.
        // Initialization here


    }
    public override void _Process(float delta)
    {
        if(gameMode == 0){
            time -= 1 * delta;
            if (time <= 0)
            {
                endMenu.Show();
                Label cash = (Label)endMenu.GetNode("Node2D/Cash");
                if(GetMoneyRecord()<money){
                    Label recLabel = (Label)endMenu.GetNode("Node2D/RecordLabel");
                    recLabel.Show();
                    SetMoneyRecord(money);
                }
                if(money<=0){
                    cash.AddColorOverride("font_color", new Color((float)(221.0 / 225.0), (float)(45.0 / 225.0), (float)(73.0 / 225.0)));
                }
                cash.SetText(money.ToString());
                shiftEnd.Play();
                GetTree().SetPause(true);
            }
            timeLeft.SetValue(time / endTime * 100);

        }else if(gameMode == 1){
            timePlayed += 1 * delta;
            TimeSpan t = TimeSpan.FromSeconds(timePlayed);
            string str = t.ToString(@"mm\:ss");
            timeElapsed.SetText("cooking for: " + str);
            if(money<=0){
                endMenu.Show();
                Label cash = (Label)endMenu.GetNode("Node2D/Cash");
                Label earned = (Label)endMenu.GetNode("Node2D/Earned");
                if (GetTimeRecord() < timePlayed)
                {
                    Label recLabel = (Label)endMenu.GetNode("Node2D/RecordLabel");
                    recLabel.Show();
                    SetTimeRecord(timePlayed);
                }
                cash.SetText(str);
                earned.SetText("time: ");
                shiftEnd.Play();
                GetTree().SetPause(true);
            }
        }
    }
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_pause")){
            if(!GetTree().IsPaused()){
                pauseMenu.SetVisible(true);
                pause.Play();
                GetTree().SetPause(true);
            }else{
                pauseMenu.SetVisible(false);
                pause.Play();
                GetTree().SetPause(false);
            }

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
    double GetTimeRecord()
    {
        File RecordFile = new File();
        RecordFile.Open("res://Data/TimeRecord", 1);
        double record = 0;
        record = RecordFile.GetDouble();
        RecordFile.Close();
        return record;
    }
    void SetTimeRecord(double record)
    {
        File RecordFile = new File();
        RecordFile.Open("res://Data/TimeRecord", 2);
        RecordFile.StoreDouble((float)record);
        RecordFile.Close();
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
    void SetMoneyRecord(double record)
    {
        File RecordFile = new File();
        RecordFile.Open("res://Data/MoneyRecord", 2);
        RecordFile.StoreDouble((float)record);
        RecordFile.Close();
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
//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }

    private void Resume()
    {
       
        click.Play();
        pauseMenu.SetVisible(false);
        GetTree().SetPause(false);

    }


    private void Leave()
    {
        click.Play();
        GetTree().SetPause(false);

        SceneSwitcher switcher = (SceneSwitcher)GetTree().GetRoot().GetNode("SceneSwitcher");
        switcher.SwitchMusic(0);
        switcher.ChangeScene("res://Scenes/Menu.tscn");
        // Replace with function body
    }

}

