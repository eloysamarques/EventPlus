using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

public interface IEventoRepository
{
    void Cadstrar(Evento evento);
    List<Evento> Listar();
    void Deletar(Guid id);
    void Atualizar(Guid id, Evento evento);
    List<Evento> ListarPorId(Guid id);
    List<Evento> ProximosEventos();

}
