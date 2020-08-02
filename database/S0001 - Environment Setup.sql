/*
 * S0001 - Environment Setup 
 */
 
-- Apoios

CREATE TABLE Produto(
	Codigo BIGSERIAL PRIMARY KEY,
    Nome VARCHAR(200) NOT NULL,
	Descricao VARCHAR(1000) NOT NULL,
    Ativo BOOLEAN NOT NULL
);

GRANT ALL PRIVILEGES ON TABLE Produto TO neomaxuser;
ALTER TABLE Produto OWNER TO neomaxuser;

CREATE TABLE Arquivo (
    Codigo BIGSERIAL PRIMARY KEY,
	Nome VARCHAR(100) NOT NULL,
	DataCriacao TIMESTAMP NOT NULL,
	Conteudo BYTEA NOT NULL
);

GRANT ALL PRIVILEGES ON TABLE Arquivo TO neomaxuser;
ALTER TABLE Arquivo OWNER TO neomaxuser;

CREATE TABLE Telefone (
	Codigo BIGSERIAL PRIMARY KEY,
	Numero VARCHAR(20) NOT NULL,
	TipoTelefone SMALLINT NOT NULL,
	NomeContato VARCHAR(80) NULL
);

GRANT ALL PRIVILEGES ON TABLE Telefone TO neomaxuser;
ALTER TABLE Telefone OWNER TO neomaxuser;

-- Cliente

CREATE TABLE UsuarioDisponivel (
    Codigo BIGSERIAL PRIMARY KEY,
    Email VARCHAR(150) NULL,
    Token VARCHAR(250) NOT NULL
);

GRANT ALL PRIVILEGES ON TABLE UsuarioDisponivel TO neomaxuser;
ALTER TABLE UsuarioDisponivel OWNER TO neomaxuser;

