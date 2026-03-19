using Godot;

public partial class StartScreen : Control
{
    [Export]
    PackedScene _jogo;
    public void OnIniciarJogoPressed()
    {
        GetTree().ChangeSceneToFile(_jogo.ResourcePath);
    }
    public void OnSairPressed()
    {
        GetTree().Quit();
    }
}
