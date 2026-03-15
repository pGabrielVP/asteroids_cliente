using Godot;

public partial class Jogador : CharacterBody2D
{
    [Export]
    public float _velocidade = 300.0f;
    
    private bool _jogando = true;
    
    public override void _PhysicsProcess(double delta)
    {
        if (_jogando)
        {
            Vector2 velocity = Vector2.Zero;
            if (Input.IsActionPressed("mover_cima")) velocity.Y = -1 * _velocidade;
            if (Input.IsActionPressed("mover_baixo")) velocity.Y = 1 * _velocidade;
            if (Input.IsActionPressed("mover_esquerda")) velocity.X = -1 * _velocidade;
            if (Input.IsActionPressed("mover_direita")) velocity.X = 1 * _velocidade;
            if (velocity.X != 0 && velocity.Y != 0) velocity /= 2;
            Velocity = velocity;
            MoveAndSlide();
        }
    }
    public void OnGameOver()
    {
        _jogando = false;
    }
    public void OnIniciar()
    {
        _jogando = true;
    }
}
