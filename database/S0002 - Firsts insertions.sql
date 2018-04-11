/*
 * S0002 - Firsts insertions
 * Description: Creates the basic profiles, permissions first users
 */

/**************************** DATABASE: Usage  ******************************/  
USE [PlataformaZ2] 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Table "Profile": new profiles
SET IDENTITY_INSERT [Profile] ON
INSERT INTO [dbo].[Profile] ([Id], [Name], [Description]) VALUES (1, 'Básico', 'Usuário Básico')
INSERT INTO [dbo].[Profile] ([Id], [Name], [Description]) VALUES (2, 'Admin', 'Usuário Administrador')
SET IDENTITY_INSERT [Profile] OFF
GO


--Table "User": 

	--First "admin" User:
		--user: plataformaz2@plataformaz2.com
		--pass: a123
	INSERT INTO [dbo].[User] ([Username], [Password], [Name], [Nickname], [Cpf], [IdProfile], [Active])
		VALUES ('plataformaz2@plataformaz2.com', '76af7efae0d034d1e3335ed1b90f24b6cadf2bf1', 'Administrador Geral (perfil admin)', 'Geral (admin)', 00000000000, 2, 1)
	GO


--Table "Permission": new permissions
SET IDENTITY_INSERT [Permission] ON
INSERT INTO [Permission] ([Id], [Name]) VALUES (1, 'Carregar Minha Sessão de Usuário');
INSERT INTO [Permission] ([Id], [Name]) VALUES (2, 'Editar Meus Dados');
INSERT INTO [Permission] ([Id], [Name]) VALUES (3, 'Alterar Minha Senha');
INSERT INTO [Permission] ([Id], [Name]) VALUES (4, 'Pesquisar/Visualizar Usuários');
INSERT INTO [Permission] ([Id], [Name]) VALUES (5, 'Cadastrar/Editar/Excluir Usuário');
INSERT INTO [Permission] ([Id], [Name]) VALUES (6, 'Gerenciar Todos os Usuários');
INSERT INTO [Permission] ([Id], [Name]) VALUES (7, 'Pesquisar/Visualizar Perfis de Usuário');
SET IDENTITY_INSERT [Permission] OFF
GO


--Table "ProfilePermission": new profile-permission relations

	--Profile "Básico" (IdProfile = 1)
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (1, 1);
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (1, 2);
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (1, 3);
	

	--Profile "Admin" (IdProfile = 2)
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (2, 1);
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (2, 2);
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (2, 3);
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (2, 4);
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (2, 5);
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (2, 6);
	INSERT INTO [ProfilePermission] ([IdProfile], [IdPermission]) VALUES (2, 7);
	

--Register the script as applied
INSERT INTO [dbo].[AppliedScript] ([ScriptNumber], [ScriptName]) VALUES (2, 'First insertions')

