using Godot;
using System;

public class SceneSwitcher : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    public Godot.Node currentScene;

    AudioStreamPlayer soundtrack;
    AudioStreamPlayer soundtrackMenu;
    public override void _Ready()
    {

        soundtrack = (AudioStreamPlayer)GetNode("Soundtrack");
        soundtrackMenu = (AudioStreamPlayer)GetNode("SoundtrackMenu");

        
        ChangeScene("res://Scenes/Menu.tscn");
        // Called every time the node is added to the scene.
        // Initialization here
       
        soundtrackMenu.Play();

    }
    public void SwitchMusic(int what){
        switch(what){
            case 0:
                soundtrackMenu.Play();
                soundtrack.Stop();

                break;
            case 1:
                soundtrackMenu.Stop();
                soundtrack.Play();
                break;
        }
    }
    public void ChangeScene(string path){
        
        if(currentScene!=null){    
            currentScene.QueueFree();
        }
        PackedScene scenePath = (PackedScene)ResourceLoader.Load(path);
        Godot.Node newScene = scenePath.Instance();
        currentScene = newScene;
        AddChild(newScene);
    }
  
//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
}
