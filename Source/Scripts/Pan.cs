using Godot;
using System;
using System.Collections.Generic;

public class Pan : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    PackedScene SlotPrefab;
    List<Slot> Slots = new List<Slot>();
    bool cooking = false;
    ProgressBar cookBar;
    double cookProgress = 0;
    Recipe currentRecipe;
    Label currentlyCookingLabel;
    Sprite lid;
    double profit = 1.4;
    Kitchen k;
    AudioStreamPlayer startCooking;
    AudioStreamPlayer cookingDone;

    public override void _Ready()
    {
        startCooking = (AudioStreamPlayer)GetNode("StartCooking");
        cookingDone = (AudioStreamPlayer)GetNode("DoneCooking");
        k = (Kitchen)GetNode("../../Kitchen"); 
        cookBar = (ProgressBar)GetNode("ProgressBar");
        currentlyCookingLabel = (Label)GetNode("CurrentlyCooking");
        SlotPrefab = (PackedScene)ResourceLoader.Load("res://Instances/Slot.tscn");
        lid = (Sprite)GetNode("Lid");
        // Called every time the node is added to the scene.
        // Initialization here
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                Slot NewSlot = (Slot)SlotPrefab.Instance();
                NewSlot.SetPosition(new Vector2((x * 64) + 32, (y * 64) + 32));
                NewSlot.place = Slot.PLACE.PAN;
                Slots.Add(NewSlot);
                AddChild(NewSlot);
            }
        }
    }
    public override void _Process(float delta)
    {
        if(cooking){
            cookProgress+=1*delta*(float)k.hardnessMultiplier;
            cookBar.SetValue((float)cookProgress / currentRecipe.cookTime * 100);
            if(cookProgress>=currentRecipe.cookTime){
                cooking = false;
                currentRecipe = null;
                cookProgress = 0;
                //add money
                double price = 0;
                foreach(Slot s in Slots){
                    price+=s.Sell();
                }
                double profited = (price*profit) - price;
                Kitchen kitchen = (Kitchen)GetNode("../../Kitchen");
                kitchen.Money += profited;
                cookBar.SetValue(0);
                currentlyCookingLabel.SetText("nothing");
                foreach (Slot s in Slots)
                {
                    s.disabled = false;
                }
                cookingDone.Play();
                lid.Hide();
            }
        }
    }

   
    public void Cook(){
        if (!cooking){
            
            List<string> ingredients = new List<string>();
            int slotsFull = 0;
            foreach(Slot s in Slots){
                if(s.food!=null){
                    ingredients.Add(s.food.foodInfo.type);
                    slotsFull += 1;
                }
            }

            List<ScoredRecipe> recipeScores = new List<ScoredRecipe>();
            foreach(Recipe toSearch in k.recipes){
                
                ScoredRecipe currentScoredRecipe = new ScoredRecipe(toSearch, 0);

                string[] toSearchIngredients = (string[])toSearch.ingredients.Clone();

                foreach (string foodType in ingredients)
                {
                    for (int i = toSearchIngredients.Length-1; i >= 0; i--)
                    {
                        if (toSearchIngredients[i] == foodType)
                        {
                            
                            currentScoredRecipe.score += 1;
                            toSearchIngredients[i] = "x";
                            break;
                        }
                    }
                }
                recipeScores.Add(currentScoredRecipe);
            }
            ScoredRecipe bestScoredRecipe = null;
            foreach(ScoredRecipe sr in recipeScores){
                
                if (bestScoredRecipe == null)
                {
                    bestScoredRecipe = sr;
                }else{
                    if(sr.score>bestScoredRecipe.score){
                        bestScoredRecipe = sr;
                    }
                }
            }
            GD.Print(slotsFull);
            if (bestScoredRecipe.score != bestScoredRecipe.recipe.ingredients.Length){
                foreach (ScoredRecipe sr in recipeScores)
                {
                    if (sr.score == sr.recipe.ingredients.Length && sr.score == slotsFull)
                    {
                        bestScoredRecipe = sr;
                        break;
                    }
                }

            }

            if(bestScoredRecipe.score==bestScoredRecipe.recipe.ingredients.Length && bestScoredRecipe.score == slotsFull){
                
                currentRecipe = bestScoredRecipe.recipe;
                cooking = true;
                currentlyCookingLabel.SetText(currentRecipe.name);
                foreach(Slot s in Slots){
                    s.disabled = true;
                }
                lid.Show();
                startCooking.Play();
            }
        }
    }


}

class ScoredRecipe
{
    public Recipe recipe;
    public int score;
    public ScoredRecipe(Recipe _recipe, int _score)
    {
        recipe = _recipe;
        score = _score;
    }
}
public class Recipe
{
    public string name;
    public string[] ingredients;
    public float cookTime;
    public Recipe(string _name, string[] _ingredients, float _cookTime)
    {
        name = _name;
        ingredients = _ingredients;
        cookTime = _cookTime;
    }

}