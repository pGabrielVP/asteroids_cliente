using Godot;
using Godot.NativeInterop;

public partial class SalvarPontuacao : Control
{
    [Export]
    private Label _pontos;
    [Export]
    private Label _response;
    [Export]
    private Button _enviar;
    [Export]
    private Button _jogarNovamente;
    [Export]
    private LineEdit _input;
    [Export]
    private VBoxContainer _tabelaClassificacao;
    
    [Signal]
    public delegate void jogarNovamenteEventHandler();

    private const string API_URL = "https://localhost:7252";
    
    private bool _enviado = false;
    private float _pontuacao = 0.0f;
    public float Pontuacao 
    {
        get => _pontuacao;
        set 
        { 
            _pontuacao = value;
            _pontos.Text = $"{_pontuacao}";
        } 
    }
    private void HttpClassificacaoRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        var json = new Json();
        json.Parse(body.GetStringFromUtf8());
        var response = json.GetData().AsGodotArray<Godot.Collections.Dictionary>();
        if (response.Count > 0) 
        {
            for (int i = 0; i < response.Count; i++)
            {
                response[i].TryGetValue("colocacao", out var colocacao);
                response[i].TryGetValue("jogador", out var jogador);
                response[i].TryGetValue("pontos", out var pontos);
                var pontuacaoLabel = new Label();
                pontuacaoLabel.Text = $"{ pontos }";
                var jogadorLabel = new Label();
                jogadorLabel.Text = $"{ jogador }";
                var classificacaoLabel = new Label();
                classificacaoLabel.Text = $"{ (int)colocacao }";
                var _hContainer = new HBoxContainer();
                _hContainer.Alignment = HBoxContainer.AlignmentMode.Center;
                _hContainer.AddChild( classificacaoLabel );
                _hContainer.AddChild( jogadorLabel );
                _hContainer.AddChild( pontuacaoLabel );
                _tabelaClassificacao.AddChild( _hContainer );
            }
        }
    }
    public override void _Ready()
    {
        var httpRequest = new HttpRequest();
        AddChild(httpRequest);
        httpRequest.RequestCompleted += HttpClassificacaoRequestCompleted;
        Error error = httpRequest.Request($"{API_URL}/api/scoreboard/3", null, HttpClient.Method.Get);
        if (error != Error.Ok)
            GD.PushError("An error occurred in the HTTP request.");
    }
    private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        var json = new Json();
        json.Parse(body.GetStringFromUtf8());
        var response = json.GetData().AsGodotDictionary();
        response.TryGetValue("colocacao",out Godot.Variant colocacao);
        _response.Text = "Sua colocação na tabela é: " + (int)colocacao;
    }
    public void OnEnviarPressed()
    {
        if (!_enviado)
        {
            _enviado = true;
            var httpRequest = new HttpRequest();
            AddChild(httpRequest);
            httpRequest.RequestCompleted += HttpRequestCompleted;
            string body = Json.Stringify(new Godot.Collections.Dictionary
            {
                { "Pontos", _pontuacao },
                { "Jogador", _input.Text }
            });
            Error error = httpRequest.Request($"{ API_URL }/api/scoreboard/", ["Content-Type:application/json"], HttpClient.Method.Post, body);
            if (error != Error.Ok)
                GD.PushError("An error occurred in the HTTP request.");
        }
    }
    public void OnJogarNovamentePressed()
    {
        EmitSignal(SignalName.jogarNovamente);
    }
}
