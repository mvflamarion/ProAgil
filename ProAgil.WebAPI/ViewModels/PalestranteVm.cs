using System.Collections.Generic;

namespace ProAgil.WebAPI.ViewModels
{
    public class PalestranteVm
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string MiniCurriculo { get; set; }
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public List<RedeSocialVm> RedesSociais { get; set; }
        public List<EventoVm> Eventos { get; set; }
    }
}