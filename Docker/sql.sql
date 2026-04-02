-- Active: 1768478254505@@127.0.0.1@3306
-- Database: postgres

-- DROP DATABASE IF EXISTS postgres;



CREATE TABLE cliente (
	id int GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	nome varchar(30),
	cpf text,
	conta int,
	isvip bool
	
);
CREATE TABLE  funcionario(
	id int GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	nome varchar(30),
	cpf text,
    senha_hash text,
	isadmin bool,
	quantidade_atestado int,
	nascimento int
	
	
);
CREATE TABLE produto(
	id int GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	nome varchar(30),
	codigo int,
	quantidade int,
	valor_revenda numeric,
	lote int 
);
INSERT INTO  funcionario (nome, cpf, senha_hash, isadmin, quantidade_atestado, nascimento)  VALUES ('padrao','12345678911','$2a$04$/ca.DjsxDIDKJijtonI7G.wpzKp2BgGpnW6hPMMtlI2Y.avHtyGzW',true,0,2000);
