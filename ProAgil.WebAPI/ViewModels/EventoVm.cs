using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.ViewModels
{
    public class EventoVm
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve estar entre {2} e {1} caracateres.")]
        public string Local { get; set; }
        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Tema { get; set; }

        [Range(2, 10000, ErrorMessage = "O campo {0} deve estar entre {1} e {2}.")]
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }

        [Phone]
        public string Telefone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public List<LoteVm> Lotes { get; set; }
        public List<RedeSocialVm> RedesSociais { get; set; }
        public List<PalestranteVm> Palestrantes { get; set; }
    }
}