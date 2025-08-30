# ?? Sistema de Estilos MAUI - KetoPlanner

Este documento describe el sistema de estilos moderno y consistente para la aplicaci�n **KetoPlanner** en .NET MAUI.

## ?? **Filosof�a de Dise�o**

- **Consistencia multiplataforma**: Los estilos funcionan id�nticamente en Windows, Android, iOS y macOS
- **Accesibilidad**: Soporte para Light/Dark mode autom�tico
- **Funcionalidad por color**: Cada acci�n tiene su color espec�fico (guardar = verde, eliminar = rojo, etc.)
- **Material Design + Fluent Design**: H�brido de los mejores elementos de ambos sistemas

---

## ?? **Estructura de Archivos**

```
Resources/Styles/
??? Colors.xaml          # Paleta de colores completa
??? ButtonStyles.xaml    # Estilos espec�ficos de botones
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

### **Colores por Funci�n**
- `SaveButton` - #10B981 (Verde) ??
- `EditButton` - #F59E0B (Naranja) ??
- `DeleteButton` - #EF4444 (Rojo) ???
- `CancelButton` - #6B7280 (Gris) ?
- `CreateButton` - #3B82F6 (Azul) ?
- `ViewButton` - #8B5CF6 (P�rpura) ???

### **Colores Nutricionales**
- `ProteinColor` - #EF4444 (Rojo para prote�nas)
- `CarbColor` - #F59E0B (Naranja para carbohidratos)
- `FatColor` - #10B981 (Verde para grasas)
- `FiberColor` - #8B5CF6 (P�rpura para fibra)

---

## ?? **Botones por Funci�n**

### **Uso en XAML:**

```xaml
<!-- Bot�n para guardar -->
<Button Text="?? Guardar" Style="{StaticResource SaveButtonStyle}" />

<!-- Bot�n para editar -->
<Button Text="?? Editar" Style="{StaticResource EditButtonStyle}" />

<!-- Bot�n para eliminar -->
<Button Text="??? Eliminar" Style="{StaticResource DeleteButtonStyle}" />

<!-- Bot�n para cancelar -->
<Button Text="? Cancelar" Style="{StaticResource CancelButtonStyle}" />

<!-- Bot�n para crear -->
<Button Text="? Crear" Style="{StaticResource CreateButtonStyle}" />

<!-- Bot�n para ver -->
<Button Text="??? Ver" Style="{StaticResource ViewButtonStyle}" />
```

### **Botones Outline:**

```xaml
<!-- Bot�n outline primario -->
<Button Text="Primario" Style="{StaticResource OutlinePrimaryButtonStyle}" />

<!-- Bot�n outline secundario -->
<Button Text="Secundario" Style="{StaticResource OutlineSecondaryButtonStyle}" />
```

### **Botones de Categor�a:**

```xaml
<!-- Botones para categor�as nutricionales -->
<Button Text="?? Prote�nas" Style="{StaticResource ProteinButtonStyle}" />
<Button Text="?? Carbohidratos" Style="{StaticResource CarbButtonStyle}" />
<Button Text="?? Grasas" Style="{StaticResource FatButtonStyle}" />
```

---

## ?? **Cards y Contenedores**

### **Tipos de Cards:**

```xaml
<!-- Card est�ndar -->
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

<!-- Card de estad�stica -->
<Frame Style="{StaticResource StatCardStyle}">
    <StackLayout>
        <Label Text="M�trica importante" />
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
            <Label Text="Opci�n:" Style="{StaticResource FormLabelStyle}" />
            <Border Style="{StaticResource InputBorderStyle}">
                <Picker Title="Selecciona" 
                        Style="{StaticResource FormPickerStyle}">
                    <!-- Items del picker -->
                </Picker>
            </Border>
        </StackLayout>

        <!-- Botones de acci�n -->
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

## ?? **T�tulos y Navegaci�n**

```xaml
<!-- Header de p�gina -->
<Label Text="T�tulo Principal" 
       Style="{StaticResource PageHeaderStyle}" />

<!-- Subt�tulo -->
<Label Text="Descripci�n de la p�gina" 
       Style="{StaticResource PageSubtitleStyle}" />

<!-- T�tulos en cards -->
<Label Text="T�tulo Grande" Style="{StaticResource Headline}" />
<Label Text="Subt�tulo" Style="{StaticResource SubHeadline}" />
```

---

## ?? **Soporte Dark Mode**

Todos los estilos incluyen soporte autom�tico para Dark Mode usando `AppThemeBinding`:

```xaml
<!-- Ejemplo: Color que cambia autom�ticamente -->
<Setter Property="TextColor" 
        Value="{AppThemeBinding Light={StaticResource Gray900}, Dark={StaticResource White}}" />
```

### **Colores Dark Mode:**
- `DarkBackground` - #0F172A (Fondo principal)
- `DarkSurface` - #1E293B (Superficie de cards)
- `DarkBorder` - #334155 (Bordes)

---

## ? **Caracter�sticas Avanzadas**

### **Animaciones Incluidas:**
- **Pressed State**: Todos los botones tienen efecto de escala (0.96)
- **Shadows**: Cards y botones importantes tienen sombras autom�ticas
- **Hover Effects**: Estados visuales para interacci�n

### **Responsividad:**
- **Breakpoints**: Los layouts se adaptan autom�ticamente
- **Touch Targets**: M�nimo 44px para accesibilidad
- **Spacing Consistente**: Sistema de spacing basado en m�ltiplos de 4px

---

## ?? **Ejemplos de Uso por M�dulo**

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
            <Label Text="Plan Keto B�sico" FontFamily="OpenSansSemibold" />
            <Label Text="Creado: 15/12/2024" TextColor="{StaticResource Gray500}" />
        </StackLayout>
        <Button Grid.Column="1" Text="???" Style="{StaticResource ViewButtonStyle}" />
    </Grid>
</Frame>
```

---

## ?? **Mejores Pr�cticas**

1. **Consistencia**: Usa siempre el estilo correspondiente a la funci�n del bot�n
2. **Jerarqu�a**: Usa `ElevatedCardStyle` solo para contenido muy importante
3. **Accesibilidad**: Mant�n los textos con contraste adecuado
4. **Performance**: Los estilos est�n optimizados para renderizado r�pido
5. **Mantenimiento**: Si necesitas un color nuevo, agr�galo a `Colors.xaml` primero

---

## ?? **Personalizaci�n**

### **Agregar un Nuevo Color:**
1. A��delo en `Colors.xaml`
2. Crea el `SolidColorBrush` correspondiente
3. �salo en tus estilos personalizados

### **Crear un Estilo Personalizado:**
```xaml
<Style x:Key="MiEstiloPersonalizado" TargetType="Button" BasedOn="{StaticResource SaveButtonStyle}">
    <Setter Property="CornerRadius" Value="20" />
    <Setter Property="FontSize" Value="18" />
</Style>
```

---

## ?? **Ver Demostraci�n**

Para ver todos los estilos en acci�n, navega a `StyleDemoPage.xaml` que incluye ejemplos de todos los componentes y sus variaciones.

---

**? �Con este sistema de estilos, tu aplicaci�n tendr� una apariencia profesional y consistente en todas las plataformas! ?**