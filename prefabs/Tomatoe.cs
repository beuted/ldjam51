using Godot;
using System;

public class Tomatoe : Node2D
{
    private AudioManager _audioManager;
    private PackedScene _seedScene;
    private AnimationPlayer _animationPlayer;
    private WaterManager _waterManager;
    private TomatoeManager _tomatoeManager;
    private InterfaceManager _interfaceManager;

    private float GrowingTime = 5.0f;
    private float MaxDryTime = 3.0f;
    private float _dryTime = 0f;

    private float _duration = 10.0f;
    private float _grownTime = 0f;

    public int DiscretPosX;
    public int DiscretPosY;

    private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _audioManager = (AudioManager)GetNode($"/root/{nameof(AudioManager)}"); // Singleton
        _interfaceManager = (InterfaceManager)GetNode($"/root/{nameof(InterfaceManager)}"); // Singleton
        _tomatoeManager = (TomatoeManager)GetNode($"/root/{nameof(TomatoeManager)}"); // Singleton
        _waterManager = (WaterManager)GetNode($"/root/{nameof(WaterManager)}"); // Singleton
        _seedScene = ResourceLoader.Load<PackedScene>("res://prefabs/Seed.tscn");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _randomNumberGenerator.Randomize();
    }

    public override void _Process(float delta)
    {
        if (Position.y < _waterManager.AimedPosition - WaterManager.WaterHydratationTileDistance*16) {
            _animationPlayer.SetCurrentAnimation("Dry");

            if (_dryTime > MaxDryTime) {
                _interfaceManager.DisplayGrowHelpOverlay();
                QueueFree(); // Destroy plant
                _tomatoeManager.DeleteTomatoe(DiscretPosX, DiscretPosY);
                _interfaceManager.IncrSeed(); // Give back the seed to avoid dead lock
                _audioManager.PlaySplotchSound();
            }
            _dryTime += delta;
            return;
        }
        _dryTime = 0f;

        _animationPlayer.SetCurrentAnimation("Grow");
        _animationPlayer.Seek((_grownTime / GrowingTime) * 1.2f);

        _grownTime += delta;
        if (_grownTime > GrowingTime) {
            _audioManager.PlayBoupSound();
            SpawnSeed();
            SpawnSeed();
            QueueFree(); // Destroy plant
            _tomatoeManager.DeleteTomatoe(DiscretPosX, DiscretPosY);
        }
    }

    public void SpawnSeed() {
      // Range attack
      var seed = _seedScene.Instance() as Seed;
      seed.Position = Position + new Vector2(_randomNumberGenerator.RandfRange(-8f, 8f), 0);
      seed.ApplyCentralImpulse(new Vector2(_randomNumberGenerator.RandfRange(-10f, 10f), _randomNumberGenerator.RandfRange(0, -5f)));
      seed.ApplyTorqueImpulse(10000f);

      GetParent().AddChild(seed);
    }
}
