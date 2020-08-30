/*
 * S0004 - Add suggestao in table solicitacaoProduto
 */

ALTER TABLE SolicitacaoProduto
    ADD COLUMN Sugestao VARCHAR(1000) NULL;