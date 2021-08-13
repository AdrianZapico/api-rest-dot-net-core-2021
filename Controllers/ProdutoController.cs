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
    public class ProdutoController : ControllerBase
    {

        public static List<Produto> ListaProduto { get; set; }

        public ProdutoController()
        {
            if (ListaProduto == null)
            {
                ListaProduto = new List<Produto>();

                Produto produto1 = new Produto();
                produto1.ProdutoId = 1;
                produto1.NomeProduto = "Caneca de café";
                produto1.Descricao = "Copo Azul,de VIDRO";
                produto1.Preco = 15.2;

                string CurrentDate = "01/01/2019";
                DateTime DataProduto = Convert.ToDateTime(CurrentDate);
                produto1.DataProduto = DataProduto;


                Produto produto2 = new Produto();
                produto2.ProdutoId = 2;
                produto2.NomeProduto = "Contra-Baixo Yamaha";
                produto2.Descricao = "Contra-baixo preto,mogmo,4 cordas";
                produto2.Preco = 875.2;

                produto2.DataProduto = Convert.ToDateTime("1/12/2021");

                Post(produto1);
                Post(produto2);

            }

        }
        // GET: api/<ProdutoController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<Produto> Get() => ListaProduto;

        // GET api/<ProdutoController>/5
        [HttpGet("{id}")]
        public Produto Get(int id)
        {
            return ListaProduto.FirstOrDefault(c => c.ProdutoId == id);
        }

        [HttpGet("nomeproduto/{nomeproduto}/descricao/{descricao}")]
        public IEnumerable<Produto> GetByNomeProdutoAndDescricao(string nomeproduto, string descricao)
        {

            return ListaProduto.Where(p => p.NomeProduto.ToUpper().Contains(nomeproduto.ToUpper()) || p.Descricao == descricao);

        }

        [HttpGet("precomin/{precomin}/precomax/{precomax}")]
        public IEnumerable<Produto> trazerProdutos(double precomin, double precomax)
        {
            return ListaProduto.Where(produto => produto.Preco > precomin && produto.Preco < precomax);
        }


        // POST api/<ProdutoController>
        [HttpPost]
        public string Post([FromBody] Produto value)
        {
            value.NomeProduto = value.NomeProduto.ToUpper();
            var produto = Get(value.ProdutoId);
            if (produto != null)
            {

                return $@"Produto {produto.NomeProduto} já 
                    cadastrado";
            }
            else
            {
                ListaProduto.Add(value);

                return $"Produto {value.NomeProduto } Cadastrado";
            }

        }
        [HttpPost("filtrar")]
        public IEnumerable<Produto> Filtra([FromBody] ProdutoFiltro filtro)
        {
            List<Produto> resultado = ListaProduto;
            if (filtro.DataDoProdutoInical != null)
            {
                resultado = resultado.Where(produto => produto.DataProduto >= filtro.DataDoProdutoInical).ToList();
            }
            if (filtro.DataDoProdutoFinal != null)
            {
                resultado = resultado.Where(produto => produto.DataProduto <= filtro.DataDoProdutoFinal).ToList();
            }
            if (filtro.PrecoInical != null)
            {
                resultado = resultado.Where(produto => produto.Preco >= filtro.PrecoInical).ToList();
            }
            if (filtro.PrecoFinal != null)
            {
                resultado = resultado.Where(produto => produto.Preco <= filtro.PrecoFinal).ToList();
            }
            return resultado;
        }

        // PUT api/<ProdutoController>/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] Produto produto)
        {
            var prod = Get(id);
            if (prod != null)
            {
                prod.NomeProduto = produto.NomeProduto;
                prod.Descricao = produto.Descricao;
                prod.Preco = produto.Preco;
                return "Produto alterado com sucesso: ";
            }
            else
            {
                return "Produto não encontrado";
            }
        }

        // DELETE api/<ProdutoController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            var produto = Get(id);
            if (produto != null)
            {
                ListaProduto.Remove(produto);
                return $"Produto {produto.ProdutoId} deletado com Sucesso: ";
            }
            else
            {
                return $"Produto {id} não encontrado";
            }
        }
        [HttpDelete("deletemult")]
        public string DeleteMult([FromBody] int[] id)
        {
            if (id.Length == 0)
            {
                return "Informe pelo menos um id.";
            }
            int[] idnotfound = ObterIdsQueNaoEstaoNaListaDeProdutos(id);
            if (idnotfound.Length > 0)
            {
                return $"Produto {String.Join(", ", idnotfound.ToArray())} não encontrado";
            }
            for (int i = 0; i < id.Length; i++)
            {

                string mensagem = Delete(id[i]);


            }
            return "Deletados com sucesso!";
        }

        private int[] ObterIdsQueNaoEstaoNaListaDeProdutos(int[] id)
        {
            List<int> IdsNaoEncontrados = new List<int>();

            for (int i = 0; i < id.Length; i++)
            {
                bool encontrado = false;
                for (int j = 0; j < ListaProduto.Count; j++)
                {
                    int prod1 = id[i];
                    int prod2 = ListaProduto[j].ProdutoId;

                    if (prod1 == prod2)
                    {
                        encontrado = true;
                    }
                }
                if (encontrado == false)
                {
                    IdsNaoEncontrados.Add(id[i]);
                }

            }
            return IdsNaoEncontrados.ToArray();
        }
    }
}
