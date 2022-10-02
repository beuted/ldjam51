using Godot;
using System;

public class HomeCollider : Area2D
{
    private InterfaceManager _interfaceManager;
    private AudioManager _audioManager;
    private CameraManager _cameraManager;


    public override void _Ready()
    {
        _interfaceManager = (InterfaceManager)GetNode($"/root/{nameof(InterfaceManager)}"); // Singleton
        _audioManager = (AudioManager)GetNode($"/root/{nameof(AudioManager)}"); // Singleton
        _cameraManager = (CameraManager)GetNode($"/root/{nameof(CameraManager)}"); // Singleton
    }

    public void OnAreaBodyEntered(Node2D body)
    {
        if (body is Player player) {
            if (_interfaceManager.Score >= 100) {
                _interfaceManager.DisplayWinOverlay();
            } else if (_interfaceManager.Score > 0) {
                _interfaceManager.DisplayNotYetWinOverlay();
                var tomatoeToSwitch = Math.Min(Math.Max(0, 100 - _interfaceManager.Score - _interfaceManager.Seed), _interfaceManager.Score);
                _interfaceManager.SetSeed(_interfaceManager.Seed + tomatoeToSwitch);
                _interfaceManager.SetScore(_interfaceManager.Score - tomatoeToSwitch);

                if (tomatoeToSwitch > 0) {
                    _cameraManager.AddTrauma(0.2f);
                    _audioManager.PlayMoneySound();
                }
            }
        }
    }

}
