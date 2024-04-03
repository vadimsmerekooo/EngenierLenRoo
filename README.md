# Проект расчитан для системного администратора организации.
Возможности:
- Авторизация;
- Чат реализованый при помощи SignalR;
- Отправка и загрузка файлов/изображений в чат;
- Регистрация по заявке;
- Управление кабинетами и сотруднкиами;
- Управление заправкой картриджей;
## Проект разработан при помощи:
- Asp.Net Core 7
- Bootstrap 5
- SignalR
- EntityFramework
- Ms Sql Server
____
1. Чат
> Чат разделен на 3 части: 
  - Список Ваших чатов;
  - Список сотрудников которым вы можете написать;
  - Окно чата;
![alt text](https://github.com/vadimsmerekooo/EngenierLenRoo/blob/main/resources/Chat.png)

Возможности:
  - Отправка текстовых сообщений;
  - ![alt text](https://github.com/vadimsmerekooo/EngenierLenRoo/blob/main/resources/Chat-MessagesBox.png)
  - Отправка файлов и изображений;
  - Создание групповых чатов.
  - ![alt text](https://github.com/vadimsmerekooo/EngenierLenRoo/blob/main/resources/Chat-MessagesBox2.png)
____
2. Управление кабинетами
> В ифнормации о кабинете доступны список сотрудников с их ифнормацией, а так же списком техники записанных на из.

![alt text](https://github.com/vadimsmerekooo/EngenierLenRoo/blob/main/resources/Cabinets.png)

Возможности:
- Добавление сотрудника;
- Редактирование;
- Удаление;
- Сотрудник:
  - Редактирование Сотрудника;
  - Перемещение с кабинета в кабинет;
  - Добавление техники;
  - Техника:
    - Добавление/Удаление;
    - Изменение;
    - Перемещение; 
  - Удаление;
____
3. Управление техникой 
![alt text](https://github.com/vadimsmerekooo/EngenierLenRoo/blob/main/resources/Techniques.png)
____
4.  Управление картриджами
    > Данный раздел предназначен для упрощения работы с картриджами сотрудников, которые они сдают на заправку.

![alt text](https://github.com/vadimsmerekooo/EngenierLenRoo/blob/main/resources/Cartridges.png)

Возможности:
- Добавление/Удаление коробки;
  - Добавление/удаление картриджа в(из) коробку(-и);
  - Изменение статуса коробки с картриджами.
____
5.  Логгер (NLog)

![alt text](https://github.com/vadimsmerekooo/EngenierLenRoo/blob/main/resources/Logerr.png)
    
