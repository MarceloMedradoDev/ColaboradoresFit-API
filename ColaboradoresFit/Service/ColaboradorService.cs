using Microsoft.EntityFrameworkCore;
using ColaboradoresFit.DataContext;
using ColaboradoresFit.Models;
using Microsoft.AspNetCore.Mvc;

namespace ColaboradoresFit.Service.ColaboradorService
{
    public class ColaboradorService : IColaboradorInterface
    {
        private readonly ApplicationDbContext _context;
        public ColaboradorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ColaboradorModel>>> GetColaborador()
        {
            ServiceResponse<List<ColaboradorModel>> serviceResponse = new ServiceResponse<List<ColaboradorModel>>();

            try
            {

                serviceResponse.Dados = _context.Colaboradores.ToList();

                if (serviceResponse.Dados.Count == 0)
                {
                    serviceResponse.Mensagem = "Nenhum dado encontrado!";
                }


            }
            catch (Exception ex)
            {

                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;

        }

        public async Task<ServiceResponse<List<ColaboradorModel>>> CreateColaborador(ColaboradorModel novoColaborador)
        {
            ServiceResponse<List<ColaboradorModel>> serviceResponse = new ServiceResponse<List<ColaboradorModel>>();

            try
            {
                if (novoColaborador == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Informar dados!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }

                novoColaborador.DataDeCadastro = DateTime.Now.ToLocalTime();

                _context.Add(novoColaborador);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Colaboradores.ToList();


            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ColaboradorModel>>> DeleteColaborador(int id)

        {
            ServiceResponse<List<ColaboradorModel>> serviceResponse = new ServiceResponse<List<ColaboradorModel>>();

            try
            {
                ColaboradorModel colaborador = _context.Colaboradores.FirstOrDefault(x => x.Id == id);

                if (colaborador == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }


                _context.Colaboradores.Remove(colaborador);
                await _context.SaveChangesAsync();


                serviceResponse.Dados = _context.Colaboradores.ToList();

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        [HttpPost("login")]
        public async Task<ServiceResponse<List<LoginModel>>> GetLogin([FromBody] LoginModel loginModel)
        {
            ServiceResponse<List<LoginModel>> serviceResponse = new ServiceResponse<List<LoginModel>>();

            try
            {

                var colaboradores = await _context.Colaboradores
                    .Where(c => c.Login.ToLower() == loginModel.Login.ToLower() && c.Senha == loginModel.Senha)
                    .ToListAsync();

                if (colaboradores == null || colaboradores.Count == 0)
                {

                    serviceResponse.Mensagem = "Usuário ou senha inválidos!";
                    serviceResponse.Sucesso = false;
                    serviceResponse.Dados = null;
                }
                else
                {

                    serviceResponse.Dados = colaboradores.Select(c => new LoginModel
                    {
                        Login = c.Login,
                        Senha = null
                    }).ToList();
                    serviceResponse.Mensagem = "Login realizado com sucesso!";
                    serviceResponse.Sucesso = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = "Erro: " + ex.Message;
                serviceResponse.Sucesso = false;
                serviceResponse.Dados = null;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ColaboradorModel>> GetColaboradorById(int id)
        {
            {
                ServiceResponse<ColaboradorModel> serviceResponse = new ServiceResponse<ColaboradorModel>();

                try
                {
                    ColaboradorModel colaborador = _context.Colaboradores.FirstOrDefault(x => x.Id == id);

                    if (colaborador == null)
                    {
                        serviceResponse.Dados = null;
                        serviceResponse.Mensagem = "Usuário não localizado!";
                        serviceResponse.Sucesso = false;
                    }

                    serviceResponse.Dados = colaborador;

                }
                catch (Exception ex)
                {

                    serviceResponse.Mensagem = ex.Message;
                    serviceResponse.Sucesso = false;
                }

                return serviceResponse;
            }

        }



        public async Task<ServiceResponse<List<ColaboradorModel>>> InativaColaborador(int id)
        {
            ServiceResponse<List<ColaboradorModel>> serviceResponse = new ServiceResponse<List<ColaboradorModel>>();

            try
            {
                ColaboradorModel colaborador = _context.Colaboradores.FirstOrDefault(x => x.Id == id);

                if (colaborador == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;
                }

                _context.Colaboradores.Update(colaborador);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Colaboradores.ToList();


            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ColaboradorModel>>> UpdateSenha([FromBody] SenhaModel senhaModel)
        {
            ServiceResponse<List<ColaboradorModel>> serviceResponse = new ServiceResponse<List<ColaboradorModel>>();
            var emailService = new EmailService();

            try
            {
                var colaboradores = await _context.Colaboradores
                    .Where(c => c.Email == senhaModel.Email)
                    .ToListAsync();

                if (colaboradores == null || colaboradores.Count == 0)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;
                }
                else
                {
                    // Gera uma nova senha temporária
                    string novaSenha = GenerateRandomPassword();

                    // Envia o e-mail com a nova senha temporária
                    await emailService.SendPasswordResetEmail(senhaModel.Email, novaSenha);

                    // Atualiza a senha dos colaboradores encontrados com a nova senha temporária
                    foreach (var colaborador in colaboradores)
                    {
                        colaborador.Senha = novaSenha;
                    }

                    await _context.SaveChangesAsync();

                    serviceResponse.Dados = colaboradores;
                    serviceResponse.Mensagem = "Senha redefinida e enviada por e-mail com sucesso!";
                    serviceResponse.Sucesso = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
        private string GenerateRandomPassword(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}