# Arquitectura de Servicios - CodeCraft.NET

## Problema Original

Anteriormente, tanto **WebAPI** como **DesktopAPI** tenían código duplicado para:
- Manejo de excepciones
- Logging patterns
- Validación de entrada
- Transformación de resultados
- Lógica de orquestación básica

## Solución: Proyecto Services Intermedio

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
- **Lógica de negocio común** entre WebAPI y DesktopAPI
- **Manejo consistente de errores** con ServiceResult<T>
- **Logging estandarizado** para todas las operaciones
- **Validación y transformación** de datos
- **Orquestación** de Commands y Queries de CQRS

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
- Clase base abstracta con toda la lógica común
- Implementa patrones estándar de CRUD
- Manejo consistente de excepciones y logging
- Template method pattern para operaciones específicas

#### 3. Servicios Específicos por Entidad
```csharp
public class ProductService : BaseEntityService<ProductCreate, ProductUpdate, ProductDto, ProductWithRelatedDto>
{
    // Solo implementa los métodos abstractos específicos
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

### ? **Eliminación de Duplicación**
- Un solo punto para lógica de negocio
- Manejo de errores consistente
- Logging unificado

### ? **Mantenibilidad**
- Cambios en lógica de negocio en un solo lugar
- Fácil testing de la lógica común
- Separación clara de responsabilidades

### ? **Flexibilidad**
- WebAPI se enfoca en HTTP concerns
- DesktopAPI se enfoca en MAUI concerns
- Services se enfoca en business logic

### ? **Reutilización**
- Mismo código para ambas APIs
- Posibilidad de agregar nuevas facades (GraphQL, gRPC, etc.)
- Testing más eficiente

## Flujo de Datos

### Operación Create:
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
- **Mismo número de calls**: No se agregan calls adicionales a la base de datos
- **Beneficio en testing**: Menos mocking necesario
- **Beneficio en debugging**: Stack trace más claro

## Próximos Pasos

1. **Actualizar Generator**: Modificar plantillas para usar Services
2. **Actualizar ServiceRegistration**: Registrar servicios automáticamente
3. **Migration de código existente**: Mover lógica a Services
4. **Testing**: Crear tests unitarios para Services layer
5. **Documentation**: Actualizar README con nueva arquitectura