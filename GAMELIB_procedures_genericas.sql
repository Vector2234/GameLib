/* ==========================================
   GAMELIB - BANCO DE DADOS (SQL Server)
   Procedures genericas, escritas no estilo simples
   ========================================== */

USE master
GO

IF DB_ID('GAMELIB') IS NULL
    CREATE DATABASE GAMELIB
GO

USE GAMELIB
GO

/* ==========================================
   TABELAS
   ========================================== */
CREATE TABLE Usuario (
 Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
 NomeUsuario varchar(50) NOT NULL,
 Email varchar(50) NOT NULL,
 FotoPerfil varbinary(max)
)
GO
  
CREATE TABLE Jogo (
 Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
 Nome varchar(150) NOT NULL,
 Descricao varchar(max) NULL,
 DataLancamento date NULL,
 Capa varbinary(max)
 )
GO


CREATE TABLE Genero (
 Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
 Nome varchar(50) NOT NULL UNIQUE
)
GO

CREATE TABLE JogoGenero (
 JogoId int NOT NULL,
 GeneroId int NOT NULL,
 PRIMARY KEY (JogoId, GeneroId),
 FOREIGN KEY (JogoId) REFERENCES Jogo(Id) ON DELETE CASCADE,
 FOREIGN KEY (GeneroId) REFERENCES Genero(Id) ON DELETE CASCADE
)
GO

CREATE TABLE Biblioteca (
 Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
 UsuarioId nvarchar(450) NOT NULL,
 JogoId int NOT NULL,
 Status varchar(20) NOT NULL DEFAULT 'NaLista',
 PorcentagemConclusao decimal(5,2) NOT NULL DEFAULT 0,
 HorasJogadas decimal(8,2) NOT NULL DEFAULT 0,
 DataInicio date NULL,
 DataFim date NULL,
 UNIQUE (UsuarioId, JogoId),
 FOREIGN KEY (JogoId) REFERENCES Jogo(Id),
 CHECK (Status IN ('Jogando','NaLista','Pausado','Abandonado','Zerado')),
 CHECK (PorcentagemConclusao BETWEEN 0 AND 100),
 CHECK (HorasJogadas >= 0),
 CHECK (DataInicio IS NULL OR DataFim IS NULL OR DataFim >= DataInicio),
 CHECK (Status <> 'Zerado' OR PorcentagemConclusao = 100)
)
GO

CREATE TABLE Conquista (
 Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
 JogoId int NOT NULL,
 Nome varchar(150) NOT NULL,
 Descricao varchar(max) NULL,
 FOREIGN KEY (JogoId) REFERENCES Jogo(Id)
)
GO

CREATE TABLE ConquistaUsuario (
 BibliotecaId int NOT NULL,
 ConquistaId int NOT NULL,
 DataObtencao date NULL,
 PRIMARY KEY (BibliotecaId, ConquistaId),
 FOREIGN KEY (BibliotecaId) REFERENCES Biblioteca(Id) ON DELETE CASCADE,
 FOREIGN KEY (ConquistaId) REFERENCES Conquista(Id) ON DELETE CASCADE
)
GO

/* ==========================================
   PROCEDURES GENERICAS
   ========================================== */

