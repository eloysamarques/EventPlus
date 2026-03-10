using EventPlus.WebAPI.Models;          
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.BdContextEvent;

namespace EventPlus.WebAPI.Repositories;

public class TipoEventoRepository :
    ITipoEventoRepository
{
    private readonly EventContext _context;

    //Inejção de dependência: Recebe  o contexto pelo construtor
    public TipoEventoRepository(EventContext context)
    {
        _context = context;
    }



    /// <summary>
    /// Atualiza um tipo de evento usando o rastreamento automatico
    /// </summary>
    /// <param name="id"> id fo tipo evento a ser atualizado</param>
    /// <param name="tipoEvento">novos dados do tipo eventno</param>
    public void Atualizar(Guid id, TipoEvento tipoEvento)
    {
        var tipoEventoBuscado = _context.TipoEventos.Find(id); //Busca o tipo de evento pelo id

        if(tipoEventoBuscado != null)
        {
            //Atualiza os campos do tipo de evento buscado com os novos dados
            tipoEventoBuscado.Titulo = tipoEvento.Titulo;

            //Detecta a mudança na propriedade "Titulo" automaticamente
            _context.SaveChanges();
        }
    }



    /// <summary>
    /// Busca um tipo de evento por id
    /// </summary>
    /// <param name="id">id do tipo evento a ser buscados</param>
    /// <returns>Obejto do tipoEvento com as informcoes do tipo de evento buscado</returns>
    public TipoEvento BuscarPorId(Guid id)
    {
        return _context.TipoEventos.Find(id)!; //Busca o tipo de evento pelo id e retorna
    }



    /// <summary>
    /// Cadastra um novo tipo de evento
    /// </summary>
    /// <param name="tipoEvento">tipo de vento a ser cadastrado</param>
    public void Cadastrar(TipoEvento tipoEvento)
    {
        _context.TipoEventos.Add(tipoEvento); //Adiciona o novo tipo de evento ao contexto
        _context.SaveChanges(); //Salva as mudanças no banco de dados
    }



    /// <summary>
    /// Deleta um tipo de evento
    /// </summary>
    /// <param name="id">id do tipo evento a ser deletado</param>
    public void Deletar(Guid id)
    {
        var tipoEventoBuscado = _context.TipoEventos.Find(id); //Busca o tipo de evento pelo id

        if(tipoEventoBuscado != null)
        {
            _context.TipoEventos.Remove(tipoEventoBuscado); //Remove o tipo de evento do contexto
            _context.SaveChanges(); //Salva as mudanças no banco de dados
        }
    }



    /// <summary>
    /// Busca a lista de tipo de eventos cadastrados
    /// </summary>
    /// <returns>Uma lista de tipo eventos</returns>
    public List<TipoEvento> Listar()
    {
        return _context.TipoEventos
            .OrderBy(tipoEvento => tipoEvento.Titulo) //Ordena os tipos de eventos por título
            .ToList(); //Retorna a lista de tipos de eventos ordenada por título
    }
}
