using System;
using System.Text.RegularExpressions;


namespace DAL.Model
{
    public class clsValidacao 
    {
        public void CampoValido(string strCampo, string strValor, Boolean Campo_Obrigatorio, Boolean Campo_Numero, Boolean Somente_Inteiro, Boolean Permite_Negativo, string  Campo_NO_SQL, ref Boolean blnCampoValido, ref string strRetorno)
        {
            blnCampoValido = true;
            strRetorno = "";


//            strValor = Regex.Replace(strValor, "[@&'(\\s)<>#]", "");

            if ((strValor.Contains("'") || (strValor.Contains("|"))))
            {
                strRetorno = strCampo + " contém caracteres inválidos.";
                blnCampoValido = false;
            }


            if ((Campo_NO_SQL == "INT") || (Campo_NO_SQL == "SMALLINT") || (Campo_NO_SQL == "NUMERIC"))
            {
                Campo_Numero = true;
                Somente_Inteiro = true;
                Permite_Negativo = false;
            }
            if ((Campo_NO_SQL == "NVARCHAR") || (Campo_NO_SQL == "VARCHAR") || (Campo_NO_SQL == "CHAR"))
            {
                Campo_Numero = false;
                Somente_Inteiro = false;
                Permite_Negativo = false;
            }
            if (Campo_Obrigatorio)
            {
                if (strValor.Trim() == "")
                {
                    strRetorno = strCampo + " preenchimento é obrigatório.";
                    blnCampoValido = false;
                }
            }

            if ((strValor.Trim() != "") && (Campo_Numero))
            {
                double s = 0;
                if (!double.TryParse(strValor, out s))
                {
                    strRetorno = strCampo + " deve ser numérico.";
                    blnCampoValido = false;
                }
            }

            if ((strValor.Trim() != "") && ((Somente_Inteiro) && (Campo_Numero)) && (strRetorno == ""))
            {

                decimal s1 = 0;
                if (decimal.TryParse(strValor, out s1))
                {
                    if ((s1 % 1) != 0)
                    {
                        strRetorno = strCampo + " não permite decimais.";
                        blnCampoValido = false;

                    }
                    
                }
                Int64 numero;
                if (!Int64.TryParse(strValor, out numero))
                {
                    strRetorno = strCampo + " não permite decimais.";
                    blnCampoValido = false;
                }
            }
            if ((strValor.Trim() != "") && (((!Permite_Negativo) && strRetorno == "") && (Campo_Numero)))
            {
                if ((Convert.ToDouble(strValor)) < 0)
                {
                    strRetorno = strCampo + " não permite negativo.";
                    blnCampoValido = false;
                }
            }

            if (((Campo_NO_SQL == "SMALLDATETIME") || (Campo_NO_SQL == "DATETIME") ) && (strValor.Trim() != "") )
            {
                bool validaData;

                try
                {
                    Convert.ToDateTime(strValor);
                    validaData = true;
                }
                catch (Exception)
                {
                    validaData = false;
                }

                if (!validaData)
                {
                    strRetorno = strCampo + " deve ser lançada uma Data Válida.";
                    blnCampoValido = false;
                }
            }
        }
        public void CampoDataValido(string strCampo, string strValor, Boolean Campo_Obrigatorio, ref Boolean blnCampoValido, ref string strRetorno)
        {
            blnCampoValido = true;
            strRetorno = "";

            if ((strValor.Contains("'") || (strValor.Contains("|"))))
            {
                strRetorno = strCampo + " formato de Data Inválido.";
                blnCampoValido = false;
            }


            if (Campo_Obrigatorio)
            {
                if (strValor.Trim() == "")
                {
                    strRetorno = strCampo + " preenchimento é obrigatório.";
                    blnCampoValido = false;
                }
            }

            if (strValor.Trim() != "")
            {

                DateTime resultado = DateTime.MinValue;
                if (DateTime.TryParse(strValor.Trim(), out resultado))
                {
                    //     Response.Write("Data Válida.");
                }
                else
                {
                    strRetorno = strCampo + " formato de Data Inválido.";
                    blnCampoValido = false;
                }
            }
        }
        public bool ValidaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
        public bool ValidaCNPJ(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
        public DateTime ObterDataHora()
        {
            DateTime ddtt = DateTime.Now;



            return ddtt;
        }
        public string RetornaDataPorExtenso(DateTime Data)
        {
            string[] array = DateTime.Now.ToString("dd/MM/yyyy").Split('/');
            int intMes = Convert.ToInt32(array[1]);
            if (intMes == 1)
                return array[0] + " de Janeiro de " + array[2];
            else if (intMes == 2)
                return array[0] + " de Fevereiro de " + array[2];
            else if (intMes == 3)
                return array[0] + " de Março de " + array[2];
            else if (intMes == 4)
                return array[0] + " de Abril de " + array[2];
            else if (intMes == 5)
                return array[0] + " de Maio de " + array[2];
            else if (intMes == 6)
                return array[0] + " de Junho de " + array[2];
            else if (intMes == 7)
                return array[0] + " de Julho de " + array[2];
            else if (intMes == 8)
                return array[0] + " de Agosto de " + array[2];
            else if (intMes == 9)
                return array[0] + " de Setembro de " + array[2];
            else if (intMes == 10)
                return array[0] + " de Outubro de " + array[2];
            else if (intMes == 11)
                return array[0] + " de Novembro de " + array[2];
            else if (intMes == 12)
                return array[0] + " de Dezembro de " + array[2];
            else
                return "";
        }
    }
}
