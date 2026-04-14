namespace CrudMVC.Services
{
    public class ClienteService
    {
        private readonly List<Models.Cliente> _clientes = new();
        private int _nextId = 1;

        public ClienteService()
        {
            // Datos de ejemplo
            _clientes.AddRange(new[]
            {
                new Models.Cliente { Id = _nextId++, Nombre = "Ana García",    Email = "ana@email.com",    Telefono = "3001234567", Ciudad = "Medellín" },
                new Models.Cliente { Id = _nextId++, Nombre = "Luis Pérez",    Email = "luis@email.com",   Telefono = "3109876543", Ciudad = "Bogotá"   },
                new Models.Cliente { Id = _nextId++, Nombre = "María López",   Email = "maria@email.com",  Telefono = "3205551234", Ciudad = "Cali"     },
            });
        }

        public List<Models.Cliente> GetAll() => _clientes.ToList();

        public Models.Cliente? GetById(int id) =>
            _clientes.FirstOrDefault(c => c.Id == id);

        public void Create(Models.Cliente cliente)
        {
            cliente.Id = _nextId++;
            _clientes.Add(cliente);
        }

        public bool Update(Models.Cliente cliente)
        {
            var existing = _clientes.FirstOrDefault(c => c.Id == cliente.Id);
            if (existing is null) return false;

            existing.Nombre   = cliente.Nombre;
            existing.Email    = cliente.Email;
            existing.Telefono = cliente.Telefono;
            existing.Ciudad   = cliente.Ciudad;
            return true;
        }

        public bool Delete(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente is null) return false;

            _clientes.Remove(cliente);
            return true;
        }
    }
}