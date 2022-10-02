using Godot;
using System;

public class CameraManager : Node2D
{
  private Camera _camera;

  public override void _Ready()
  {
  }

  public void Init(Camera camera)
  {
    _camera = camera;
  }

  public void AddTrauma(float amount)
  {
    _camera.AddTrauma(amount);
  }
}
