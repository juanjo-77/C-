# Clinica veteninaria

## Creamos un proyecto MVC 
dotnet new mvc -n ClinicaVeterinaria

## Instalamos el paquete de MySql 
dotnet add package MySql.Data

## Instalamos Entity Framework
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

#### en caso de necesitar una version especifica
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0


## =======================================================================



# Clínica Veterinaria — Programación Asíncrona en C#

Sistema de consola que demuestra programación asíncrona y convenciones de codificación en C#, simulando operaciones de una clínica veterinaria con persistencia en MySQL.

## Tecnologías

- **Lenguaje:** C# — .NET 8
- **Base de datos:** MySQL con Entity Framework Core (Pomelo)
- **Arquitectura:** MVC adaptado a consola (Models, Services, Controllers)

## Estructura del proyecto

```
ClinicaVeterinaria/
├── Controllers/
│   └── PacienteController.cs   # Coordina operaciones y demostraciones
├── Data/
│   └── AppDbContext.cs          # Contexto de Entity Framework Core
├── Models/
│   └── Paciente.cs              # Entidad con validaciones
├── Services/
│   └── ClinicaService.cs        # Lógica asíncrona y acceso a MySQL
└── Program.cs                   # Punto de entrada
```

## Instalación

```bash
# Clonar el repositorio
git clone https://github.com/TU_USUARIO/ClinicaVeterinaria.git
cd ClinicaVeterinaria

# Instalar dependencias
dotnet restore

# Configurar la cadena de conexión en Data/AppDbContext.cs
# "Server=localhost;Port=3306;Database=clinica_veterinaria;User=root;Password=TU_PASSWORD;"

# Aplicar migraciones
dotnet ef migrations add InitialCreate
dotnet ef database update

# Correr el proyecto
dotnet run
```

## Tareas implementadas

| Task | Descripción |
|---|---|
| Task 1 | Fundamentos de programación asíncrona documentados con comentarios |
| Task 2 | `RegistrarPacienteAsync()` con `async/await` y persistencia en MySQL |
| Task 3 | Tareas paralelas con `Task.Run()` y `Task.WhenAll` |
| Task 4 | Comparación práctica de `Task.WhenAll` vs `Task.WhenAny` |
| Task 5 | Buenas prácticas — `await` en vez de `.Result` o `.Wait()` |
| Task 6 | Convenciones de codificación — PascalCase, camelCase, nombres descriptivos |

## Conceptos demostrados

**Síncrono vs Asíncrono**
```csharp
// Síncrono — bloquea el hilo
void RegistrarPaciente() { Thread.Sleep(2000); }

// Asíncrono — libera el hilo mientras espera
async Task RegistrarPacienteAsync() { await Task.Delay(2000); }
```

**Task.WhenAll vs Task.WhenAny**
```csharp
// WhenAll — espera que TODOS terminen
await Task.WhenAll(tarea1, tarea2, tarea3);

// WhenAny — espera que CUALQUIERA termine primero
await Task.WhenAny(tarea1, tarea2, tarea3);
```

**Buenas prácticas**
```csharp
// ✓ CORRECTO
string diagnostico = await ObtenerDiagnosticoAsync(paciente);

// ✗ INCORRECTO — bloquea el hilo principal
string diagnostico = ObtenerDiagnosticoAsync(paciente).Result;
```

## Convenciones de codificación aplicadas

- `PascalCase` para clases y métodos: `RegistrarPacienteAsync`, `PacienteController`
- `camelCase` para variables y parámetros: `nombreMascota`, `tiempoConsulta`
- Nombres descriptivos que terminan en `Async` para métodos asíncronos
- Comentarios en bloques complejos explicando el propósito
- Sangría y espaciado consistente en todo el proyecto

---

Corre `dotnet run` 

==============================================================================

# Ayudas  

## Opción 3 — WhenAll

simula que la pagina esta haciendo varias cosas al mismo tiempo, en vez de hacer una 
por una que se demoraria 6 segundos, hace todo de una se demora 3 segundos

## Opción 4 — WhenAll vs WhenAny

Primero registra 3 mascotas al mismo tiempo con WhenAll, despues busca el veterinario 
disponible con WhenAny -  3 doctores "responden" en tiempos distintos y el primero que responde atiende al paciente.

## Opción 5 — Buenas prácticas

Muestra la diferencia entre hacer await ObtenerDiagnosticoAsync() (correcto) vs .Result (incorrecto que bloquea el hilo).


## A tener en cuenta
 PascalCase en clase y métodos, camelCase en variables