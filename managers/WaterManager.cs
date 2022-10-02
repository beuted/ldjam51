using Godot;
using System;
using System.Collections.Generic;

public class WaterManager : Node2D
{
  public static int WaterHydratationTileDistance = 4;
  public float AimedPosition { get; set; } // Horrible mais pas le temps

  private CameraManager _cameraManager;
  private AudioManager _audioManager;

  private Vector2 _startPosition = new Vector2(500f, 0-16*50);
  private TileMap _tileMap;
  private Area2D _water;

  private Timer _timer;
  private float _duration = 10f; // Must be 10sec
  private float _speedTween = 20.0f;

  private Dictionary<int, int> _hydratedCellMapping = new Dictionary<int, int>() {
    {0, 13},
    {1, 14},
    {2, 15},
    {12, 16},
    {11, 20}
  };

  private Dictionary<int, int> _hydratedCellInvertMapping = new Dictionary<int, int>() {
    {13, 0},
    {14, 1},
    {15, 2},
    {16, 12},
    {20, 11}
  };

  public override void _Ready()
  {
    _cameraManager = (CameraManager)GetNode($"/root/{nameof(CameraManager)}"); // Singleton
    _audioManager = (AudioManager)GetNode($"/root/{nameof(AudioManager)}"); // Singleton

    _timer = new Timer();
    _timer.WaitTime = _duration;
    _timer.Autostart = true;
    AddChild(_timer);
    _timer.Connect("timeout", this, nameof(OnTimeout));

    AimedPosition = _startPosition.y;
  }

  public override void _Process(float delta)
  {
    if (_water.Position.y < AimedPosition) {
        var newPosY = Mathf.Max(_water.Position.y, _water.Position.y + (delta * _speedTween));
        _water.Position = new Vector2(_water.Position.x, newPosY);
    }
  }

  public void Init(Area2D water, TileMap tileMap) {
    _water = water;
    _tileMap = tileMap;
    _water.Position = new Vector2(_startPosition.x, _startPosition.y);
  }

  public void Reset() {
    AimedPosition = _startPosition.y;
    _water.Position = new Vector2(_startPosition.x, _startPosition.y);

    // Reste the tileMap
    for (var x=0; x < 64; x++) {
      for (var y=-60; y < 60; y++) {
        var cell = _tileMap.GetCell(x, y);
        if (cell != -1 && _hydratedCellInvertMapping.TryGetValue(cell, out var matchingCell)) {
          _tileMap.SetCell(x, y, matchingCell);
        }
      }
    }

    _timer.Start();
  }

  public void OnTimeout() {
    _cameraManager.AddTrauma(0.3f);
    _audioManager.PlayWaveSound();
    AimedPosition = AimedPosition + 2 * 16f; // Go down from 2 steps
    _timer.Start(_duration);

    // Make the cell go greener
    // 64 is larger than the screen size in tiles
    int row = (int)(Mathf.Floor(AimedPosition / 16f)) - WaterHydratationTileDistance;
    for (var y=-60; y < row; y++) {
      for (var x=0; x < 60; x++) {
        var cell = _tileMap.GetCell(x, y);
        if (cell != -1 && _hydratedCellMapping.TryGetValue(cell, out var matchingCell)) {
          _tileMap.SetCell(x, y, matchingCell);
        }
      }
    }
  }

  private float RangeLerp(float value, float istart, float istop, float ostart, float ostop)
  {
    return ostart + (ostop - ostart) * value / (istop - istart);
  }
}
