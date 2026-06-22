# Registro de Gastos MAUI

Aplicación móvil desarrollada en .NET MAUI para Android que permite registrar y gestionar transacciones personales (ingresos y gastos). La aplicación calcula automáticamente el balance disponible, el total de ingresos y egresos, manteniendo un historial persistente de todas las transacciones.

## Requisitos del Proyecto

- Permitir el ingreso de transacciones como gasto
- Permitir el ingreso de transacciones como ingreso
- Mostrar el balance total
- Mostrar el total de ingresos
- Mostrar el total de egresos
- Mostrar el listado de transacciones
- Implementar el patrón MVVM con Community Toolkit MVVM
- Utilizar SQLite para persistencia de datos

## Características Implementadas

La aplicación incluye:

- Pantalla principal con visualización del balance, ingresos y egresos totales
- Resumen dinámico que muestra la cantidad de movimientos registrados
- Formulario para agregar transacciones con validación de entrada
- Soporte para registrar tanto ingresos como gastos con checkbox
- Selector de fecha para cada transacción
- Listado de todas las transacciones ordenadas por fecha
- Cálculo automático de totales
- Almacenamiento persistente en SQLite que se mantiene entre sesiones
- Interfaz responsiva y clara

## Estructura del Proyecto

```
Registro-de-Gastos/
├── Models/
│   └── FinancialTransaction.cs       (Modelo de datos de transacción)
├── Services/
│   └── FinanceDatabaseService.cs     (Servicio de base de datos SQLite)
├── ViewModels/
│   ├── HomeViewModel.cs              (ViewModel de pantalla principal)
│   └── TransactionEntryViewModel.cs  (ViewModel de formulario)
├── MainPage.xaml(.cs)                (Pantalla principal)
├── SecondPage.xaml(.cs)              (Pantalla de ingreso)
├── AppShell.xaml(.cs)                (Navegación)
├── App.xaml(.cs)                     (Configuración de aplicación)
└── MauiProgram.cs                    (Configuración inicial)
```

## Tecnologías Utilizadas

- .NET 10.0 MAUI (Multi-platform App UI)
- CommunityToolkit.Mvvm (patrones MVVM)
- SQLite con sqlite-net-pcl (persistencia)
- XAML para interfaces de usuario
- Android como plataforma destino

## Cómo Ejecutar

Requisitos previos:
- .NET 10 SDK instalado
- Android SDK configurado
- Emulador de Android o dispositivo físico conectado

Pasos:

1. Navega a la carpeta del proyecto:
   ```
   cd "Registro-de-Gastos"
   ```

2. Verifica que tu dispositivo esté visible en ADB:
   ```
   adb devices
   ```

3. Compila y ejecuta la aplicación:
   ```
   dotnet run --project RegistrodeGastos.csproj -f net10.0-android
   ```

4. La app se compilará e instalará automáticamente en tu dispositivo/emulador.

Para solo compilar sin ejecutar:
```
dotnet build RegistrodeGastos.csproj -f net10.0-android
```

## Arquitectura MVVM

La aplicación implementa el patrón MVVM utilizando Community Toolkit:

- **Models**: FinancialTransaction contiene los datos de cada transacción
- **ViewModels**: Contienen la lógica de negocio
  - HomeViewModel: Carga transacciones, calcula balances, maneja navegación
  - TransactionEntryViewModel: Valida y guarda nuevas transacciones
- **Views**: MainPage.xaml y SecondPage.xaml son las vistas que se enlazan al ViewModel
- **Services**: FinanceDatabaseService gestiona la conexión a SQLite

Los ViewModels utilizan:
- ObservableProperty para notificar cambios a la UI
- RelayCommand para ejecutar acciones desde botones
- ObservableCollection para listas que se actualizan automáticamente

## Base de Datos

La tabla FinancialTransaction almacena:
- Id (clave primaria, auto-increment)
- Amount (monto de la transacción)
- Description (descripción o glosa)
- IsIncome (boolean para diferenciar ingreso de egreso)
- CreatedAt (fecha y hora de la transacción)

La base de datos se crea automáticamente en la primera ejecución en la ruta:
`/data/data/com.inf320.AppGastos/files/finance.db3`

## Características de Interfaz

La aplicación cuenta con dos pantallas principales:

**MainPage (Pantalla principal)**
- Muestra el saldo disponible en un card destacado
- Dos cards laterales con totales de ingresos y egresos
- Botón para agregar nuevo movimiento
- Historial de transacciones con código de color (verde para ingresos, rojo para gastos)
- Mensaje cuando no hay transacciones registradas

**SecondPage (Agregar transacción)**
- Campo de texto para glosa o descripción
- Campo numérico para cantidad
- Selector de fecha
- Checkbox para indicar si es ingreso o gasto
- Botones para guardar o cancelar

## Validaciones

La aplicación valida:
- Cantidad debe ser un número mayor a cero
- Descripción no puede estar vacía
- Se debe seleccionar una fecha válida
- Los datos se guardan solo si pasan todas las validaciones

## Persistencia de Datos

Los datos se persisten en SQLite en el almacenamiento interno de la aplicación. Esto significa que:
- Las transacciones se mantienen incluso después de cerrar la app
- Los datos persisten entre reinicios del dispositivo
- Cerrar la app desde el gestor de aplicaciones no afecta los datos guardados