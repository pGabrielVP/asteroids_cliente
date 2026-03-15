using Godot;

public partial class Obstaculo : Area2D
{
    [Export]
    private float _velocidade;
    
    [Signal]
    public delegate void RemovidoEventHandler(string causa);

    public override void _Ready()
    {
        base._Ready();
    }
    public override void _PhysicsProcess(double delta)
    {
        var posicao = Position;
        posicao.Y += (float)(_velocidade * delta);
        Position = posicao;
        if (Position.Y > (GetViewportRect().Size.Y + 50))
        {
            EmitSignal(SignalName.Removido, "saiu_da_tela");
            Free();
        }
    }
}
