public class Nodo
{
    public Tarea Tarea { get; set; }
    public Nodo Siguiente { get; set; }

    // Constructor
    public Nodo(Tarea tarea)
    {
        Tarea = tarea;
        Siguiente = null;
    }
}