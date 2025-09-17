# Arquitectura de Servicios - CodeCraft.NET

## Problema Original

Anteriormente, tanto **WebAPI** como **DesktopAPI** ten�an c�digo duplicado para:
- Manejo de excepciones
- Logging patterns
- Validaci�n de entrada
- Transformaci�n de resultados
- L�gica de orquestaci�n b�sica

## Soluci�n: Proyecto Services Intermedio

### Nueva Estructura de Capas

```
???????????????????????????????????????????????????????????????
?                    Presentation Layer                       ?
???????????????????????????????????????????????????????????????
?     WebAPI          ?           DesktopAPI                  ?
?   (HTTP Facade)     ?        (Direct Facade)               ?
???????????????????????????????????????????????????????????????
?                    Services Layer                           ?
?              (Business Orchestration)                       ?
???????????????????????????????????????????????????????????????
?                  Application Layer                          ?
?                    (CQRS/MediatR)                          ?
???????????????????????????????????????????????????????????????
?                   Domain Layer                              ?
???????????????????????????????????????????????????????????????
```

## Proyecto CodeCraft.NET.Services

### Responsabilidades:
- **L�gica de negocio com�n** entre WebAPI y DesktopAPI
- **Manejo consistente de errores** con ServiceResult<T>
- **Logging estandarizado** para todas las operaciones
- **Validaci�n y transformaci�n** de datos
- **Orquestaci�n** de Commands y Queries de CQRS

### Componentes Principales:

#### 1. ServiceResult<T>
```csharp
public class ServiceResult<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }
    public List<string> ValidationErrors { get; private set; }
}
```

#### 2. BaseEntityService<T>
- Clase base abstracta con toda la l�gica com�n
- Implementa patrones est�ndar de CRUD
- Manejo consistente de excepciones y logging
- Template method pattern para operaciones espec�ficas

#### 3. Servicios Espec�ficos por Entidad
```csharp
public class ProductService : BaseEntityService<ProductCreate, ProductUpdate, ProductDto, ProductWithRelatedDto>
{
    // Solo implementa los m�todos abstractos espec�ficos
    protected override object CreateDeleteCommand(int id) => new ProductDelete { Id = id };
    protected override object CreateGetByIdQuery(int id) => new GetProductByIdQuery { Id = id };
    protected override object CreateGetWithRelatedQuery(int id) => new GetProductWithRelatedQuery { Id = id };
}
```

## APIs como Fachadas

### WebAPI Controller (HTTP Facade)
```csharp
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreate command)
    {
        var result = await _productService.CreateAsync(command);
        
        if (result.IsSuccess)
            return Ok(new { id = result.Data });
            
        return BadRequest(new { error = result.ErrorMessage, validationErrors = result.ValidationErrors });
    }
    
    // Convierte ServiceResult<T> a IActionResult HTTP
}
```

### DesktopAPI Service (Direct Facade)
```csharp
public class ProductDesktopService
{
    private readonly ProductService _productService;

    public async Task<ServiceResult<int>> CreateAsync(ProductCreate command)
    {
        return await _productService.CreateAsync(command);
        // Retorna directamente ServiceResult<T> para MAUI
    }
}
```

## Ventajas de esta Arquitectura

### ? **Eliminaci�n de Duplicaci�n**
- Un solo punto para l�gica de negocio
- Manejo de errores consistente
- Logging unificado

### ? **Mantenibilidad**
- Cambios en l�gica de negocio en un solo lugar
- F�cil testing de la l�gica com�n
- Separaci�n clara de responsabilidades

### ? **Flexibilidad**
- WebAPI se enfoca en HTTP concerns
- DesktopAPI se enfoca en MAUI concerns
- Services se enfoca en business logic

### ? **Reutilizaci�n**
- Mismo c�digo para ambas APIs
- Posibilidad de agregar nuevas facades (GraphQL, gRPC, etc.)
- Testing m�s eficiente

## Flujo de Datos

### Operaci�n Create:
```
HTTP Request ? WebAPI Controller ? ProductService ? MediatR ? Command Handler ? Repository ? Database
     ?                                    ?
ServiceResult<int> ? HTTP Response ? ServiceResult<int>

MAUI Call ? DesktopAPI Service ? ProductService (same as above)
     ?                              ?
ServiceResult<int> ? Direct Return ? ServiceResult<int>
```

## Migration Path

### Paso 1: ? Crear proyecto Services
### Paso 2: ? Definir interfaces y base classes
### Paso 3: ?? Actualizar templates del Generator
### Paso 4: ?? Migrar controllers existentes
### Paso 5: ?? Actualizar registration de servicios

## Consideraciones de Performance

- **Sin overhead adicional**: Las fachadas son thin layers
- **Mismo n�mero de calls**: No se agregan calls adicionales a la base de datos
- **Beneficio en testing**: Menos mocking necesario
- **Beneficio en debugging**: Stack trace m�s claro

## Pr�ximos Pasos

1. **Actualizar Generator**: Modificar plantillas para usar Services
2. **Actualizar ServiceRegistration**: Registrar servicios autom�ticamente
3. **Migration de c�digo existente**: Mover l�gica a Services
4. **Testing**: Crear tests unitarios para Services layer
5. **Documentation**: Actualizar README con nueva arquitectura