CREATE TABLE Cliente (
    Codigo BIGSERIAL PRIMARY KEY,
	CNPJFontePagadora VARCHAR(18) NULL,
	Sexo INT NULL,
	CodigoFoto INTEGER NULL,
	TipoNotaEmitida INT NULL,
	FaturamentoAnual INT NULL,
	NaturezaEmpresa INT NULL,
	CONSTRAINT FK_Cliente_Foto FOREIGN KEY (CodigoFoto)
	REFERENCES Arquivo (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE Cliente TO neomaxuser;
ALTER TABLE Cliente OWNER TO neomaxuser;

CREATE TABLE ClienteTelefone (
    CodigoCliente INTEGER NOT NULL,
	CodigoTelefone INTEGER NOT NULL,
	PRIMARY KEY (CodigoCliente, CodigoTelefone),
	CONSTRAINT FK_ClienteTelefone_Cliente FOREIGN KEY (CodigoCliente)
	REFERENCES Cliente (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_ClienteTelefone_Telefone FOREIGN KEY (CodigoTelefone)
	REFERENCES Telefone (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE ClienteTelefone TO neomaxuser;
ALTER TABLE ClienteTelefone OWNER TO neomaxuser;

CREATE TABLE ClienteDocumento (
    CodigoCliente INTEGER NOT NULL,
	CodigoDocumento INTEGER NOT NULL,
	PRIMARY KEY (CodigoCliente, CodigoDocumento),
	CONSTRAINT FK_ClienteDocumento_Cliente FOREIGN KEY (CodigoCliente)
	REFERENCES Cliente (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_ClienteDocumento_Documento FOREIGN KEY (CodigoDocumento)
	REFERENCES Arquivo (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE ClienteDocumento TO neomaxuser;
ALTER TABLE ClienteDocumento OWNER TO neomaxuser;

CREATE TABLE ClienteDiaContato (
    CodigoCliente INTEGER NOT NULL,
	DiaContato INTEGER NOT NULL,
	CONSTRAINT FK_ClienteDiaContato_Cliente FOREIGN KEY (CodigoCliente)
	REFERENCES Cliente (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE ClienteDiaContato TO neomaxuser;
ALTER TABLE ClienteDiaContato OWNER TO neomaxuser;

CREATE TABLE ClienteHoraContato (
    CodigoCliente INTEGER NOT NULL,
	HorarioContato INTEGER NOT NULL,
	CONSTRAINT FK_ClienteHoraContato_Cliente FOREIGN KEY (CodigoCliente)
	REFERENCES Cliente (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE ClienteHoraContato TO neomaxuser;
ALTER TABLE ClienteHoraContato OWNER TO neomaxuser;

CREATE TABLE Banco (
    Codigo BIGSERIAL PRIMARY KEY,
    Banco VARCHAR(100) NOT NULL,
	Agencia VARCHAR(10) NOT NULL,
	Conta VARCHAR(10) NOT NULL
);

GRANT ALL PRIVILEGES ON TABLE Banco TO neomaxuser;
ALTER TABLE Banco OWNER TO neomaxuser;

CREATE TABLE ClienteBanco (
    CodigoCliente INT NOT NULL,
	CodigoBanco INT NOT NULL,
	CONSTRAINT FK_ClienteBanco_Cliente FOREIGN KEY (CodigoCliente)
	REFERENCES Cliente (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_ClienteBanco_Banco FOREIGN KEY (CodigoBanco)
	REFERENCES Banco (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE ClienteBanco TO neomaxuser;
ALTER TABLE ClienteBanco OWNER TO neomaxuser;
	
-- Cliente
CREATE TABLE Usuario (
    Codigo BIGSERIAL PRIMARY KEY,
	Nome VARCHAR(100) NOT NULL,
	NickName VARCHAR(50) NULL,
	Usuario VARCHAR(18) NOT NULL,
	Email VARCHAR(150),
	Senha VARCHAR(64) NOT NULL,
	TokenAcesso VARCHAR(250) NULL,
	TokenAcessoDataCriacao TIMESTAMP NULL,
	CodigoFoto INT NULL,
	CodigoCliente INT NULL,
	Ativo BOOLEAN NOT NULL,
	CONSTRAINT FK_Usuario_Foto FOREIGN KEY (CodigoFoto)
	REFERENCES Arquivo (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_Usuario_Cliente FOREIGN KEY (CodigoCliente)
	REFERENCES Cliente (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE Usuario TO neomaxuser;
ALTER TABLE Usuario OWNER TO neomaxuser;

CREATE TABLE DefinicaoSenha (
	Codigo BIGSERIAL PRIMARY KEY,
	CodigoUsuario INT NOT NULL,
	Token VARCHAR(250) NOT NULL,
	DataCriacao TIMESTAMP NOT NULL,
	DataExpiracao TIMESTAMP NULL,
	CONSTRAINT FK_DefinicaoSenha_Usuario FOREIGN KEY (CodigoUsuario)
	REFERENCES Usuario (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE DefinicaoSenha TO neomaxuser;
ALTER TABLE DefinicaoSenha OWNER TO neomaxuser;

-- Solicitações

CREATE TABLE Solicitacao(
	Codigo BIGSERIAL PRIMARY KEY,
    Protocolo VARCHAR(15) NOT NULL,
	CodigoCliente INT NOT NULL,
	Status INT NOT NULL,
	DataCriacao TIMESTAMP NOT NULL,
	CONSTRAINT FK_Solicitacao_Cliente FOREIGN KEY (CodigoCliente)
	REFERENCES Cliente (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE Solicitacao TO neomaxuser;
ALTER TABLE Solicitacao OWNER TO neomaxuser;

CREATE TABLE SolicitacaoProduto(
	Codigo BIGSERIAL PRIMARY KEY,
    CodigoSolicitacao INT NOT NULL,
	CodigoProduto INT NOT NULL,
	Status INT NOT NULL,
	DataCriacao TIMESTAMP NOT NULL,
	Titulo VARCHAR(150),
	CONSTRAINT FK_SolicitacaoProduto_Solicitacao FOREIGN KEY (codigoSolicitacao)
	REFERENCES Solicitacao (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_SolicitacaoProduto_Produto FOREIGN KEY (codigoProduto)
	REFERENCES Produto (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE SolicitacaoProduto TO neomaxuser;
ALTER TABLE SolicitacaoProduto OWNER TO neomaxuser;

CREATE TABLE SolicitacaoProdutoDocumento (
    CodigoSolicitacaoProduto INTEGER NOT NULL,
	CodigoDocumento INTEGER NOT NULL,
	CONSTRAINT FK_SolicitacaoProdutoDocumento_SolicitacaoProduto FOREIGN KEY (CodigoSolicitacaoProduto)
	REFERENCES SolicitacaoProduto (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_SolicitacaoProdutoDocumento_Documento FOREIGN KEY (CodigoDocumento)
	REFERENCES Arquivo (Codigo) MATCH SIMPLE ON UPDATE NO ACTION ON DELETE NO ACTION
);

GRANT ALL PRIVILEGES ON TABLE SolicitacaoProdutoDocumento TO neomaxuser;
ALTER TABLE SolicitacaoProdutoDocumento OWNER TO neomaxuser;