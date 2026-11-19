namespace models;


    public class cliente
    {
        public int id { get; set; }
        public string nome{get;set;}
        public string empresa { get; set;}
        public string cpf{get;set;}
        public int conta{get;set;}
        public bool isvip{get;set;}
        public int data { get; set; }
    }


    public class Funcionario
    {
        public int id { get; set; }
        public string nome{get;set;}
        public string senha_hash { get; set; }
        public string cpf{get;set;}
        public bool isadmin{get;set;}
        public int quantidade_atestado{get;set;}
        public int nascimento{get;set;}
        public int data { get; set; }
    }

    
    public class Produto
    {
        public int id { get; set; }
        public string nome{get;set;}
        public int codigo{get;set;}
        public int quantidade{get;set;}
        public decimal valor_revenda{get;set;}
        public int lote{get;set;}
    }

