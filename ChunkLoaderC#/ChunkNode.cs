using Godot;
using System;
using System.Collections.Generic;

public class ChunkNode : Node2D
{
    private List<Array> chunkData = new List<Array>();
    private Vector2 chunkKey; 

//color array 0
    private Color color;
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private ColorRect sprite;
    private ChunkDataSaver chunkDataSaver;
    private Label label;

    public override void _Ready()
    {
        rng.Randomize();
        sprite = GetNode("Sprite") as ColorRect;
        chunkDataSaver = GetNode("/root/ChunkDataSaver") as ChunkDataSaver;
        label = GetNode("Label") as Label;
    }

    //Everything in the chunkData list has to be an Array
    public void Start(Vector2 key){
        chunkKey = key;
        label.Text = GD.Str(chunkKey);
        
        if (chunkDataSaver.chunkKeys.IndexOf(chunkKey) < 0){
            chunkDataSaver.Add(chunkKey, chunkData);
            color = new Color(rng.RandfRange(0.2f, 1f), rng.RandfRange(0.2f, 1f), rng.RandfRange(0.2f, 1f));
            //Creating an array to store the color variable
            Color[] colors = {color}; chunkData.Add((colors));
        }
        else {
            chunkData = chunkDataSaver.Retrieve(chunkKey);
            color = ((chunkData[0] as Color[])[0]);
        }
        sprite.Color = color;
    }

    public void Save(){
        chunkDataSaver.Save(chunkKey, chunkData);
        QueueFree();
    }

}
