using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TipoEventoControllers : ControllerBase
{
    private ITipoEventoRepository _tipoEventoRepository;

    public TipoEventoControllers(ITipoEventoRepository tipoEventoRepository)
    {
        _tipoEventoRepository = tipoEventoRepository;
    }



    /// <summary>
    /// EndPoint da API que faz chamada para o metodo de listar os tipos de evento
    /// </summary>
    /// <returns>Status code 200 e a lista de tipos de eventos</returns>
    [HttpGet]
    public IActionResult Listar()
    {
        try
        {
            return Ok(_tipoEventoRepository.Listar());
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }



    /// <summary>
    /// EndPoint da API que faz chamada para o metodo de buscar um tipo de evento específico por id
    /// </summary>
    /// <param name="id">Id do tipo de evento buscado</param>
    /// <returns>Status code 200 e tipo de evento buscado</returns>
    [HttpGet("{id}")]
    public IActionResult BuscarPorId(Guid id)
    {
        try
        {
            return Ok(_tipoEventoRepository.BuscarPorId(id));
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }



    /// <summary>
    /// EndPoint da API que faz a chamada para o metodo de cadastrar um tipo de evento
    /// </summary>
    /// <param name="tipoEvento">Tipo de eventos a ser cadastrado</param>
    /// <returns>Satus code 201 e o tipo de evento cadastrado</returns>
    [HttpPost]
    public IActionResult Cadastrar(TipoEventoDTO tipoEvento)
    {
        try
        {
            var novoTipoEvento = new TipoEvento
            {
                Titulo = tipoEvento.Titulo!
            };

            _tipoEventoRepository.Cadastrar(novoTipoEvento);

            return StatusCode(201, novoTipoEvento);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }




    /// <summary>
    /// EndPoint da API que faz a chamada para o metodo de atualizar um tipo de evento
    /// </summary>
    /// <param name="id">Id do tipo de evento a ser atualizados</param>
    /// <param name="tipoEvento">atipo de evento com os dados atualizados</param>
    /// <returns> Status code 204 e o tipo de evento atuazalizado</returns>
    [HttpPut("{id}")]
    public IActionResult Atualizar(Guid id, TipoEvento tipoEvento)
    {
        try
        {
            _tipoEventoRepository.Atualizar(id, tipoEvento);
            return StatusCode(204, tipoEvento);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }




    /// <summary>
    /// EndPoint da API que faz a chamada para o metodo de deletar um tipo de evento
    /// </summary>
    /// <param name="id">Id do tipo de evento a ser excluido</param>
    /// <returns>Status code 204<returns>
    [HttpDelete("{id}")]
    public IActionResult Deletar(Guid id)
    {
        try
        {
            _tipoEventoRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }
}
