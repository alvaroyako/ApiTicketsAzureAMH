using ApiTicketsAzureAMH.Data;
using ApiTicketsAzureAMH.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiTicketsAzureAMH.Repositories
{
    public class RepositoryEmpresa
    {
        private ContextEmpresa context;

        public MediaTypeWithQualityHeaderValue Header { get; private set; }

        public RepositoryEmpresa(ContextEmpresa context)
        {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.context = context;
        }

        private int GetMaxIdUsuario()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuarios.Max(z => z.IdUsuario) + 1;
            }
        }

        public List<Ticket> GetTicketsUsuario(int idusuario)
        {
            var consulta = from datos in this.context.Tickets
                           where datos.IdUsuario==idusuario
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return consulta.ToList();
            }
        }

        public Ticket GetTicket(int idticket)
        {
            return this.context.Tickets.SingleOrDefault(x => x.IdTicket == idticket);
        }

        public async Task CrearTicket(Ticket ticket)
        {
            string urlFlowInsert = "https://prod-45.westeurope.logic.azure.com:443/workflows/35d8fc47990145d6ad16390b23df6cc1/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=aWa7n-_922y7tarnI-2vrFyqM24M2M58crWVVIJgRwE";
            using (HttpClient client=new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string json = JsonConvert.SerializeObject(ticket);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                await client.PostAsync(urlFlowInsert, content);
            }
        }

        public void CrearUsuario(Usuario user)
        {
            int idusuario = this.GetMaxIdUsuario();
            Usuario usu = new Usuario();
            usu.IdUsuario = idusuario;
            usu.Nombre = user.Nombre;
            usu.Apellidos = user.Apellidos;
            usu.Email = user.Email;
            usu.UserName = user.UserName;
            usu.Password = user.Password;
            this.context.Usuarios.Add(usu);
            this.context.SaveChanges();

        }

        public async Task ProcesarTicket(Ticket ticket)
        {
            string urlFlowProcess = "https://prod-149.westeurope.logic.azure.com:443/workflows/8bdef5878cf348c887d283bb6310f65c/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=qtuWnbRtVuLfqSgJOrvmz9bP4cJF8YXjc6ASw6i71Uw";
            
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string json = JsonConvert.SerializeObject(ticket);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                await client.PostAsync(urlFlowProcess, content);
            }

        }
    }
}
