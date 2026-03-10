using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

public interface IComentarioEventoRepository
{
    void Cadastrar (ComentarioEvento comentarioEvento);
    void Deletar(Guid id);
    List<ComentarioEvento> Listar(Guid IdEvento); //ComentarioEvento vem do Models, banco de dados
    ComentarioEvento BuscarPorUsuario(Guid IdUsuario, Guid IdEvento); 
    List<ComentarioEvento> ListarSomenteExibe(Guid IdEvento);
}
