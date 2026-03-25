using Godot;
using System.Globalization;

public partial class Jogo : Node2D
{
    
    [Export]
    private Timer _timer;
    [Export]
    private Label _pontosLabel;
    [Export]
    private PackedScene _obstaculo;
    [Export]
    private PackedScene _salvarPontos;

    [Signal]
    public delegate void GameoverEventHandler();
    [Signal]
    public delegate void IniciarEventHandler();
    
    private bool _jogando = false;
    private float _pontos = 0.0f;
    
    public static string NumeroLocalizado(float numero)
    {
        return numero.ToString("N2", new CultureInfo(TranslationServer.GetLocale()));
    }
    public override void _Ready()
    {
        base._Ready();
        this.Comecar();
    }
    public void Comecar()
    {
        if (!_jogando)
        {
            GD.Randomize();
            _jogando = true;
            _pontos = 0;
            _timer.Start();
            _pontosLabel.Text = $"{Tr("INTERFACE_PONTOS")} {NumeroLocalizado(_pontos)}";
            EmitSignal(SignalName.Iniciar);
            GetParent().GetNodeOrNull<SalvarPontuacao>("./SalvarPontuacao")?.QueueFree();
            var obstaculos = GetTree().GetNodesInGroup("obstaculos");
            foreach (var obstaculo in obstaculos)
                obstaculo.QueueFree();
        }
    }
    private void GameOver()
    {
        if (_jogando)
        {
            _jogando = false;
            _timer.Stop();
            EmitSignal(SignalName.Gameover);
            var salvarPontos = _salvarPontos.Instantiate<SalvarPontuacao>();
            salvarPontos.Pontuacao = _pontos;
            salvarPontos.jogarNovamente += Comecar;
            GetParent().AddChild(salvarPontos);
        }
    }
    public void OnCollision(Node2D body)
    {
        switch (body)
        {
            case Jogador:
                GameOver();
                break;
            default:
                break;
        }
    }
    public void OnObstaculoRemovido(string causa)
    {
        switch (causa)
        {
            case "saiu_da_tela":
                if (_jogando) _pontos += 1;
                break;
            default:
                break;
        }
        _pontosLabel.Text = $"{Tr("INTERFACE_PONTOS")} {NumeroLocalizado(_pontos)}";
    }
    public void OnTimerTimeout()
    {
        var obstaculo = _obstaculo.Instantiate<Obstaculo>();
        obstaculo.Removido += OnObstaculoRemovido;
        obstaculo.BodyEntered += OnCollision;
        obstaculo.AddToGroup("obstaculos");
        var novaPosicao = obstaculo.Position;
        novaPosicao.X = GD.RandRange(50, 430);
        novaPosicao.Y = GD.RandRange(-50, -150);
        obstaculo.Position = novaPosicao;
        GetParent().AddChild(obstaculo);
    }
}
