namespace DAL.Model
{
    public class DispositivoMovelDoUsuario
    {
        public int CD_INDEX { get; set; }
        public int CD_USUARIO { get; set; }
        public int CD_SITUACAO { get; set; }
        public string TX_KEY { get; set; }
        public string NR_FONE { get; set; }
        public string ID_DISPOSITIVO { get; set; }
        public string NM_DISPOSITIVO { get; set; }
        public string NM_MODELO { get; set; }
        public string NM_FABRICANTE { get; set; }
    }
}
