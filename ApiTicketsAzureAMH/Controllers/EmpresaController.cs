using ApiTicketsAzureAMH.Models;
using ApiTicketsAzureAMH.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTicketsAzureAMH.Controllers
{
  
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private RepositoryEmpresa repo;

        public EmpresaController(RepositoryEmpresa repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [Route("[action]/{idusuario}")]
        public ActionResult<List<Ticket>> TicketsUsuario(int idusuario)
        {
            List<Ticket> tickets = this.repo.GetTicketsUsuario(idusuario);
            return tickets;
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public ActionResult <Ticket> FindTicket(int id)
        {
            Ticket ticket = this.repo.GetTicket(id);
            return ticket;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateTicket(Ticket ticket)
        {
            await this.repo.CrearTicket(ticket);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult CreateUsuario(Usuario user)
        {
            this.repo.CrearUsuario(user);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> ProcessTicket(Ticket ticket)
        {
            await this.repo.ProcesarTicket(ticket);
            return Ok();
        }
    }
}
