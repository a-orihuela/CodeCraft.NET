# ?? Sistema de Estilos MAUI - KetoPlanner

Este documento describe el sistema de estilos moderno y consistente para la aplicación **KetoPlanner** en .NET MAUI.

## ?? **Filosofía de Diseño**

- **Consistencia multiplataforma**: Los estilos funcionan idénticamente en Windows, Android, iOS y macOS
- **Accesibilidad**: Soporte para Light/Dark mode automático
- **Funcionalidad por color**: Cada acción tiene su color específico (guardar = verde, eliminar = rojo, etc.)
- **Material Design + Fluent Design**: Híbrido de los mejores elementos de ambos sistemas

---

## ?? **Estructura de Archivos**

```
Resources/Styles/
??? Colors.xaml          # Paleta de colores completa
??? ButtonStyles.xaml    # Estilos específicos de botones
??? ComponentStyles.xaml # Cards, Forms, Layouts
??? Styles.xaml         # Estilos base + importaciones
```

---

## ?? **Paleta de Colores**

### **Colores Primarios**
- `Primary` - #10B981 (Verde Keto)
- `PrimaryDark` - #065F46 
- `PrimaryLight` - #D1FAE5
- `PrimaryAccent` - #059669

### **Colores por Función**
- `SaveButton` - #10B981 (Verde) ??
- `EditButton` - #F59E0B (Naranja) ??
- `DeleteButton` - #EF4444 (Rojo) ???
- `CancelButton` - #6B7280 (Gris) ?
- `CreateButton` - #3B82F6 (Azul) ?
- `ViewButton` - #8B5CF6 (Púrpura) ???

### **Colores Nutricionales**
- `ProteinColor` - #EF4444 (Rojo para proteínas)
- `CarbColor` - #F59E0B (Naranja para carbohidratos)
- `FatColor` - #10B981 (Verde para grasas)
- `FiberColor` - #8B5CF6 (Púrpura para fibra)

---

## ?? **Botones por Función**

### **Uso en XAML:**

```xaml
<!-- Botón para guardar -->
<Button Text="?? Guardar" Style="{StaticResource SaveButtonStyle}" />

<!-- Botón para editar -->
<Button Text="?? Editar" Style="{StaticResource EditButtonStyle}" />

<!-- Botón para eliminar -->
<Button Text="??? Eliminar" Style="{StaticResource DeleteButtonStyle}" />

<!-- Botón para cancelar -->
<Button Text="? Cancelar" Style="{StaticResource CancelButtonStyle}" />

<!-- Botón para crear -->
<Button Text="? Crear" Style="{StaticResource CreateButtonStyle}" />

<!-- Botón para ver -->
<Button Text="??? Ver" Style="{StaticResource ViewButtonStyle}" />
```

### **Botones Outline:**

```xaml
<!-- Botón outline primario -->
<Button Text="Primario" Style="{StaticResource OutlinePrimaryButtonStyle}" />

<!-- Botón outline secundario -->
<Button Text="Secundario" Style="{StaticResource OutlineSecondaryButtonStyle}" />
```

### **Botones de Categoría:**

```xaml
<!-- Botones para categorías nutricionales -->
<Button Text="?? Proteínas" Style="{StaticResource ProteinButtonStyle}" />
<Button Text="?? Carbohidratos" Style="{StaticResource CarbButtonStyle}" />
<Button Text="?? Grasas" Style="{StaticResource FatButtonStyle}" />
```

---

## ?? **Cards y Contenedores**

### **Tipos de Cards:**

```xaml
<!-- Card estándar -->
<Frame Style="{StaticResource CardStyle}">
    <StackLayout>
        <Label Text="Contenido del card" />
    </StackLayout>
</Frame>

<!-- Card elevado (para contenido importante) -->
<Frame Style="{StaticResource ElevatedCardStyle}">
    <StackLayout>
        <Label Text="Contenido destacado" />
    </StackLayout>
</Frame>

<!-- Card compacto (para listas) -->
<Frame Style="{StaticResource CompactCardStyle}">
    <StackLayout>
        <Label Text="Elemento de lista" />
    </StackLayout>
</Frame>

<!-- Card de estadística -->
<Frame Style="{StaticResource StatCardStyle}">
    <StackLayout>
        <Label Text="Métrica importante" />
    </StackLayout>
</Frame>

<!-- Card de alerta -->
<Frame Style="{StaticResource AlertCardStyle}">
    <StackLayout>
        <Label Text="?? Advertencia importante" />
    </StackLayout>
</Frame>
```

### **Borders Modernos:**

```xaml
<!-- Border principal -->
<Border Style="{StaticResource MainBorderStyle}">
    <Label Text="Contenido con border" />
</Border>

<!-- Border para inputs -->
<Border Style="{StaticResource InputBorderStyle}">
    <Entry Placeholder="Campo de texto" />
</Border>

<!-- Border con acento -->
<Border Style="{StaticResource AccentBorderStyle}">
    <Label Text="Contenido destacado" />
</Border>
```

---

## ?? **Formularios**

### **Estructura Recomendada:**

