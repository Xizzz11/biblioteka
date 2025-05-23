# biblioteka

![image](https://github.com/user-attachments/assets/770c08e1-f2fa-4721-aeca-c0145492b176)

Диаграмма последовательностей

Название: Сценарий выдачи книги читателю

Описание: Демонстрирует взаимодействие объектов при выполнении ключевого процесса — оформления заказа.
Соответствие требованиям:

Применяет стандартные элементы UML (акторы, lifeline, сообщения).

Наглядно объясняет порядок действий системы.



![image](https://github.com/user-attachments/assets/73a56c3e-24b1-4b71-a94b-770f1706ef7b)

Диаграмма вариантов использования
Название: Функциональные возможности системы
Описание: Описывает, какие действия доступны библиотекарю.
Соответствие требованиям:

Использует стандартные символы (акторы, use case, связи include).

Лаконично перечисляет ключевые сценарии.

![image](https://github.com/user-attachments/assets/2e8b21d0-c45e-4648-a8e5-8ef4e79cbd94)

Диаграмма классов
Название: Структура данных системы управления библиотекой

Описание:
Диаграмма отображает ключевые сущности системы, их атрибуты, методы и взаимосвязи. Она служит основой для проектирования базы данных и бизнес-логики приложения.

Соответствие требованиям:

Использует стандартные обозначения UML (классы, атрибуты, методы, связи)

Включает все основные сущности предметной области

Четко показывает отношения между классами





Структура базы данных

Таблицы
1. Books — информация о книгах.
   - Поля: BookID (PK), Title, ISBN (уникальный), PublicationYear, PublisherID (FK), AvailableCopies
   - Связь: Один-ко-многим с Reviews, многие-ко-многим с Authors через BookAuthors
2. Authors — информация об авторах.
   - Поля: AuthorID (PK), FirstName, LastName, BirthYear
   - Связь: Многие-ко-многим с Books через BookAuthors
3. ublishers — информация об издательствах.
   - Поля: PublisherID (PK), Name, Address
   - Связь: Один-ко-многим с Books
4. Readers — информация о читателях.
   - Поля: ReaderID (PK), FirstName, LastName, Email (уникальный), Phone
   - Связь: Один-к-одному с LibraryCards, один-ко-многим с Orders и Reviews
5. LibraryCards — библиотечные карты.
   - Поля: CardID (PK), ReaderID (FK, уникальный), IssueDate, ExpiryDate
   - Связь: Один-к-одному с Readers
6. Orders — заказы (выдача/возврат книг).
   - Поля: OrderID (PK), BookID (FK), ReaderID (FK), BorrowDate, ReturnDate, Status
   - Связь: Один-ко-многим с Books и Readers
7. Reviews — отзывы о книгах.
   - Поля: ReviewID (PK), BookID (FK), ReaderID (FK), Rating, Comment, ReviewDate
   - Связь: Один-ко-многим с Books и Readers
8. BookAuthors — связь книг и авторов.
   - Поля: BookID (FK), AuthorID (FK)
   - Связь: Многие-ко-многим между Books и Authors

Типы связей
- Один-к-одному: Readers ↔ LibraryCards
- Один-ко-многим: Books → Reviews, Publishers → Books, Readers → Orders
- Многие-ко-многим: Books ↔ Authors (через BookAuthors)

Используемая СУБД
SQLite (файл: `library.db`)

SQL-скрипты
Скрипты для создания таблиц и начального наполнения находятся в `create_database.sql`.

Интеграция
Приложение использует `System.Data.SQLite` для работы с базой данных. Пример кода для подключения и базовых операций находится в `LibraryDatabase.cs`.

