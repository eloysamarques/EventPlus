using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

public interface IInstituicaoRepository
{
    List<Instituicao> Listar();
        void Cadastrar(Instituicao tipoInstituicao);
        void Atualizar(Guid id, Instituicao tipoInstituicao);
        void Deletar(Guid id);
        Instituicao BuscarPorId(Guid id);
}