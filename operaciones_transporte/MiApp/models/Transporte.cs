namespace MiApp.Models
{
    public class Transporte
    {
        public int Id { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public float Distacia { get; set; }
        public string Estado { get; set; }
        public float CostoTotal { get; set; }
    }
}