create procedure usp_Inserir
(
 @Tabela sysname,
 @Dados nvarchar(max)
)
as
begin
 set nocount on

 if @Tabela not in ('Jogo', 'Genero', 'JogoGenero', 'Biblioteca', 'Conquista', 'ConquistaUsuario')
  throw 50001, 'Tabela nao permitida.', 1;

 if ISJSON(@Dados) <> 1
  throw 50002, 'JSON invalido.', 1;

 declare @Colunas nvarchar(max)
 declare @Valores nvarchar(max)
 declare @SQL nvarchar(max)

 select
 @Colunas = string_agg(convert(nvarchar(max), quotename(C.name)), ','),
 @Valores = string_agg(convert(nvarchar(max),
  '(select [value] from openjson(@Dados) where [key] = N''' + replace(C.name, '''', '''''') + ''')'), ',')
 from sys.columns C
 inner join sys.tables T on T.object_id = C.object_id
 inner join sys.schemas S on S.schema_id = T.schema_id
 inner join openjson(@Dados) J on J.[key] collate database_default = C.name collate database_default
 where T.name = @Tabela
 and S.name = 'dbo'
 and C.is_identity = 0
 and C.is_computed = 0
 and C.system_type_id <> 189

 if @Colunas is null
  throw 50003, 'Nenhuma coluna valida encontrada.', 1;

 set @SQL = N'insert into dbo.' + quotename(@Tabela) + N' (' + @Colunas + N') select ' + @Valores + N';'

 exec sp_executesql @SQL, N'@Dados nvarchar(max)', @Dados = @Dados
end
GO

create procedure usp_Atualizar
(
 @Tabela sysname,
 @Dados nvarchar(max),
 @Chaves nvarchar(max)
)
as
begin
 set nocount on

 if @Tabela not in ('Jogo', 'Genero', 'JogoGenero', 'Biblioteca', 'Conquista', 'ConquistaUsuario')
  throw 50004, 'Tabela nao permitida.', 1;

 if ISJSON(@Dados) <> 1 or ISJSON(@Chaves) <> 1
  throw 50005, 'JSON invalido.', 1;

 declare @Set nvarchar(max)
 declare @Where nvarchar(max)
 declare @SQL nvarchar(max)

 select
 @Set = string_agg(convert(nvarchar(max),
  quotename(C.name) + N' = (select [value] from openjson(@Dados) where [key] = N''' + replace(C.name, '''', '''''') + N''')'), ',')
 from sys.columns C
 inner join sys.tables T on T.object_id = C.object_id
 inner join sys.schemas S on S.schema_id = T.schema_id
 inner join openjson(@Dados) J on J.[key] collate database_default = C.name collate database_default
 where T.name = @Tabela
 and S.name = 'dbo'
 and C.is_identity = 0
 and C.is_computed = 0
 and C.system_type_id <> 189
 and not exists (
  select 1
  from sys.indexes I
  inner join sys.index_columns IC on IC.object_id = I.object_id and IC.index_id = I.index_id
  where I.object_id = C.object_id
  and I.is_primary_key = 1
  and IC.column_id = C.column_id
 )

 select
 @Where = string_agg(convert(nvarchar(max),
  quotename(C.name) + N' = (select [value] from openjson(@Chaves) where [key] = N''' + replace(C.name, '''', '''''') + N''')'), ' and ')
 from sys.columns C
 inner join sys.tables T on T.object_id = C.object_id
 inner join sys.schemas S on S.schema_id = T.schema_id
 inner join sys.indexes I on I.object_id = T.object_id and I.is_primary_key = 1
 inner join sys.index_columns IC on IC.object_id = I.object_id and IC.index_id = I.index_id and IC.column_id = C.column_id
 inner join openjson(@Chaves) J on J.[key] collate database_default = C.name collate database_default
 where T.name = @Tabela
 and S.name = 'dbo'

 if @Set is null or @Where is null
  throw 50006, 'Dados ou chaves insuficientes.', 1;

 set @SQL = N'update dbo.' + quotename(@Tabela) + N' set ' + @Set + N' where ' + @Where + N';'

 exec sp_executesql @SQL, N'@Dados nvarchar(max), @Chaves nvarchar(max)', @Dados = @Dados, @Chaves = @Chaves
end
GO

create procedure usp_Excluir
(
 @Tabela sysname,
 @Chaves nvarchar(max)
)
as
begin
 set nocount on

 if @Tabela not in ('Jogo', 'Genero', 'JogoGenero', 'Biblioteca', 'Conquista', 'ConquistaUsuario')
  throw 50007, 'Tabela nao permitida.', 1;

 if ISJSON(@Chaves) <> 1
  throw 50008, 'JSON invalido.', 1;

 declare @Where nvarchar(max)
 declare @SQL nvarchar(max)

 select
 @Where = string_agg(convert(nvarchar(max),
  quotename(C.name) + N' = (select [value] from openjson(@Chaves) where [key] = N''' + replace(C.name, '''', '''''') + N''')'), ' and ')
 from sys.columns C
 inner join sys.tables T on T.object_id = C.object_id
 inner join sys.schemas S on S.schema_id = T.schema_id
 inner join sys.indexes I on I.object_id = T.object_id and I.is_primary_key = 1
 inner join sys.index_columns IC on IC.object_id = I.object_id and IC.index_id = I.index_id and IC.column_id = C.column_id
 inner join openjson(@Chaves) J on J.[key] collate database_default = C.name collate database_default
 where T.name = @Tabela
 and S.name = 'dbo'

 if @Where is null
  throw 50009, 'Chaves primarias nao informadas.', 1;

 set @SQL = N'delete from dbo.' + quotename(@Tabela) + N' where ' + @Where + N';'

 exec sp_executesql @SQL, N'@Chaves nvarchar(max)', @Chaves = @Chaves
end
GO

create procedure usp_ConsultarPorChave
(
 @Tabela sysname,
 @Chaves nvarchar(max)
)
as
begin
 set nocount on

 if @Tabela not in ('Jogo', 'Genero', 'JogoGenero', 'Biblioteca', 'Conquista', 'ConquistaUsuario')
  throw 50010, 'Tabela nao permitida.', 1;

 if ISJSON(@Chaves) <> 1
  throw 50011, 'JSON invalido.', 1;

 declare @Where nvarchar(max)
 declare @SQL nvarchar(max)

 select
 @Where = string_agg(convert(nvarchar(max),
  quotename(C.name) + N' = (select [value] from openjson(@Chaves) where [key] = N''' + replace(C.name, '''', '''''') + N''')'), ' and ')
 from sys.columns C
 inner join sys.tables T on T.object_id = C.object_id
 inner join sys.schemas S on S.schema_id = T.schema_id
 inner join sys.indexes I on I.object_id = T.object_id and I.is_primary_key = 1
 inner join sys.index_columns IC on IC.object_id = I.object_id and IC.index_id = I.index_id and IC.column_id = C.column_id
 inner join openjson(@Chaves) J on J.[key] collate database_default = C.name collate database_default
 where T.name = @Tabela
 and S.name = 'dbo'

 if @Where is null
  throw 50012, 'Chaves primarias nao informadas.', 1;

 set @SQL = N'select * from dbo.' + quotename(@Tabela) + N' where ' + @Where + N';'

 exec sp_executesql @SQL, N'@Chaves nvarchar(max)', @Chaves = @Chaves
end
GO

create procedure usp_Listar
(
 @Tabela sysname
)
as
begin
 set nocount on

 if @Tabela not in ('Jogo', 'Genero', 'JogoGenero', 'Biblioteca', 'Conquista', 'ConquistaUsuario')
  throw 50013, 'Tabela nao permitida.', 1;

 declare @SQL nvarchar(max)

 set @SQL = N'select * from dbo.' + quotename(@Tabela) + N';'

 exec sp_executesql @SQL
end
GO
