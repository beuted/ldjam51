using Godot;
using System;

public class MuteButton : TextureButton
{
    private AudioManager _audioManager;
    private Texture _buttonNormalTexture;
    private Texture _buttonMuteTexture;
    private TextureRect _textureRect;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _textureRect = GetNode<TextureRect>("TextureRect");

        _buttonNormalTexture = ResourceLoader.Load<Texture>("res://assets/button-normal.png");
        _buttonMuteTexture = ResourceLoader.Load<Texture>("res://assets/button-mute.png");
        _audioManager = (AudioManager)GetNode($"/root/{nameof(AudioManager)}"); // Singleton
    }

    void OnClick() {
        if (_audioManager.ToggleStopMusic())
            _textureRect.Texture = _buttonNormalTexture;
        else
            _textureRect.Texture = _buttonMuteTexture;
    }

}