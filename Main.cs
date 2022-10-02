using Godot;
using System;

public class Main : Node2D
{
  private Node2D _scene;
  private Node2D _ySort;

  private PackedScene _cameraScene;
  private PackedScene _playerScene;
  private PackedScene _waterScene;

  private CameraManager _cameraManager;
  private InterfaceManager _interfaceManager;
  private WaterManager _waterManager;
  private AudioManager _audioManager;

  public override void _Ready()
  {
    _scene = GetTree().Root.GetNode<Node2D>("Main");
    _ySort = _scene.GetNode<Node2D>("YSort");
    var tileMap = _scene.GetNode<TileMap>("TileMap");
    var scoreLabel = _scene.GetNode<Label>("CanvasLayer/Interface/TextureRect2/Label");
    var seedLabel = _scene.GetNode<Label>("CanvasLayer/Interface/TextureRect/Label");
    var startOverlay = _scene.GetNode<ColorRect>("CanvasLayer/Interface/StartOverlay");
    var winOverlay = _scene.GetNode<ColorRect>("CanvasLayer/Interface/WinOverlay");
    var looseOverlay = _scene.GetNode<ColorRect>("CanvasLayer/Interface/LooseOverlay");
    var notYetWinOverlay = _scene.GetNode<ColorRect>("CanvasLayer/Interface/NotYetWinOverlay");
    var growHelpOverlay = _scene.GetNode<ColorRect>("CanvasLayer/Interface/HelpGrowOverlay");
    var audioStreamPlayer = _scene.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    var audioStreamPlayerMoney = _scene.GetNode<AudioStreamPlayer>("AudioStreamPlayerMoney");
    var audioStreamPlayerBoup = _scene.GetNode<AudioStreamPlayer>("AudioStreamPlayerBoup");
    var audioStreamPlayerSplotch = _scene.GetNode<AudioStreamPlayer>("AudioStreamPlayerSplotch");
    var audioStreamPlayerClap = _scene.GetNode<AudioStreamPlayer>("AudioStreamPlayerClap");
    var audioStreamPlayerMusic = _scene.GetNode<AudioStreamPlayer>("AudioStreamPlayerMusic");


    _cameraScene = ResourceLoader.Load<PackedScene>("res://prefabs/MainCamera.tscn");
    _playerScene = ResourceLoader.Load<PackedScene>("res://prefabs/Player.tscn");
    _waterScene = ResourceLoader.Load<PackedScene>("res://prefabs/Water.tscn");

    _audioManager = (AudioManager)GetNode($"/root/{nameof(AudioManager)}"); // Singleton
    _cameraManager = (CameraManager)GetNode($"/root/{nameof(CameraManager)}"); // Singleton
    _waterManager = (WaterManager)GetNode($"/root/{nameof(WaterManager)}"); // Singleton
    _interfaceManager = (InterfaceManager)GetNode($"/root/{nameof(InterfaceManager)}"); // Singleton

    // set the camera on the current player
    var playerInstance = _playerScene.Instance() as Player;
    var cameraInstance = _cameraScene.Instance() as Camera;
    playerInstance.AddChild(cameraInstance);
    playerInstance.Position = playerInstance.StartPosition;
    playerInstance.Init(tileMap);
    _scene.AddChild(playerInstance);

    var waterInstance = _waterScene.Instance() as Water;
    waterInstance.Position = new Vector2(500, 560+16*4);
    _scene.AddChild(waterInstance);

    // Manager Init (Order matters)
    _audioManager.Init(audioStreamPlayer, audioStreamPlayerMoney, audioStreamPlayerBoup, audioStreamPlayerSplotch, audioStreamPlayerClap, audioStreamPlayerMusic);
    _waterManager.Init(waterInstance, tileMap);
    _interfaceManager.Init(scoreLabel, seedLabel, playerInstance, startOverlay, winOverlay, looseOverlay, notYetWinOverlay, growHelpOverlay, _scene);
    _cameraManager.Init(cameraInstance);

    // Display start Overlay
    _interfaceManager.DisplayStartOverlay();


  }

  public override void _Process(float delta)
  {
  }

}
