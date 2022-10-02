using Godot;
using System;

public class Water : Area2D
{
    private InterfaceManager _interfaceManager;

    public override void _Ready()
    {
        _interfaceManager = (InterfaceManager)GetNode($"/root/{nameof(InterfaceManager)}"); // Singleton
    }

    public void OnAreaBodyEntered(Node2D body)
    {
        if (body is Player player) {
            _interfaceManager.DisplayLooseOverlay();
        }
    }
}
