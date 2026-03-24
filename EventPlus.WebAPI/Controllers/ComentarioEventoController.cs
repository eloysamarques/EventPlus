using Azure;
using Azure.AI.ContentSafety;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComentarioEventoController : ControllerBase
{
    private readonly ContentSafetyClient _contentSafetyClient;
    private readonly IComentarioEventoRepository _comentarioEventoRepository;

    public ComentarioEventoController(ContentSafetyClient contentSafetyClient, IComentarioEventoRepository comentarioEventoRepository)
    {
        _contentSafetyClient = contentSafetyClient;
        _comentarioEventoRepository = comentarioEventoRepository;
    }


    /// <summary>
    /// EndPoint da API que cadastra e modera um comentario
    /// </summary>
    /// <param name="comentarioEvento">comentario a ser moderado</param>
    /// <returns>StatusCode 201 e o comentario criado</returns>
    [HttpPost]
    public async Task<IActionResult> Cadastrar(ComentarioEventoDTO comentarioEvento)
    {
        try
        {
            if (string.IsNullOrEmpty(comentarioEvento.Descricao))
            { 
                return BadRequest("O texto a ser moderado não pode ser vazio.");
            }

            //criar objeto de analise
            var request = new AnalyzeTextOptions(comentarioEvento.Descricao);

            //chamar a API do Azure Content Safety
            Response<AnalyzeTextResult> response = await _contentSafetyClient.AnalyzeTextAsync(request);

            //vereficar se o texto tem alguma severidade  maior que 0
            bool temConteudoInapropriado = response.Value.CategoriesAnalysis.Any
                (comentario => comentario.Severity > 0);

            var novoComentario = new ComentarioEvento
            {
                Descricao = comentarioEvento.Descricao,
                IdUsuario = comentarioEvento.IdUsuario,
                IdEvento = comentarioEvento.IdEvento,
                DataComentarioEvento = DateTime.Now,
                //Define se o comentario vai ser exibido
                Exibe = !temConteudoInapropriado
            };

            //Cadastrar comentario
            _comentarioEventoRepository.Cadastrar(novoComentario);

            return StatusCode(201, novoComentario);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    [HttpGet("Evento/{idEvento}")]
    public IActionResult ListarSomenteExibe(Guid idEvento)
    {
        try
        {
            return Ok(_comentarioEventoRepository.ListarSomenteExibe(idEvento));
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    [HttpGet("{idUsuario}/{idEvento}")]
    public IActionResult BuscarPorIdUsuario(Guid idUsuario, Guid idEvento) // Busca um comentário específico de um usuário para um evento específico
    {
        try
        {
            var comentario = _comentarioEventoRepository.BuscarPorIdUsuario(idUsuario, idEvento); // Chama o método do repositório para buscar o comentário com base no ID do usuário e do evento

            if (comentario != null) // Verifica se o comentário foi encontrado
                return NotFound(); // Retorna 404 se o comentário não for encontrado

            return Ok(comentario); // Retorna 200 com o comentário encontrado
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }

    [HttpGet]
    public IActionResult Listar(Guid idEvento)
    {
        try
        {
            return Ok(_comentarioEventoRepository.Listar(idEvento));
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }


    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _comentarioEventoRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }
}
