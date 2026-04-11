#### Como se hizo

Despues de instalar dependencias y todos los comandos

se crean los models que son la representacion de la Base de datos 

AstroNovaSystem/
├── Models/         ← aquí van las entidades
├── Data/           ← aquí va el DbContext
├── Services/       ← aquí van los CRUDs
├── Program.cs
└── AstroNovaSystem.csproj

======================================================================

Program.cs          → solo llama métodos, no sabe cómo funcionan
Services/           → aquí vive toda la lógica
Data/AppDbContext   → solo habla con la base de datos