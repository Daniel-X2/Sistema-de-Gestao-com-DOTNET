-- Active: 1768478254505@@127.0.0.1@3306
-- Database: postgres

-- DROP DATABASE IF EXISTS postgres;



CREATE TABLE cliente (
	id int GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	nome varchar(30),
	cpf text,
	conta int,
	isvip bool,
	data int,
	empresa varchar(255)
);
CREATE TABLE  funcionario(
	id int GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	nome varchar(30),
	cpf text,
	isadmin bool,
    senha_hash text,
	quantidade_atestado int,
	nascimento int,
	data int
);
CREATE TABLE produto(
	id int GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	nome varchar(30),
	codigo int,
	quantidade int,
	valor_revenda numeric,
	lote int 
)