```xaml
<Frame Style="{StaticResource CardStyle}">
    <StackLayout Spacing="16">
        <!-- Campo de texto -->
        <StackLayout Style="{StaticResource FormGroupStyle}">
            <Label Text="Nombre:" Style="{StaticResource FormLabelStyle}" />
            <Border Style="{StaticResource InputBorderStyle}">
                <Entry Placeholder="Ingresa tu nombre" 
                       Style="{StaticResource FormEntryStyle}" />
            </Border>
        </StackLayout>

        <!-- Selector -->
        <StackLayout Style="{StaticResource FormGroupStyle}">
            <Label Text="Opción:" Style="{StaticResource FormLabelStyle}" />
            <Border Style="{StaticResource InputBorderStyle}">
                <Picker Title="Selecciona" 
                        Style="{StaticResource FormPickerStyle}">
                    <!-- Items del picker -->
                </Picker>
            </Border>
        </StackLayout>

        <!-- Botones de acción -->
        <Grid ColumnDefinitions="*,*" ColumnSpacing="12">
            <Button Grid.Column="0" Text="?? Guardar" 
                    Style="{StaticResource SaveButtonStyle}" />
            <Button Grid.Column="1" Text="? Cancelar" 
                    Style="{StaticResource CancelButtonStyle}" />
        </Grid>
    </StackLayout>
</Frame>
```

---

## ??? **Badges y Estados**

```xaml
<!-- Badge de estado activo -->
<Frame Style="{StaticResource BadgeStyle}" 
       BackgroundColor="{StaticResource Success}">
    <Label Text="Activo" Style="{StaticResource StatusBadgeStyle}" />
</Frame>

<!-- Badge de advertencia -->
<Frame Style="{StaticResource BadgeStyle}" 
       BackgroundColor="{StaticResource Warning}">
    <Label Text="Pendiente" Style="{StaticResource StatusBadgeStyle}" />
</Frame>

<!-- Badge de error -->
<Frame Style="{StaticResource BadgeStyle}" 
       BackgroundColor="{StaticResource Danger}">
    <Label Text="Error" Style="{StaticResource StatusBadgeStyle}" />
</Frame>
```

---

## ?? **Títulos y Navegación**

```xaml
<!-- Header de página -->
<Label Text="Título Principal" 
       Style="{StaticResource PageHeaderStyle}" />

<!-- Subtítulo -->
<Label Text="Descripción de la página" 
       Style="{StaticResource PageSubtitleStyle}" />

<!-- Títulos en cards -->
<Label Text="Título Grande" Style="{StaticResource Headline}" />
<Label Text="Subtítulo" Style="{StaticResource SubHeadline}" />
```

---

## ?? **Soporte Dark Mode**

Todos los estilos incluyen soporte automático para Dark Mode usando `AppThemeBinding`:

```xaml
<!-- Ejemplo: Color que cambia automáticamente -->
<Setter Property="TextColor" 
        Value="{AppThemeBinding Light={StaticResource Gray900}, Dark={StaticResource White}}" />
```

### **Colores Dark Mode:**
- `DarkBackground` - #0F172A (Fondo principal)
- `DarkSurface` - #1E293B (Superficie de cards)
- `DarkBorder` - #334155 (Bordes)

---

## ? **Características Avanzadas**

### **Animaciones Incluidas:**
- **Pressed State**: Todos los botones tienen efecto de escala (0.96)
- **Shadows**: Cards y botones importantes tienen sombras automáticas
- **Hover Effects**: Estados visuales para interacción

### **Responsividad:**
- **Breakpoints**: Los layouts se adaptan automáticamente
- **Touch Targets**: Mínimo 44px para accesibilidad
- **Spacing Consistente**: Sistema de spacing basado en múltiplos de 4px

---

## ?? **Ejemplos de Uso por Módulo**

### **Dashboard:**
```xaml
<Frame Style="{StaticResource StatCardStyle}">
    <StackLayout>
        <Label Text="Planes Creados" Style="{StaticResource FormLabelStyle}" />
        <Label Text="12" FontSize="32" FontFamily="OpenSansSemibold" />
    </StackLayout>
</Frame>
```

### **Formulario de Usuario:**
```xaml
<StackLayout Style="{StaticResource FormGroupStyle}">
    <Label Text="Peso (kg):" Style="{StaticResource FormLabelStyle}" />
    <Border Style="{StaticResource InputBorderStyle}">
        <Entry Keyboard="Numeric" Style="{StaticResource FormEntryStyle}" />
    </Border>
</StackLayout>
```

### **Lista de Planes:**
```xaml
<Frame Style="{StaticResource CompactCardStyle}">
    <Grid ColumnDefinitions="*,Auto">
        <StackLayout Grid.Column="0">
            <Label Text="Plan Keto Básico" FontFamily="OpenSansSemibold" />
            <Label Text="Creado: 15/12/2024" TextColor="{StaticResource Gray500}" />
        </StackLayout>
        <Button Grid.Column="1" Text="???" Style="{StaticResource ViewButtonStyle}" />
    </Grid>
</Frame>
```

---

## ?? **Mejores Prácticas**

1. **Consistencia**: Usa siempre el estilo correspondiente a la función del botón
2. **Jerarquía**: Usa `ElevatedCardStyle` solo para contenido muy importante
3. **Accesibilidad**: Mantén los textos con contraste adecuado
4. **Performance**: Los estilos están optimizados para renderizado rápido
5. **Mantenimiento**: Si necesitas un color nuevo, agrégalo a `Colors.xaml` primero

---

## ?? **Personalización**

### **Agregar un Nuevo Color:**
1. Añádelo en `Colors.xaml`
2. Crea el `SolidColorBrush` correspondiente
3. Úsalo en tus estilos personalizados

### **Crear un Estilo Personalizado:**
```xaml
<Style x:Key="MiEstiloPersonalizado" TargetType="Button" BasedOn="{StaticResource SaveButtonStyle}">
    <Setter Property="CornerRadius" Value="20" />
    <Setter Property="FontSize" Value="18" />
</Style>
```

---

## ?? **Ver Demostración**

Para ver todos los estilos en acción, navega a `StyleDemoPage.xaml` que incluye ejemplos de todos los componentes y sus variaciones.

---

**? ¡Con este sistema de estilos, tu aplicación tendrá una apariencia profesional y consistente en todas las plataformas! ?**