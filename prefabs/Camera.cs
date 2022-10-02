using Godot;
using System;

public class Camera : Camera2D
{
  private float _decay = 0.8f;
  private float _trauma = 0;
  private float _maxRoll = 0.1f;
  private Vector2 _maxOffset = new Vector2(100, 75);
  private float _traumaPower = 2;
  private float _noiseY = 0;
  private OpenSimplexNoise _noise = new OpenSimplexNoise();

  public override void _Ready()
  {
    _noise.Octaves = 2;
    _noise.Period = 4;
    _noise.Seed = System.Environment.TickCount;
  }

  public override void _Process(float delta)
  {
    if (_trauma > 0)
    {
      _trauma = Mathf.Max(_trauma - _decay * delta, 0);
      Shake();
    }
  }

  public void AddTrauma(float amount)
  {
    _trauma = Mathf.Min(_trauma + amount, 1.0f);
  }

  public void Shake()
  {
    var amount = Mathf.Pow(_trauma, _traumaPower);
    _noiseY += 1.0f;
    Rotation = _maxRoll * amount * _noise.GetNoise2d(_noise.Seed, _noiseY);
    Offset = new Vector2(
        _maxOffset.x * amount * _noise.GetNoise2d(_noise.Seed * 2, _noiseY),
        _maxOffset.y * amount * _noise.GetNoise2d(_noise.Seed * 3, _noiseY)
    );
  }
}
