using Godot;
using System;

public class Player : KinematicBody2D
{
    private float speed = 5000f;
    private Vector2 direction = new Vector2();
    private Vector2 velocity = new Vector2();
    public override void _PhysicsProcess(float delta)
    {
        direction = new Vector2();
        if (Input.IsActionPressed("ui_left")){
            direction.x = -1;
        }
        else if (Input.IsActionPressed("ui_right")){
            direction.x = 1;
        }
        if (Input.IsActionPressed("ui_up")){
            direction.y = -1;
        }
        else if (Input.IsActionPressed("ui_down")){
            direction.y = 1;
        }

        velocity = direction.Normalized() * speed * delta;
        velocity = MoveAndSlide(velocity);
    }
}
