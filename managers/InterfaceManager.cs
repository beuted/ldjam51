using Godot;
using System;

public class InterfaceManager : Node2D
{
  private WaterManager _waterManager;
  private TomatoeManager _tomatoeManager;
  private AnimationPlayer _seedAnimationPlayer;
  private AnimationPlayer _tomatoeAnimationPlayer;

  private Label _scoreLabel;
  private Label _seedLabel;
  private Player _player;
  private ColorRect _startOverlay;
  private ColorRect _winOverlay;
  private ColorRect _looseOverlay;
  private ColorRect _notYetWinOverlay;
  private ColorRect _helpGrowOverlay;

  private RichTextLabel _startLabel;
  private RichTextLabel _winLabel;
  private RichTextLabel _looseLabel;

  private Node2D _mainNode;

  private Timer _timer;
  private Timer _timer2;
  private Timer _timer3;

  public int Score { get; private set; }
  public int Seed { get; private set; }

  public override void _Ready()
  {
    _tomatoeManager = (TomatoeManager)GetNode($"/root/{nameof(TomatoeManager)}"); // Singleton
    _waterManager = (WaterManager)GetNode($"/root/{nameof(WaterManager)}"); // Singleton

    _timer = new Timer();
    _timer.WaitTime = 0.05f;
    _timer.Autostart = false;
    AddChild(_timer);
    _timer.Connect("timeout", this, nameof(OnTimeout));

    _timer2 = new Timer();
    _timer2.WaitTime = 5f;
    _timer2.Autostart = false;
    AddChild(_timer2);
    _timer2.Connect("timeout", this, nameof(OnTimeout2));

    _timer3 = new Timer();
    _timer3.WaitTime = 5f;
    _timer3.Autostart = false;
    AddChild(_timer3);
    _timer3.Connect("timeout", this, nameof(OnTimeout3));
  }

  public override void _Input(InputEvent ev)
  {
    if (ev.IsActionPressed("plant") && (_looseOverlay.Visible || _winOverlay.Visible || _startOverlay.Visible))
    {
      _looseOverlay.Visible = false;

      _winOverlay.Visible = false;

      _startOverlay.Visible = false;

      _player.Position = _player.StartPosition;
      SetScore(0);
      SetSeed(3);

      foreach (var n in _mainNode.GetChildren()) {
        if (n is Tomatoe || n is Seed) {
          _mainNode.RemoveChild(n as Node);
          (n as Node).QueueFree();
        }
      }
      _waterManager.Reset();
      _tomatoeManager.ResetTomatoes();
    }
  }

  public void Init(Label scoreLabel, Label seedLabel, Player player, ColorRect startOverlay,
    ColorRect winOverlay, ColorRect looseOverlay, ColorRect notYetWinOverlay, ColorRect helpGrowOverlay, Node2D mainNode)
  {
    _scoreLabel = scoreLabel;
    _seedLabel = seedLabel;
    _player = player;
    _startOverlay = startOverlay;
    _startLabel = startOverlay.GetNode<RichTextLabel>("Label");
    _winOverlay = winOverlay;
    _winLabel = winOverlay.GetNode<RichTextLabel>("Label");
    _looseOverlay = looseOverlay;
    _looseLabel = looseOverlay.GetNode<RichTextLabel>("Label");
    _notYetWinOverlay = notYetWinOverlay;
    _helpGrowOverlay = helpGrowOverlay;
    _mainNode = mainNode;

    _seedAnimationPlayer = _mainNode.GetNode<AnimationPlayer>("CanvasLayer/Interface/AnimationPlayer");
    _tomatoeAnimationPlayer = _mainNode.GetNode<AnimationPlayer>("CanvasLayer/Interface/AnimationPlayer2");

    SetScore(0);
    SetSeed(3);
  }

  public void SetScore(int score)
  {
    Score = score;
    _scoreLabel.Text = score.ToString().PadLeft(3, '0');
    _tomatoeAnimationPlayer.Play("blink");
  }

  public void SetSeed(int seed)
  {
    Seed = seed;
    _seedLabel.Text = seed.ToString().PadLeft(2, '0');
    _seedAnimationPlayer.Play("blink");
  }

  public void IncrScore()
  {
    SetScore(Score+1);
    _tomatoeAnimationPlayer.Play("blink");
  }

  public void IncrSeed()
  {
    SetSeed(Seed+1);
    _seedAnimationPlayer.Play("blink");
  }

  public void DecrScore()
  {
    SetScore(Score-1);
    _tomatoeAnimationPlayer.Play("blink");
  }

  public void DecrSeed()
  {
    SetSeed(Seed-1);
    _seedAnimationPlayer.Play("blink");
  }

  public void DisplayStartOverlay() {
    _startOverlay.Visible = true;

    _winLabel.VisibleCharacters = 0;
    _startLabel.VisibleCharacters = 0;
    _looseLabel.VisibleCharacters = 0;
    _timer.Start();
  }

  public void OnTimeout() {
    _startLabel.VisibleCharacters += 1;
    _winLabel.VisibleCharacters += 1;
    _looseLabel.VisibleCharacters += 1;
    if (_startLabel.VisibleCharacters < 1000)
      _timer.Start();
  }

  public void DisplayWinOverlay() {
    _winOverlay.Visible = true;

    _winLabel.VisibleCharacters = 0;
    _startLabel.VisibleCharacters = 0;
    _looseLabel.VisibleCharacters = 0;
    _timer.Start();
  }

  public void DisplayLooseOverlay() {
    _looseOverlay.Visible = true;

    _winLabel.VisibleCharacters = 0;
    _startLabel.VisibleCharacters = 0;
    _looseLabel.VisibleCharacters = 0;
    _timer.Start();
  }

  public void DisplayNotYetWinOverlay() {
    _notYetWinOverlay.Visible = true;
    _timer2.Start();
  }

  public void DisplayGrowHelpOverlay() {
    if (_notYetWinOverlay.Visible || _looseOverlay.Visible || _winOverlay.Visible || _startOverlay.Visible)
      return;
    _helpGrowOverlay.Visible = true;
    _timer3.Start();
  }

  public void OnTimeout2() {
    _notYetWinOverlay.Visible = false;
  }

  public void OnTimeout3() {
    _helpGrowOverlay.Visible = false;
  }
}
