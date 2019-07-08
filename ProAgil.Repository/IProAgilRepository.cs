using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantres = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantres = false);
        Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrantres = false);

        Task<Palestrante[]> GetAllPalestrantesAsyncByNome(string nome, bool includeEventos = false);
        Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false);
        Task<Palestrante> GetPalestranteAsyncById(int palestranteId, bool includeEventos = false);
    }
}