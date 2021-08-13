using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Apitest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public static List<Cliente>ListaCliente { get; set; }

        public ClienteController()
        {
            if(ListaCliente==null)
            {
                ListaCliente = new List<Cliente>();

                Cliente cliente1 = new Cliente();
                cliente1.ClienteId = 69;
                cliente1.Nome = "Adrian";
                cliente1.cpf = "15151515";
                cliente1.telefone = "363636";

                ListaCliente.Add(cliente1);

                Cliente cliente2 = new Cliente();
                cliente2.ClienteId = 70;
                cliente2.Nome = "Pedro";
                cliente2.cpf = "505050";
                cliente2.telefone = "030102";

                ListaCliente.Add(cliente2);
            }
            
        }
        // GET: api/<ClienteController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<Cliente> Get()
        {
            return ListaCliente;
        }

        // GET api/<ClienteController>/5
        [HttpGet("{id}")]
        public Cliente Get(int id)
        {
            return ListaCliente.FirstOrDefault(c=>c.ClienteId==id);
      
        }
        [HttpGet("cpf/{cpf}")]
        public Cliente GetByCpf(string cpf)
        {

            
            return ListaCliente.FirstOrDefault(c => c.cpf == cpf);

           
        }
        [HttpGet("nome/{nome}/telefone/{telefone}")]
        public IEnumerable<Cliente> GetByNomeAndTelefone(string nome,string telefone)
        {

            
            return ListaCliente.Where(c => c.Nome.ToUpper().Contains(nome.ToUpper())||c.telefone==telefone);


        }

        // POST api/<ClienteController>
        [HttpPost]
        public string Post([FromBody] Cliente value)
        {
            value.Nome = value.Nome.ToUpper();
            var cliente = Get(value.ClienteId);
            if (cliente != null)
            {
                
                return "Cliente já cadastrado ";
            }
            else
            {
                ListaCliente.Add(value);

                return "Cadastrado:";
            }
        
            
        }

        // PUT api/<ClienteController>/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] string nome)
        {
            var cliente = Get(id);
            if (cliente!=null) 
            {
                cliente.Nome = nome;
                return "nome alterado com sucesso: ";
            }
            else 
            {
                return "Cliente não encontrado";
            }
        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            var cliente = Get(id);
            if (cliente != null)
            {
                ListaCliente.Remove(cliente);
                return "Cliente deletedado com Sucesso: ";
            }
            else
            {
                return "Cliente não encontrado";
            }

        }
    }
}
