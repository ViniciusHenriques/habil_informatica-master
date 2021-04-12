using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Persistence;
using DAL.Model;
namespace DAL.Persistence
{
    public class Habil_Impostos
    {

        /// <summary>
        ///     Método que retorna valor do ICMS
        /// </summary>
        /// <param name="decBaseCalculo"></param>
        /// <param name="decAliquotaICMS"></param>
        /// <returns></returns>
        public static decimal CalcularICMS(decimal decBaseCalculo, decimal decAliquotaICMS, RegFisIcms regICMS)
        {
            decimal decValorICMS_ST = 0;
            
            if (regICMS.CodHabil_RegTributario != 0)
            {
                if (regICMS.CodHabil_RegTributario == 3)  //REGIME NORMAL
                {
                    switch (regICMS.CodCST_CSOSN)
                    {
                        case 00:
                            decValorICMS_ST = decBaseCalculo * (regICMS.CST00ICMS / 100);
                            break;
                        case 10:    
                            if(regICMS.CST10MVASaida > 0)
                                decValorICMS_ST = (decBaseCalculo * (1 + regICMS.CST10MVASaida / 100)) * (1 - (regICMS.CST10ReducaoBCICMSST / 100)) * (regICMS.CST10ICMSProprio / 100);
                            else
                                decValorICMS_ST = (decBaseCalculo) * (1 - (regICMS.CST10ReducaoBCICMSST / 100)) * (regICMS.CST10ICMSProprio / 100);
                            break;
                        case 20:
                            decValorICMS_ST = (decBaseCalculo * (1 - (regICMS.CST20ReducaoBCICMS / 100)) * (regICMS.CST20ICMS / 100));
                            break;
                        case 30:
                            if (regICMS.CST10MVASaida > 0)
                                decValorICMS_ST = (decBaseCalculo * (1 + regICMS.CST10MVASaida / 100)) * (1 - (regICMS.CST10ReducaoBCICMSST / 100)) * (regICMS.CST10ICMSProprio / 100);
                            else
                                decValorICMS_ST = (decBaseCalculo) * (1 - (regICMS.CST10ReducaoBCICMSST / 100)) * (regICMS.CST10ICMSProprio / 100);
                            break;
                        case 40:
                            decValorICMS_ST = decBaseCalculo * (regICMS.CST404150MotDesoneracao / 100);
                            break;
                        case 41:
                            decValorICMS_ST = decBaseCalculo * (regICMS.CST404150MotDesoneracao / 100);
                            break;
                        case 50:
                            decValorICMS_ST = decBaseCalculo * (regICMS.CST404150MotDesoneracao / 100);
                            break;
                        case 51:
                            decValorICMS_ST = decBaseCalculo * (regICMS.CST51ICMS / 100);
                            break;
                        case 70:
                            if (regICMS.CST70MVASaida > 0)
                                decValorICMS_ST = (decBaseCalculo * (1 + regICMS.CST70MVASaida / 100)) * (1 - (regICMS.CST70ReducaoBCICMSST / 100)) * (regICMS.CST70ICMSProprio / 100);
                            else
                                decValorICMS_ST = (decBaseCalculo) * (1 - (regICMS.CST70ReducaoBCICMSST / 100)) * (regICMS.CST70ICMSProprio / 100);
                            break;
                        case 90:
                            decValorICMS_ST = (decBaseCalculo * (1 - (regICMS.CST90ReducaoBCICMSST / 100)) * (regICMS.CST90ICMS / 100));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (regICMS.CodCST_CSOSN)
                    {
                        case 101:
                            decValorICMS_ST = decBaseCalculo * (regICMS.CSOSN101_ICMS_SIMPLES / 100);
                            break;
                        case 201:
                            if (regICMS.CSOSN201_MVASaida > 0)
                                decValorICMS_ST = (decBaseCalculo * (1 + regICMS.CSOSN201_MVASaida / 100)) * (1 - (regICMS.CSOSN201_ReducaoBCICMSST / 100)) * (regICMS.CSOSN201_ICMS_SIMPLES / 100);
                            else
                                decValorICMS_ST = (decBaseCalculo) * (1 - (regICMS.CSOSN201_ReducaoBCICMSST / 100)) * (regICMS.CSOSN201_ICMS_SIMPLES / 100);
                            break;
                        case 202:
                            if (regICMS.CSOSN202_203_MVASaida > 0)
                                decValorICMS_ST = (decBaseCalculo * (1 + regICMS.CSOSN202_203_MVASaida / 100)) * (1 - (regICMS.CSOSN202_203_ReducaoBCICMSST / 100)) * (regICMS.CSOSN202_203_ICMS / 100);
                            else
                                decValorICMS_ST = (decBaseCalculo) * (1 - (regICMS.CSOSN202_203_ReducaoBCICMSST / 100)) * (regICMS.CSOSN202_203_ICMS / 100);
                            break;
                        case 203:
                            if (regICMS.CSOSN202_203_MVASaida > 0)
                                decValorICMS_ST = (decBaseCalculo * (1 + regICMS.CSOSN202_203_MVASaida / 100)) * (1 - (regICMS.CSOSN202_203_ReducaoBCICMSST / 100)) * (regICMS.CSOSN202_203_ICMS / 100);
                            else
                                decValorICMS_ST = (decBaseCalculo) * (1 - (regICMS.CSOSN202_203_ReducaoBCICMSST / 100)) * (regICMS.CSOSN202_203_ICMS / 100);
                            break;
                        case 900:
                            decValorICMS_ST = (decBaseCalculo * (1 - (regICMS.CSOSN900_ReducaoBCICMSST / 100)) * (regICMS.CSOSN900_ICMS / 100));
                            break;
                        default:
                            break;
                    }
                }
            }
            return decValorICMS_ST;

        }

        /// <summary>
        ///     Método que retorna valor do IPI
        /// </summary>
        /// <param name="decBaseCalculo"></param>
        /// <param name="decAliquotaIPI"></param>
        /// <returns></returns>
        public static decimal CalcularIPI(decimal decBaseCalculo, decimal decAliquotaIPI)
        {
            return decBaseCalculo * (decAliquotaIPI / 100);
        }

        /// <summary>
        ///  Método que retorna valor do PIS
        /// </summary>
        /// <param name="decBaseCalculo"></param>
        /// <param name="decAliquotaPIS"></param>
        /// <returns></returns>
        public static decimal CalcularPIS(decimal decBaseCalculo, decimal decAliquotaPIS)
        {
            return decBaseCalculo * (decAliquotaPIS / 100);
        }

        /// <summary>
        ///  Método que retorna valor do COFINS
        /// </summary>
        /// <param name="decBaseCalculo"></param>
        /// <param name="decAliquotaCOFINS"></param>
        /// <returns></returns>
        public static decimal CalcularCOFINS(decimal decBaseCalculo, decimal decAliquotaCOFINS)
        {

            return decBaseCalculo * (decAliquotaCOFINS / 100);
        }
    }
}
