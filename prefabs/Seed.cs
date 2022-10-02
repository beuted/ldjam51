using Godot;
using System;

public class Seed : RigidBody2D
{
    private InterfaceManager _interfaceManager;
    private AudioManager _audioManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _interfaceManager = (InterfaceManager)GetNode($"/root/{nameof(InterfaceManager)}"); // Singleton
        _audioManager = (AudioManager)GetNode($"/root/{nameof(AudioManager)}"); // Singleton
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerHitbox playerHitbox) {
            _interfaceManager.IncrScore();
            _audioManager.PlayBoupSound();
            QueueFree(); // Destroy tomatoe
        }
    }
}
