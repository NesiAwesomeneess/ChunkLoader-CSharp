using Godot;
using System;
using System.Collections.Generic;

public class ChunkDataSaver : Node
{
    public List<Vector2> chunkKeys = new List<Vector2>();
    public List<List<Array>> chunkData = new List<List<Array>>();

    public void Add(Vector2 key, List<Array> data){
        // GD.Print(key);
        if (chunkKeys.IndexOf(key) < 0){
            chunkData.Add(data);
            chunkKeys.Add(key);
        }
    }

    public List<Array> Retrieve(Vector2 key){
        List<Array> Data = new List<Array>();
        Data = chunkData[chunkKeys.IndexOf(key)];
        return Data;
    }

    public void Save(Vector2 key, List<Array> data){
        chunkData[chunkKeys.IndexOf(key)] = data;
    }

}
