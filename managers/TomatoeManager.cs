using Godot;
using System;
using System.Collections.Generic;


public class TomatoeManager : Node
{
    public Dictionary<(int, int), Tomatoe> _tomatoes = new Dictionary<(int, int), Tomatoe>() {
    };
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public Tomatoe GetTomatoe(int discretPosPlotX, int discretPosPlotY) {
        if (!_tomatoes.TryGetValue((discretPosPlotX, discretPosPlotY), out var t))
            return null;
        return t;
    }

    public void SetTomatoe(int discretPosPlotX, int discretPosPlotY, Tomatoe tomatoe) {
        _tomatoes[(discretPosPlotX, discretPosPlotY)] = tomatoe;
    }

    public void DeleteTomatoe(int discretPosPlotX, int discretPosPlotY) {
        _tomatoes.Remove((discretPosPlotX, discretPosPlotY));
    }

    public void ResetTomatoes() {
        _tomatoes =new Dictionary<(int, int), Tomatoe>() {};
    }
}
