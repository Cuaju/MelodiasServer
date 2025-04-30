# 🎶 Melodías del Mundo - Backend

Sistema de gestión para una tienda de música, desarrollado como aplicación cliente-servidor utilizando **WCF**, **Entity Framework (Code First)** y **SQL Server**. Este proyecto forma parte del sistema completo que interactúa con una interfaz gráfica WPF en el cliente.

---

## 🚀 Tecnologías Utilizadas

- **C# (.NET Framework 4.8)**
- **WCF (Windows Communication Foundation)** - para servicios distribuidos
- **Entity Framework 6.5.1** - enfoque Code First
- **SQL Server 2017**
- **Visual Studio 2022**

---

## 📁 Estructura del Proyecto

```
MelodiasServer/
├── DataAccess/           → Modelos, DAOs y contexto EF
├── MelodiasService/      → Interfaces WCF y clases de contrato (DataContracts)
├── Host/                 → Aplicación de consola que hospeda el servicio WCF
├── packages/             → Dependencias NuGet
├── .gitignore
├── MelodiasServer.sln    → Solución principal
```

---

## 🧩 Servicios Disponibles

### `IUsersManager`
- Autenticación de usuarios
- Registro y eliminación de empleados
- Verificación de datos únicos (correo, número, usuario)

### `IProductsManager`
- CRUD de productos
- Validaciones por nombre

### `ISuppliersManager`
- CRUD de proveedores
- Validaciones por nombre y correo

### `ISalesManager`
- Registro, edición y cancelación de ventas
- Consulta por ID
- Detalles de venta incluidos

---

## 🛠 Configuración del Servidor

### 1. Asegúrate de tener SQL Server 2017 o superior
> Debes tener una instancia llamada `.\MSSQLALTA` o cambiar el `connectionString` en `App.config`.

### 2. Ejecuta el proyecto `Host`
Este se encargará de abrir el servicio WCF localmente:

```
net.tcp://localhost:8001/ServiceImplementation/
```

---

## 👥 Contribuyentes

Agradecimientos a quienes han colaborado con este proyecto:

- [@Cuaju](https://github.com/Cuaju)
- [@marco1gk](https://github.com/marco1gk)
- [@carimvch33](https://github.com/carimvch33)
- [@GerlyUwU](https://github.com/GerlyUwU)
 <a href="https://github.com/Cuaju/MelodiasServer/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=Cuaju/MelodiasServer" />
</a>

---



## 📄 Licencia

Este proyecto es de uso académico. Todos los derechos reservados a los autores.
