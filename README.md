# EL CERDO VOLADOR

Por favor considere este proyecto un vertical slice.  
Funcional y casi completo en la estructura, pero no en el contenido jugable.

---

## INSTRUCCIONES DE INSTALACIÓN

Antes de importar es recomendable crear las **Tags** y **Layers** que tengo en el proyecto.  
No estoy seguro, pero creo que Unity no los traspasa al enviar Assets.  
Algunos pueden haber sido creados pero no usados, esto por haber hecho refactorizaciones de código.  
Esto evitará errores.

### Tags
```text
Enemy 
```

```text
Floor
```

```text
BulletP
```

```text
BulletE
```

### Layers
```text
Player
```

```text
Obstacle
```
---

## MINIDOCUMENTACIÓN DE SCRIPTS

### Gameplay

#### Jugador
- **PlayerController**: asigno los inputs (compatibles con teclado y mando) en el editor y mando el.  
- **PlayerMove**: no se comunica con `PlayerController`, espera una entrada, siempre se mueve hacia adelante. Rota con Quaternions usando Euler angles, solo dos ángulos para evitar Gimbal Lock.

#### Enemigo
- **EnemyController**: máquina de estados, detecta al jugador o patrulla unos puntos específicos.  
- **EnemyMove**: mueve al enemigo a su objetivo establecido.

#### Disparo
- **Shoot**: encargado de disparar tanto para el jugador como para enemigos. Es modular porque este no manda nada, solo recibe.  
- **Bullet**: se mueve hacia adelante, se comunica con el pool. No detecta nada; serán otros scripts quienes lo hagan.

### Sistemas de juego
- **GameManager** (singleton): se encarga de manejar la victoria, pausa, tener una referencia del GameObject del jugador, calcular la puntuación, entre otras cosas.  
- **PoolManager** (singleton): como habrá muchas balas, no sería óptimo hacer un pool de objetos de cada enemigo y jugador, por lo que esto lo maneja de manera accesible y modular.  
- **Manager1**: aunque pongo Lvl1 en su carpeta, me ha servido de manera simple para el Lvl2. Se encarga de detectar objetos (Ballon), y cuando todos se destruyen pasa a la siguiente escena.

### UI
- **EnableDisable**: método público utilizado para abrir UI accediendo desde botones.  
- **ButtonEnableSelectUI**: al activarse se selecciona; esto permite que al usar el mando se pueda navegar sin ratón.  
- **LoadScene**: sencillo, hace un método público para cargar una escena usando un botón.

---

## CRÉDITOS

### Arte y Modelos 3D
- Hunyuan: generación de modelos 3D highpoly y Skybox.  
- Low poly y texturizado en Blender.  
- Sprites simples en GIMP.

### Sistemas y Assets
- Radar System Lite de la AssetStore, modificado:  
  - Cambiar de un círculo a un medio círculo.  
  - Iconos para detectar si está arriba o debajo del jugador.  
- Shader de agua de LuisCanary en un tutorial de YouTube.

### Inspiración
- Para dudas rápidas de programación he preguntado a ChatGPT y DeepSeek, además de consultar cómo enfocar algunos sistemas para tener una idea general y hacer correcciones de código.  
- Para los diálogos he preguntado a ChatGPT para inspirarme en chistes malos.

### Enlaces
- Radar system: [https://assetstore.unity.com/packages/tools/game-toolkits/radar-system-lite-plug-play-solution-226229](https://assetstore.unity.com/packages/tools/game-toolkits/radar-system-lite-plug-play-solution-226229)  
- Shader agua: [https://youtu.be/wWJGaEDA6Rk?si=LwyynaKKowrEoF2I](https://youtu.be/wWJGaEDA6Rk?si=LwyynaKKowrEoF2I)
