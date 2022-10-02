using Godot;
using System;

public class AudioManager : Node2D
{
  private AudioStreamPlayer _audioStreamPlayerWave;
  private AudioStreamPlayer _audioStreamPlayerMoney;
  private AudioStreamPlayer _audioStreamPlayerBoup;
  private AudioStreamPlayer _audioStreamPlayerSplotch;
  private AudioStreamPlayer _audioStreamPlayerClap;
  private AudioStreamPlayer _audioStreamPlayerMusic;

  public override void _Ready()
  {
  }

  public void Init(AudioStreamPlayer audioStreamPlayerWave, AudioStreamPlayer audioStreamPlayerMoney, AudioStreamPlayer audioStreamPlayerBoup,
    AudioStreamPlayer audioStreamPlayerSplotch, AudioStreamPlayer audioStreamPlayerClap, AudioStreamPlayer audioStreamPlayerMusic)
  {
    _audioStreamPlayerWave = audioStreamPlayerWave;
    _audioStreamPlayerMoney = audioStreamPlayerMoney;
    _audioStreamPlayerBoup = audioStreamPlayerBoup;
    _audioStreamPlayerSplotch = audioStreamPlayerSplotch;
    _audioStreamPlayerClap = audioStreamPlayerClap;
    _audioStreamPlayerMusic = audioStreamPlayerMusic;

    PlayMusic();
  }

  public void PlayWaveSound() {
    _audioStreamPlayerWave.Play();
  }

  public void PlayMoneySound() {
    _audioStreamPlayerMoney.Play();
  }

  public void PlayBoupSound() {
    _audioStreamPlayerBoup.Play();
  }

  public void PlaySplotchSound() {
    _audioStreamPlayerSplotch.Play();
  }
  public void PlayClapSound() {
    _audioStreamPlayerClap.Play();
  }

  public void PlayMusic() {
    _audioStreamPlayerMusic.Play();
  }

  public void StopMusic() {
    _audioStreamPlayerMusic.Stop();
  }

  public bool ToggleStopMusic() {
    if (_audioStreamPlayerMusic.IsPlaying()) {
      _audioStreamPlayerMusic.Stop();
      return false;
    } else {
      _audioStreamPlayerMusic.Play();
      return true;
    }
  }
}
