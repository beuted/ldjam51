using Godot;
using System;

public class Player : KinematicBody2D
{
  public Vector2 StartPosition = new Vector2(33*16, -54*16);

  private InterfaceManager _interfaceManager;
  private TomatoeManager _tomatoeManager;
  private AudioManager _audioManager;

  private PackedScene _tomatoeScene;
  private AnimationPlayer _animationPlayer;
  private Sprite _sprite;
  private TileMap _tileMap;

  private float _motionSpeed = 0;
  private const float _maxMotionSpeed = 130;
  private const float _horizontalAcceleration = 3000f;
  private const int _gravity = 20;
  private const int _jumpSpeed = 270;
  private bool _jumping = false;
  private bool _jump = false;
  private bool _isRooted = false;
  private bool _airJumpAvailable = false;
  private float _lastTimeRooted = 0f; // Coyote time
  private float CoyoteTimeLimit = 0.15f; // Coyote time limit

  private Vector2 speed = Vector2.Zero;

  private float _inputDirection;

  public void Init(TileMap tileMap) {
      _tileMap = tileMap;
  }

  public override void _Ready()
  {
    _interfaceManager = (InterfaceManager)GetNode($"/root/{nameof(InterfaceManager)}"); // Singleton
    _tomatoeManager = (TomatoeManager)GetNode($"/root/{nameof(TomatoeManager)}"); // Singleton
    _audioManager = (AudioManager)GetNode($"/root/{nameof(AudioManager)}"); // Singleton

    _tomatoeScene = ResourceLoader.Load<PackedScene>("res://prefabs/Tomatoe.tscn");
    _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    _sprite = GetNode<Sprite>("Sprite");
  }

  public override void _Input(InputEvent ev)
  {
    _inputDirection = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

    if (ev.IsActionPressed("jump"))
    {
      if (!_jumping)
      {
        _jump = true;
      }
      _jumping = true;
    }

    if (ev.IsActionReleased("jump"))
    {
      _jumping = false;
    }

    if (ev.IsActionPressed("plant") && _isRooted && _interfaceManager.Seed > 0)
    {
      SpawnTomatoe();
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    // Movements
    var tweenParam = Mathf.Min(1.0f, Mathf.Max(0, delta*60));
    // Friction if _inputDirection = 0
    //TODO: delta should be involved in the friction here
    if (speed.x >= 0)
      speed.x = Mathf.Max(0.0f, speed.x - 2000f*delta);
    else
      speed.x = Mathf.Min(0.0f, speed.x + 2000f*delta);


    // We increase progressively the speed not suddenly
    speed.x = Mathf.Max(-_maxMotionSpeed, Mathf.Min(_maxMotionSpeed, speed.x + delta * _horizontalAcceleration * _inputDirection));

    if (_inputDirection < 0)
    {
      _sprite.FlipH = true;
      _animationPlayer.Play("Walk");
    }
    else if (_inputDirection > 0)
    {
      _sprite.FlipH = false;
      _animationPlayer.Play("Walk");
    }
    else
    {
      _animationPlayer.Play("Idle");
    }

    // Gravity
    speed.y += _gravity;

    // Jump
    if (_jump && (_isRooted || _lastTimeRooted < CoyoteTimeLimit || _airJumpAvailable))
    {
      speed.y = -_jumpSpeed;
      if (!_isRooted && _lastTimeRooted >= CoyoteTimeLimit)
        _airJumpAvailable = false;
      if (_isRooted || _lastTimeRooted < CoyoteTimeLimit)
        _lastTimeRooted = 10000f; // disable coyote time until we touch floor again (if we used it or if we jumped from the floor)
    }


    // Jump Pro tip: inverted gravity
    if (_jumping) {
      speed.y -= _gravity / 3;

      if (speed.y < 100) {
        _animationPlayer.Play("Jump");
      }
    }

    if (speed.y > 100) {
      _animationPlayer.Play("Down");
    }

    speed = MoveAndSlide(speed, Vector2.Up);

    _isRooted = IsOnFloor();
    if (_isRooted) {
      _airJumpAvailable = true;
      _lastTimeRooted = 0;
    } else {
      _lastTimeRooted+=delta;
    }

    // If we succeed to jump or not whatever just set back to false and way for next jump button press
    _jump = false;

  }

  public void SpawnTomatoe() {
    var discretPosPlotX = (int)(Mathf.Floor((Position.x)/16));
    var discretPosPlotY = (int)(Mathf.Floor((Position.y+16+16)/16));

    if (_tileMap.GetCell(discretPosPlotX, discretPosPlotY) == -1 || _tomatoeManager.GetTomatoe(discretPosPlotX, discretPosPlotY) != null)
      return;

    var tomatoePos = new Vector2(((int)(Mathf.Floor((Position.x)/16)))*16f, ((int)(Mathf.Floor((Position.y+16)/16)))*16f);

    var tomatoe = _tomatoeScene.Instance() as Tomatoe;
    tomatoe.Position = tomatoePos;
    tomatoe.DiscretPosX = discretPosPlotX;
    tomatoe.DiscretPosY = discretPosPlotY;

    _tomatoeManager.SetTomatoe(discretPosPlotX, discretPosPlotY, tomatoe);

    GetParent().AddChild(tomatoe);

    _interfaceManager.DecrSeed();
    _audioManager.PlayClapSound();
  }
}