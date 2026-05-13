# Firmeza — Sistema de Gestión de Materiales de Construcción

Sistema web administrativo desarrollado con ASP.NET Core 9 Razor Pages y PostgreSQL, orientado a la gestión de productos, clientes y ventas de una empresa de materiales de construcción.

---

## Tecnologías utilizadas

| Capa | Tecnología |
|------|-----------|
| Backend | ASP.NET Core 9 Razor Pages |
| Base de datos | PostgreSQL 16 |
| ORM | Entity Framework Core 9 |
| Autenticación | ASP.NET Identity |
| PDF | QuestPDF 2024 |
| Excel | EPPlus 7 |
| Pruebas | xUnit + EF InMemory |
| Contenedor | Docker + Docker Compose |

---

## Funcionalidades

- Login seguro con roles (Administrador)
- Dashboard con métricas en tiempo real
- CRUD completo de Productos, Clientes y Ventas
- Generación automática de recibo PDF al registrar una venta
- Exportación de datos a Excel y PDF
- Importación masiva desde archivos Excel desnormalizados
- Validación y log de errores en importación
- Pruebas unitarias con xUnit
- Despliegue con Docker y Docker Compose

---

## Requisitos previos (ejecución local)

- .NET 9 SDK
- PostgreSQL 16
- dotnet-ef tool

```bash
dotnet tool install --global dotnet-ef --version 9.0.0
```

---

## Instalación y ejecución local

```bash
# 1. Clonar o descomprimir el proyecto
cd ~/Desktop/Firmeza/Firmeza.Web

# 2. Configurar la cadena de conexión en appsettings.json
# "DefaultConnection": "Host=localhost;Database=firmeza_db;Username=postgres;Password=TU_PASSWORD"

# 3. Crear la base de datos
dotnet ef database update

# 4. Ejecutar
dotnet run
```

Accede en: `http://localhost:5000`  
Credenciales por defecto:
- **Email:** admin@firmeza.com  
- **Contraseña:** Admin123*

---

## Ejecución con Docker

```bash
# Desde la raíz del proyecto
cd ~/Desktop/Firmeza

# Construir y levantar servicios
docker-compose up --build

# La app estará disponible en:
# http://localhost:8080
```

---

## Estructura del proyecto
Firmeza/
├── Firmeza.Web/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Models/
│   │   ├── ApplicationUser.cs
│   │   ├── Cliente.cs
│   │   ├── Producto.cs
│   │   ├── Venta.cs
│   │   └── DetalleVenta.cs
│   ├── Pages/
│   │   ├── Account/         # Login, Logout, AccesoDenegado
│   │   ├── Clientes/        # CRUD + Exportar Excel/PDF
│   │   ├── Productos/       # CRUD + Exportar Excel/PDF
│   │   ├── Ventas/          # CRUD + Exportar Excel/PDF + Recibos
│   │   ├── Importar/        # Carga masiva desde Excel
│   │   └── Dashboard.cshtml
│   ├── wwwroot/
│   │   └── recibos/         # PDFs generados automáticamente
│   ├── Program.cs
│   └── Dockerfile
├── Firmeza.Tests/
│   └── ProductoServiceTests.cs
├── docker-compose.yml
└── README.md

---

## Modelo Entidad-Relación
CLIENTE ||--o{ VENTA : "realiza"
VENTA ||--|{ DETALLE_VENTA : "contiene"
PRODUCTO ||--o{ DETALLE_VENTA : "incluido en"

### Entidades principales

**Producto**
- Id, Nombre, Descripcion, Categoria, Precio, Stock

**Cliente**
- Id, Nombre, Documento, Correo, Telefono, Direccion

**Venta**
- Id, Fecha, ClienteId (FK), Total, IVA

**DetalleVenta**
- Id, VentaId (FK), ProductoId (FK), Cantidad, PrecioUnitario

---

## Diagrama de clases simplificado
ApplicationUser (IdentityUser)

NombreCompleto

Producto

Id, Nombre, Descripcion
Categoria, Precio, Stock

Cliente

Id, Nombre, Documento
Correo, Telefono, Direccion

Venta

Id, Fecha, ClienteId
Total, IVA
Detalles: List<DetalleVenta>

DetalleVenta

Id, VentaId, ProductoId
Cantidad, PrecioUnitario
Subtotal (calculado)


---

## Credenciales por defecto

| Campo | Valor |
|-------|-------|
| Email | admin@firmeza.com |
| Contraseña | Admin123* |
| Rol | Administrador |

---

## Recibos PDF

Al registrar una venta, el sistema genera automáticamente un PDF en:
wwwroot/recibos/recibo_{id}.pdf

Descargable directamente desde la vista de Ventas.

---

## Importación Excel

El sistema acepta archivos `.xlsx` con columnas mezcladas. Detecta automáticamente si los datos corresponden a Productos o Clientes según las columnas presentes, normaliza e inserta o actualiza los registros.

**Columnas para Productos:** nombre, descripcion, categoria, precio, stock  
**Columnas para Clientes:** nombre, documento, correo, telefono, direccion