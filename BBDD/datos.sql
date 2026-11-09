-- =============================================
-- BibliotecaAPI - Datos de la BBDD
-- Las tablas las crea la migración de EF (InitialCreate).
-- Ejecutar en SQL Server Management Studio (F5).
-- Se puede ejecutar todas las veces que se quiera: siempre deja la BBDD igual.
-- =============================================
USE [Biblioteca];
GO

-- 1. Vaciar las tablas (primero la intermedia, luego las que tienen claves ajenas)
DELETE FROM AuthorBook;
DELETE FROM Books;
DELETE FROM Authors;
DELETE FROM Categories;

-- 2. Categorías
SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (Id, Name, Description) VALUES
(1, N'Novela',          N'Narrativa de ficción'),
(2, N'Ciencia ficción', N'Futuro, ciencia y tecnología'),
(3, N'Historia',        N'Libros de divulgación histórica'),
(4, N'Fantasía',        N'Mundos imaginarios');
SET IDENTITY_INSERT Categories OFF;

-- 3. Autores
SET IDENTITY_INSERT Authors ON;
INSERT INTO Authors (Id, Name, Nationality, BirthYear) VALUES
(1, N'Miguel de Cervantes', N'española',       1547),
(2, N'Isaac Asimov',        N'estadounidense', 1920),
(3, N'Mary Shelley',        N'británica',      1797),
(4, N'Terry Pratchett',     N'británica',      1948),
(5, N'Neil Gaiman',         N'británica',      1960);
SET IDENTITY_INSERT Authors OFF;

-- 4. Libros (CategoryId es la clave ajena a Categories)
SET IDENTITY_INSERT Books ON;
INSERT INTO Books (Id, Title, Isbn, Pages, PublicationYear, CategoryId) VALUES
(1, N'Don Quijote',      N'9780000000001', 1200, 1605, 1),
(2, N'Fundación',        N'9780000000002',  250, 1951, 2),
(3, N'Yo, robot',        N'9780000000003',  280, 1950, 2),
(4, N'Frankenstein',     N'9780000000004',  300, 1818, 1),
(5, N'Buenos presagios', N'9780000000005',  400, 1990, 4);
SET IDENTITY_INSERT Books OFF;

-- 5. Autores de cada libro (tabla intermedia de la relación M:N, sin IDENTITY)
INSERT INTO AuthorBook (AuthorsId, BooksId) VALUES
(1, 1),           -- Don Quijote: Cervantes
(2, 2),           -- Fundación: Asimov
(2, 3),           -- Yo, robot: Asimov
(3, 4),           -- Frankenstein: Mary Shelley
(4, 5), (5, 5);   -- Buenos presagios: Pratchett y Gaiman

-- 6. Que los próximos Id generados sigan después de los insertados
DBCC CHECKIDENT ('Categories', RESEED, 4);
DBCC CHECKIDENT ('Authors',    RESEED, 5);
DBCC CHECKIDENT ('Books',      RESEED, 5);

-- 7. Comprobación
SELECT * FROM Categories;
SELECT * FROM Authors;
SELECT * FROM Books;
SELECT * FROM AuthorBook;
