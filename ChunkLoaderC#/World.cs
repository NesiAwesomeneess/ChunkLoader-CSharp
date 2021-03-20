using Godot;
using System;
using System.Collections.Generic;

public class World : Node2D
{
    [Export]
    public NodePath playerPath;
    private Node2D player;

    [Export]
    public float chunkSize;
    [Export]
    public int renderDistance;
    [Export]
    private PackedScene chunkNode;

    [Export]
    public float revolutionDistance;
    [Export]
    public bool circumnavigatible = false;
    private Vector2 currentCoord;
    private List<ChunkNode> activeChunks = new List<ChunkNode>();
    private List<Vector2> activeCoords = new List<Vector2>();
    private Vector2 previousCoord = new Vector2();


//Functions
    public override void _Ready()
    {
        player = GetNode<Node2D>(playerPath);
        currentCoord = _GetCurrentChunk(player.Position);
        RenderChunks();
    }

    public override void _PhysicsProcess(float _delta)
    {
        //This way it only renders chunks when the player moves to a different chunk.
        currentCoord = _GetCurrentChunk(player.GlobalPosition);
        if (currentCoord != previousCoord){
            RenderChunks();
        }
        previousCoord = currentCoord;
    }

    private Vector2 _GetCurrentChunk(Vector2 pos){
        Vector2 coords;
        coords.y = ((int) (pos.y/chunkSize));
        coords.x = ((int) (pos.x/chunkSize));
        if (pos.x < 0){
            coords.x -= 1;
        }
        if (pos.y < 0){
            coords.y -= 1;
        }
        return coords;
    }

    //The RenderChunks function is used to render chunks duh.
    public void RenderChunks(){
        //The renderChunkNo is the number of chunks to render with the render distance given.
        float renderChunkNo = ((renderDistance*2f)+1f);
        List<Vector2> loadingChunks = new List<Vector2>();


        for (int _x = 1;_x <= renderChunkNo;_x++){
            float x = currentCoord.x + (_x - (Mathf.Ceil(renderChunkNo/2f)));
            for (int _y = 1;_y <= renderChunkNo;_y++){
                float y = currentCoord.y + (_y - (Mathf.Ceil(renderChunkNo/2f)));

                Vector2 chunkCoord = new Vector2(x, y);
                loadingChunks.Add(chunkCoord);

                if (activeCoords.IndexOf(chunkCoord) < 0){
                    ChunkNode chunk = chunkNode.Instance() as ChunkNode;
                    activeCoords.Add(chunkCoord);
                    activeChunks.Add(chunk);
                    
                    Vector2 chunkKey = _GetChunkKey(chunkCoord);
                    
                    chunk.GlobalPosition = chunkCoord * chunkSize;
                    AddChild(chunk);
                    chunk.Start(chunkKey);
                }
            }
        }

        List<Vector2> deletingCoords = new List<Vector2>();
        foreach (Vector2 coord in activeCoords){
            if (loadingChunks.IndexOf(coord) < 0){
                deletingCoords.Add(coord);
            }
        }
        foreach (Vector2 coord in deletingCoords){
            int index = activeCoords.IndexOf(coord);
            activeChunks[index].Save();
            activeChunks.RemoveAt(index);
            activeCoords.RemoveAt(index);
        }
    }

    private Vector2 _GetChunkKey(Vector2 coord){
        Vector2 key = coord;
        if (circumnavigatible == false){
            return key;
        }
        key.x = Mathf.Wrap(coord.x, -revolutionDistance, (revolutionDistance+1));
        return key;
    }
    
}
