# Sistema de Gestión de Habitaciones MONITOR-WEB

![Main Page](/Imgs_App/Initial_Page.jpg)

Este proyecto es una aplicación diseñada para la gestión de habitaciones de un establecimiento o Empresa, como un hotel o un complejo turístico. Cada habitación puede tener varios estados, y la aplicación permite realizar un seguimiento detallado de la disponibilidad y el estado de cada habitación a través del protocolo MQTT.


## Estados de Habitación

![Estados de Habitación](/Imgs_App/Estados.jpg)

El sistema admite los siguientes estados de habitación, definidos como una enumeración en el código:

- **Error:** Representa un estado de error o excepción en la habitación.

- **Inicialización (Init):** Estado inicial de una habitación que está lista para ser configurada.

- **Limpieza:** Indica que la habitación se encuentra en proceso de limpieza después de que un cliente haya salido.

- **Ocupada:** La habitación está actualmente ocupada por un cliente.

- **Libre:** La habitación está disponible y lista para ser ocupada por un cliente.

- **Mantenimiento:** Indica que la habitación está siendo sometida a tareas de mantenimiento o reparación.

- **Cerrada:** La habitación está cerrada, para luego pasar al estado de ocupada.

- **Abierta:** La habitación ha sido reabierta después de un período de cierre.

## Almacenamiento de Datos

El sistema utiliza LiteDB, una base de datos incorporada de código abierto escrita en C#, para almacenar y gestionar los registros de los estados de las habitaciones. LiteDB es una excelente opción para aplicaciones locales que requieren una base de datos ligera y eficiente.

## Comunicación a través de MQTT

El sistema utiliza el protocolo MQTT para la comunicación en tiempo real entre las habitaciones y el sistema central. Cada habitación está equipada con un dispositivo MQTT que publica y suscribe a los estados de la habitación y otros datos relevantes. Esto permite un seguimiento en tiempo real de la ocupación y el estado de todas las habitaciones en el establecimiento.

## Temporizadores

Cada estado de habitación cuenta con un temporizador que calcula el tiempo que la habitación ha estado en ese estado. Esto es útil para llevar un registro de cuánto tiempo ha pasado desde la última limpieza, cuánto tiempo ha estado ocupada, etc.

## Información de Ocupación

El sistema realiza un seguimiento de cuántas habitaciones han estado libres u ocupadas en cada turno. Esto proporciona información útil para la planificación y la toma de decisiones.

## Informes y Estadísticas

El sistema genera informes y estadísticas sobre la ocupación de las habitaciones, y otros datos relevantes, lo que ayuda a la toma de decisiones y la planificación, cada en informe es enviado por correo en formato PDF o HTML.

## Configuración Completa

![Configuración Completa](/Imgs_App/Cofiguration_Page.jpg)

El sistema cuenta con un apartado de configuración que permite personalizar todos los aspectos del sistema, incluyendo:

- Configuración del servidor MQTT para la comunicación en tiempo real con las habitaciones.

- Configuración de las habitaciones, incluyendo número, tipo, capacidad y detalles.

- Definición de turnos y horarios de trabajo para el personal.

- Configuración de correos electrónicos para notificaciones y comunicaciones con los clientes.


## Requisitos del Sistema

- Lenguaje de Programación: C#
- Framework: Blazor
- Base de Datos: LiteDB

## Configuración y Uso

1. Clona el repositorio a tu servidor local.
2. Configura el servidor MQTT y otros parámetros de configuración según sea necesario.
3. Compila y ejecuta la aplicación.

## Contribución

Si deseas contribuir a este proyecto, sigue estos pasos:

1. Hace un fork de este repositorio en GitHub.
2. Clona el repositorio bifurcado en tu máquina local.
3. Crea una nueva rama para tu función o corrección de errores.
4. Realiza tus cambios y envía una solicitud de extracción.

## Licencia

Este proyecto está bajo la licencia [Licencia MIT](LICENSE).

## Contacto

Si tienes preguntas o sugerencias, no dudes en ponerte en contacto conmigo.

linkedin.com/in/xavierjp/
