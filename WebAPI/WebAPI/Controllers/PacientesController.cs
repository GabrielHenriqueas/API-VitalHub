﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Domains;
using WebAPI.Interfaces;
using WebAPI.Repositories;
using WebAPI.Utils.BlobStorage;
using WebAPI.Utils.Mail;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private IPacienteRepository pacienteRepository { get; set; }

        private readonly EmailSendingService _emailSendingServices;

        public PacientesController(EmailSendingService emailSendingServices)
        {
            pacienteRepository = new PacienteRepository();
            _emailSendingServices = emailSendingServices;
        }

        [HttpGet("PerfilLogado")]
        public IActionResult GetLogged()
        {
            try
            {
                Guid idUsuario = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                return Ok(pacienteRepository.BuscarPorId(idUsuario));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [HttpGet("BuscarPorId")]
        public IActionResult BuscarPorId(Guid id)
        {
            return Ok(pacienteRepository.BuscarPorId(id));
        }

        //[HttpPost]
        //public async Task<IActionResult> Post(PacienteViewModel pacienteModel)
        //{
        //    Usuario user = new Usuario();

        //    user.Nome = pacienteModel.Nome;
        //    user.Email = pacienteModel.Email;
        //    user.TipoUsuarioId = pacienteModel.IdTipoUsuario;
        //    user.Foto = pacienteModel.Foto;
        //    user.Senha = pacienteModel.Senha;

        //    user.Paciente = new Paciente();

        //    user.Paciente.DataNascimento = pacienteModel.DataNascimento;
        //    user.Paciente.Rg = pacienteModel.Rg;
        //    user.Paciente.Cpf = pacienteModel.Cpf;

        //    user.Paciente.Endereco = new Endereco();

        //    user.Paciente.Endereco.Logradouro = pacienteModel.Logradouro;
        //    user.Paciente.Endereco.Numero = pacienteModel.Numero;
        //    user.Paciente.Endereco.Cep = pacienteModel.Cep;
        //    user.Paciente.Endereco.Cidade = pacienteModel.Cidade;

        //    pacienteRepository.Cadastrar(user);

        //    await _emailSendingServices.SendWelcomeEmail(user.Email!, user.Nome!);

        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PacienteViewModel pacienteModel)
        {
            try
            {
                //objeto a ser cadastrado
                Usuario user = new Usuario();

                //recebe os valores e preenche as propriedades do objeto
                user.Nome = pacienteModel.Nome;
                user.Email = pacienteModel.Email;
                user.TipoUsuarioId = pacienteModel.IdTipoUsuario;



                //define o nome do container do blob
                var containerName = "vitalhubmesa02grupo12";

                //define a string de conexão
                var connectionString = "DefaultEndpointsProtocol=https;AccountName=blobvitalhubmesa2grupo12;AccountKey=epWweuVQHWWXDQoqhlNm5XeOWuglGXfbgW80JI3WuLfFVmm5uohUMQZQGxQmO1F2ATCI37KL0PLh+ASt+fQyPg==;EndpointSuffix=core.windows.net";

            //aqui vamos chamar o metodo para upload da imagem
            user.Foto = await AzureBlobStorageHelper.UploadImageBlobAsync(pacienteModel.Arquivo, connectionString, containerName);

                user.Senha = pacienteModel.Senha;

                user.Paciente = new Paciente();

                user.Paciente.DataNascimento = pacienteModel.DataNascimento;
                user.Paciente.Rg = pacienteModel.Rg;
                user.Paciente.Cpf = pacienteModel.Cpf;

                user.Paciente.Endereco = new Endereco();

                user.Paciente.Endereco.Logradouro = pacienteModel.Logradouro;
                user.Paciente.Endereco.Numero = pacienteModel.Numero;
                user.Paciente.Endereco.Cep = pacienteModel.Cep;
                user.Paciente.Endereco.Cidade = pacienteModel.Cidade;

                pacienteRepository.Cadastrar(user);

                await _emailSendingServices.SendWelcomeEmail(user.Email!, user.Nome!);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpGet("BuscarPorData")]
        public IActionResult GetByDate(DateTime data, Guid id)
        {
            try
            {
                return Ok(pacienteRepository.BuscarPorData(data, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateProfile(Guid idUsuario, PacienteViewModel paciente)
        {
            try
            {
                return Ok(pacienteRepository.AtualizarPerfil(idUsuario, paciente));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
