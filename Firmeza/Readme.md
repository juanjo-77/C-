## ¿Qué pidieron?

Construir un sistema completo llamado **Firmeza** para una empresa de materiales de construcción, que funcione todo junto con un solo comando de Docker.

---

## ¿Qué construi?

**4 piezas que trabajan juntas:**

**1. Firmeza.Web (Admin)** → Panel administrativo en web donde puedes:
- Crear, editar y eliminar productos, clientes y ventas
- Generar recibos PDF automáticamente al vender
- Importar datos masivos desde Excel
- Exportar listas a Excel y PDF

**2. Firmeza.API** → Los mismos datos pero expuestos como servicios web (endpoints REST) para que otras apps los consuman

**3. Firmeza.Client** → Una app en React que consume la API y muestra productos y clientes en una interfaz moderna

**4. Firmeza.Tests** → 6 pruebas automáticas que verifican que la lógica funciona correctamente antes de arrancar todo

---

## ¿Cuál es el resultado final?

Con un solo comando:
```bash
sudo docker compose up --build
```

Pasan estas cosas en orden:
1. Se ejecutan las 6 pruebas — si alguna falla, todo se detiene
2. Arranca la base de datos PostgreSQL
3. Arranca el panel admin en `http://localhost:5000`
4. Arranca la API en `http://localhost:5001/swagger`
5. Arranca el cliente React en `http://localhost:3000`

**Sin abrir ningún IDE, sin instalar nada manualmente.**