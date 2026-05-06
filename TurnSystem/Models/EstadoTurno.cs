namespace TurnSystem.Web.Models;

public enum EstadoTurno
{
    Pendiente = 0,
    EnEspera = 1,
    EnAtencion = 2,
    Finalizado = 3,
    NoPresentado = 4,
    Cancelado = 5
}