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

======================================================================

Se usa una base de datos SQL lite para mayor facilidad, aunque se  
podria cambiar por cualquier otra conexion.

======================================================================

en program se utilizan varios metodos los cuales llamamos en el menu, 
para mostrarlo por consola y realizar el CRUD por medio de informacion 
que nos entregue el cliente