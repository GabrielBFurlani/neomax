/*
 * S0002 - Add codigoUsuario in table Cliente
 */
 
ALTER TABLE Cliente ADD COLUMN codigoUsuario INTEGER NOT NULL;

ALTER TABLE Cliente
ADD CONSTRAINT FK_Cliente_Usuario FOREIGN KEY (CodigoUsuario) REFERENCES Usuario(codigo); 

ALTER TABLE solicitacaoproduto
    ALTER COLUMN codigoproduto DROP NOT NULL;

ALTER TABLE solicitacaoproduto
    ADD COLUMN nomeproduto character varying(100) NOT NULL;

ALTER TABLE solicitacaoproduto
    ADD COLUMN CNPJFontePagadora character varying(14) NULL;
    