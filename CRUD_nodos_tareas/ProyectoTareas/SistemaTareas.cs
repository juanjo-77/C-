public class SistemaTareas
{
    private Nodo cabeza;

    // Constructor
    public SistemaTareas()
    {
        cabeza = null;
    }

    // Agregar tarea
    public void AgregarTarea(string titulo, Prioridad prioridad)
    {
        Tarea nuevaTarea = new Tarea(titulo, prioridad);
        Nodo nuevoNodo = new Nodo(nuevaTarea);

        if (cabeza == null)
        {
            cabeza = nuevoNodo;
        }
        else
        {
            Nodo actual = cabeza;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevoNodo;
        }
    }

    // Eliminar tarea por título
    public void EliminarTarea(string titulo)
    {
        if (cabeza == null) return;

        if (cabeza.Tarea.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
        {
            cabeza = cabeza.Siguiente;
            return;
        }

        Nodo actual = cabeza;
        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.Tarea.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                return;
            }
            actual = actual.Siguiente;
        }
    }

    // Mostrar todas las tareas
    public void MostrarTareas()
    {
        Nodo actual = cabeza;

        if (actual == null)
        {
            Console.WriteLine("No hay tareas.");
            return;
        }

        while (actual != null)
        {
            Console.WriteLine(actual.Tarea);
            actual = actual.Siguiente;
        }
    }

    // Completar tarea (y eliminarla)
    public void CompletarTarea(string titulo)
    {
        if (cabeza == null) return;

        if (cabeza.Tarea.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
        {
            cabeza.Tarea.Completada = true;
            cabeza = cabeza.Siguiente;
            return;
        }

        Nodo actual = cabeza;
        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.Tarea.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
            {
                actual.Siguiente.Tarea.Completada = true;
                actual.Siguiente = actual.Siguiente.Siguiente;
                return;
            }
            actual = actual.Siguiente;
        }
    }

    // Buscar tarea por título
    public Tarea BuscarTarea(string titulo)
    {
        Nodo actual = cabeza;

        while (actual != null)
        {
            if (actual.Tarea.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase))
            {
                return actual.Tarea;
            }
            actual = actual.Siguiente;
        }

        return null;
    }
}