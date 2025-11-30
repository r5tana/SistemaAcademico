
use SIGA


select top(1)* from dbo.tmeinfobancos
-- Esta ejecución dura aproximadamente 5min por la cantidad (605242) de registros en la tabla
 --ALTER TABLE [dbo].tmeinfobancos]
 --ADD IdBanco INT identity(1,1) primary key NOT NULL;

 select top(1)* from dbo.tmxusuarios
--ALTER TABLE [dbo].[tmxusuarios]
--ADD PasswordWeb nvarchar (50) NULL;

-- Contraseña temporal de usuarios: 2025
-- UPDATE [dbo].[tmxusuarios] SET PasswordWeb = 'MgAwADIANQA=';


select * from dbo.tmecajas
--ALTER TABLE dbo.tmecajas
--ADD Serie nvarchar(10) NULL 

select * from dbo.tmxcontador
--ALTER TABLE dbo.tmxcontador
--ADD TipoContador int NOT NULL default (1);

--insert into dbo.tmxcontador ([tabla_nombre],[tabla_contador],[TipoContador])
--     values ('A', 1, 2);

--insert into dbo.tmxcontador ([tabla_nombre],[tabla_contador],[TipoContador])
--    values ('B', 1, 2);

