using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Table : Node2D
{
    PackedScene SlotPrefab;
    PackedScene FoodPrefab;

    List<FoodChance> foodChances = new List<FoodChance>();
    List<Slot> Slots = new List<Slot>();
    List<int> foodDeck = new List<int>();
    float SpawnCounter = 0;
    float SpawnTime = (float)2.5;
    int currentDeckIndex = 0;
    AudioStreamPlayer placeSound;
    Kitchen k;
    public override void _Ready()
    {
        placeSound = (AudioStreamPlayer)GetNode("Place");
        SlotPrefab = (PackedScene)ResourceLoader.Load("res://Instances/Slot.tscn");
        FoodPrefab = (PackedScene)ResourceLoader.Load("res://Instances/Food.tscn");

        k = (Kitchen)GetNode("../../Kitchen");

        foreach(FoodInfo fi in k.FoodData){
            foodChances.Add(new FoodChance(fi.type));
        }
        float totalFoodItems = 0;
        foreach(Recipe re in k.recipes){
            foreach(string ingredient in re.ingredients){
                totalFoodItems += 1;
                FoodChance result = foodChances.Find(x => x.name == ingredient);
                if(result!=null){
                    result.chance += 1;
                }
            }
        }
        foreach(FoodChance fc in foodChances){
            fc.chance /=totalFoodItems;
         }
        GenerateDeck();

        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                Slot NewSlot = (Slot)SlotPrefab.Instance();
                NewSlot.SetPosition(new Vector2((x * 64) + 32, (y * 64) + 32));
                NewSlot.place = Slot.PLACE.TABLE;
                Slots.Add(NewSlot);
                AddChild(NewSlot);
            }
        }
        SetProcess(true);

    }
    void GenerateDeck(){
        foodDeck.Clear();
        for (int foodIndex = 0; foodIndex < k.FoodData.Count; foodIndex++)
        {
            FoodChance result = foodChances.Find(x => x.name == k.FoodData[foodIndex].type);
            for (int oneFoodAmount = 0; oneFoodAmount < 10/*Math.Floor(result.chance*100)*/; oneFoodAmount++)
            {
                foodDeck.Add(foodIndex);
            }

        }
        Shuffle(foodDeck);

    }
    public override void _Process(float delta)
    {
        SpawnCounter += 1 * delta * (float)k.hardnessMultiplier;
        if(SpawnCounter>=SpawnTime){
            SpawnFood();
            SpawnCounter = 0;
        }

        // Called every frame. Delta is time since last frame.
        // Update game logic here.
        
    }

    void SpawnFood(){
        if (currentDeckIndex >= foodDeck.Count)
        {
            GenerateDeck();
            currentDeckIndex = 0;
        }

        bool spawned = false;

        Food NewFood = (Food)FoodPrefab.Instance();

        NewFood.foodInfo = k.FoodData[foodDeck[currentDeckIndex]];
        foreach (Slot slot in Slots)
        {
            if (slot.food == null)
            {
                
                AddChild(NewFood);
                slot.Drop(NewFood);
                spawned = true;

                break;
            }
        }
        if(!spawned){
            Kitchen kitchen = (Kitchen)GetNode("../../Kitchen");
            kitchen.Money -= NewFood.foodInfo.Price;
            Bin bin = (Bin)GetNode("../Bin");
        }
        placeSound.Play();
        currentDeckIndex++;


    }
   
    private Random rng = new Random();  
    public void Shuffle(List<int> list)
    {
       // RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            int value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }


}
class FoodChance{
    public string name;
    public double chance = 0;
    public FoodChance(string _name){
        name = _name;
    }
}