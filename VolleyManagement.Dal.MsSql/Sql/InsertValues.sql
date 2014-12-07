﻿USE master;
GO

USE VolleyManagement;
GO

/*************************************************
  Inserting data to database
*************************************************/

INSERT INTO dbo.Tournament(
[Name],
[Scheme],
[Season],
[Description]
)
VALUES
('Первый чемпионат любительской лиги', '1', '2012/2013', 'Любительская лига'),
('Второй чемпионат любительской лиги', '1', '2013/2014', 'Любительская лига'),
('Третий чемпионат любительской лиги', '2', '2014/2015', 'Любительская лига'),
('Четвертый чемпионат любительской лиги', '3', '2014/2015', 'Любительская лига'),
('Пятый чемпионат любительской лиги', '1', '2014/2015', 'Любительская лига');
GO