using SGP.Core.Domain.Common;

namespace SGP.Core.Domain.Entities
{
    public class PruebaLaboratorio : BaseEntity
    {
        public string Nombre { get; set; }

        // Foreign Key
        public int ConsultorioId { get; set; }

        // Navigation Properties
        public Consultorio? Consultorio { get; set; }
        public List<ResultadoLaboratorio> Resultados { get; set; } = new();
    }
}
