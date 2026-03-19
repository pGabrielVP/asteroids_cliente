using Godot;
using System.Runtime.InteropServices.JavaScript;

public partial class StartScreen : Control
{
    [Export]
    PackedScene _jogo;
    [Export]
    Button _inicar;
    [Export]
    Button _sair;
    public void OnIniciarJogoPressed()
    {
        GetTree().ChangeSceneToFile(_jogo.ResourcePath);
    }
    public void OnSairPressed()
    {
        GetTree().Quit();
    }
}
