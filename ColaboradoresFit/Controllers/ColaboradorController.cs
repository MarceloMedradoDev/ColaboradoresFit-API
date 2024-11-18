using ColaboradoresFit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ColaboradoresFit.Service.ColaboradorService;
using MimeKit;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MailKit.Security;

namespace ColaboradoresFit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorInterface _colaboradorInterface;
        public ColaboradorController(IColaboradorInterface colaboradorInterface)
        {
            _colaboradorInterface = colaboradorInterface;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ColaboradorModel>>>> GetColaborador()
        {
            return Ok(await _colaboradorInterface.GetColaborador());
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ColaboradorModel>>>> CreateColaborador(ColaboradorModel novoColaborador)
        {
            return Ok(await _colaboradorInterface.CreateColaborador(novoColaborador));
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<List<ColaboradorModel>>>> DeleteColaborador(int id)
        {
            ServiceResponse<List<ColaboradorModel>> serviceResponse = await _colaboradorInterface.DeleteColaborador(id);

            return Ok(serviceResponse);

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<ColaboradorModel>>> GetColaboradorById(int id)
        {
            ServiceResponse<ColaboradorModel> serviceResponse = await _colaboradorInterface.GetColaboradorById(id);

            return Ok(serviceResponse);
        }

        [HttpPut("inativaColaborador")]
        public async Task<ActionResult<ServiceResponse<List<ColaboradorModel>>>> InativaColaborador(int id)
        {
            ServiceResponse<List<ColaboradorModel>> serviceResponse = await _colaboradorInterface.InativaColaborador(id);

            return Ok(serviceResponse);

        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<ColaboradorModel>>>> UpdateSenha([FromBody] SenhaModel senhaModel)
        {
            ServiceResponse<List<ColaboradorModel>> serviceResponse = await _colaboradorInterface.UpdateSenha(senhaModel);

            return Ok(serviceResponse);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<List<LoginModel>>>> GetLogins([FromBody] LoginModel loginModel)
        {
            return Ok(await _colaboradorInterface.GetLogin(loginModel));
        }


    }
}
