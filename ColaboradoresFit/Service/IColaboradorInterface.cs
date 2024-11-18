using ColaboradoresFit.Models;
using Microsoft.AspNetCore.Mvc;

namespace ColaboradoresFit.Service.ColaboradorService
{
    public interface IColaboradorInterface
    {
        Task<ServiceResponse<List<ColaboradorModel>>> GetColaborador();
        Task<ServiceResponse<List<LoginModel>>> GetLogin([FromBody] LoginModel loginModel);
        Task<ServiceResponse<List<ColaboradorModel>>> CreateColaborador(ColaboradorModel novoColaborador);
        Task<ServiceResponse<ColaboradorModel>> GetColaboradorById(int id);
        Task<ServiceResponse<List<ColaboradorModel>>> UpdateSenha([FromBody] SenhaModel senhaModel);
        Task<ServiceResponse<List<ColaboradorModel>>> DeleteColaborador(int id);
        Task<ServiceResponse<List<ColaboradorModel>>> InativaColaborador(int id);
    }
}
