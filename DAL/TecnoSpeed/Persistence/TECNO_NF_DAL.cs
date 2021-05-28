using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.Persistence;

namespace DAL.TecnoSpeed.Persistence
{
    public class TECNO_NF_DAL : Conexao
    {
        protected string strSQL = "";

        public TECNO_NF ListarTECNO_NF(decimal decID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF where ID_NOTA_FISCAL = @v1";

                Cmd = new SqlCommand(comando, Con);
                Cmd.Parameters.AddWithValue("@v1", decID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();

                TECNO_NF obj = null;
                if (Dr.Read())
                {
                    obj = new TECNO_NF();

                    if (Dr["ID_NOTA_FISCAL"] != DBNull.Value)
                        obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);

                    if (Dr["IDE_VERSAO"] != DBNull.Value)
                        obj.IDE_VERSAO = Convert.ToDecimal(Dr["IDE_VERSAO"]);
                    
                    if (Dr["IDE_CUF"] != DBNull.Value)
                        obj.IDE_CUF = Convert.ToDecimal(Dr["IDE_CUF"]);

                    if (Dr["ISSQNTOT_VSERV"] != DBNull.Value)
                        obj.ISSQNTOT_VSERV = Convert.ToDecimal(Dr["ISSQNTOT_VSERV"]);

                    if (Dr["IDE_CNF"] != DBNull.Value)
                        obj.IDE_CNF = Convert.ToDecimal(Dr["IDE_CNF"]);

                    if (Dr["ISSQNTOT_VBC"] != DBNull.Value)
                        obj.ISSQNTOT_VBC = Convert.ToDecimal(Dr["ISSQNTOT_VBC"]);

                    if (Dr["IDE_NATOP"] != DBNull.Value)
                        obj.IDE_NATOP = Convert.ToString(Dr["IDE_NATOP"]);

                    if (Dr["ISSQNTOT_VISS"] != DBNull.Value)
                        obj.ISSQNTOT_VISS = Convert.ToDecimal(Dr["ISSQNTOT_VISS"]);

                    if (Dr["IDE_INDPAG"] != DBNull.Value)
                        obj.IDE_INDPAG = Convert.ToDecimal(Dr["IDE_INDPAG"]);

                    if (Dr["ISSQNTOT_VPIS"] != DBNull.Value)
                        obj.ISSQNTOT_VPIS = Convert.ToDecimal(Dr["ISSQNTOT_VPIS"]);

                    if (Dr["IDE_MOD"] != DBNull.Value)
                        obj.IDE_MOD = Convert.ToDecimal(Dr["IDE_MOD"]);

                    if (Dr["ISSQNTOT_VCOFINS"] != DBNull.Value)
                        obj.ISSQNTOT_VCOFINS = Convert.ToDecimal(Dr["ISSQNTOT_VCOFINS"]);

                    if (Dr["IDE_SERIE"] != DBNull.Value)
                        obj.IDE_SERIE = Convert.ToDecimal(Dr["IDE_SERIE"]);

                    if (Dr["IDE_NNF"] != DBNull.Value)
                        obj.IDE_NNF = Convert.ToDecimal(Dr["IDE_NNF"]);

                    if (Dr["IDE_DEMI"] != DBNull.Value)
                        obj.IDE_DEMI = Convert.ToDateTime(Dr["IDE_DEMI"]);

                    if (Dr["IDE_DSAIENT"] != DBNull.Value)
                        obj.IDE_DSAIENT = Convert.ToDateTime(Dr["IDE_DSAIENT"]);

                    if (Dr["IDE_TPNF"] != DBNull.Value)
                        obj.IDE_TPNF = Convert.ToString(Dr["IDE_TPNF"]);

                    if (Dr["IDE_CMUNFG"] != DBNull.Value)
                        obj.IDE_CMUNFG = Convert.ToDecimal(Dr["IDE_CMUNFG"]);

                    if (Dr["IDE_FINNFE"] != DBNull.Value)
                        obj.IDE_FINNFE = Convert.ToString(Dr["IDE_FINNFE"]);

                    if (Dr["EMIT_CNPJ"] != DBNull.Value)
                        obj.EMIT_CNPJ = Convert.ToString(Dr["EMIT_CNPJ"]);

                    if (Dr["EMIT_IEST"] != DBNull.Value)
                        obj.EMIT_IEST = Convert.ToString(Dr["EMIT_IEST"]);

                    if (Dr["EMIT_IM"] != DBNull.Value)
                        obj.EMIT_IM = Convert.ToString(Dr["EMIT_IM"]);

                    if (Dr["EMIT_CNAE"] != DBNull.Value)
                        obj.EMIT_CNAE = Convert.ToString(Dr["EMIT_CNAE"]);

                    if (Dr["DEST_CNPJ_CPF"] != DBNull.Value)
                        obj.DEST_CNPJ_CPF = Convert.ToString(Dr["DEST_CNPJ_CPF"]);

                    if (Dr["DEST_XNOME"] != DBNull.Value)
                        obj.DEST_XNOME = Convert.ToString(Dr["DEST_XNOME"]);

                    if (Dr["ENDERDEST_XLGR"] != DBNull.Value)
                        obj.ENDERDEST_XLGR = Convert.ToString(Dr["ENDERDEST_XLGR"]);

                    if (Dr["ENDERDEST_NRO"] != DBNull.Value)
                        obj.ENDERDEST_NRO = Convert.ToString(Dr["ENDERDEST_NRO"]);

                    if (Dr["ENDERDEST_XCPL"] != DBNull.Value)
                        obj.ENDERDEST_XCPL = Convert.ToString(Dr["ENDERDEST_XCPL"]);

                    if (Dr["ENDERDEST_XBAIRRO"] != DBNull.Value)
                        obj.ENDERDEST_XBAIRRO = Convert.ToString(Dr["ENDERDEST_XBAIRRO"]);

                    if (Dr["ENDERDEST_CMUN"] != DBNull.Value)
                        obj.ENDERDEST_CMUN = Convert.ToDecimal(Dr["ENDERDEST_CMUN"]);

                    if (Dr["ENDERDEST_XMUN"] != DBNull.Value)
                        obj.ENDERDEST_XMUN = Convert.ToString(Dr["ENDERDEST_XMUN"]);

                    if (Dr["ENDERDEST_UF"] != DBNull.Value)
                        obj.ENDERDEST_UF = Convert.ToString(Dr["ENDERDEST_UF"]);

                    if (Dr["ENDERDEST_CEP"] != DBNull.Value)
                        obj.ENDERDEST_CEP = Convert.ToDecimal(Dr["ENDERDEST_CEP"]);

                    if (Dr["ENDERDEST_CPAIS"] != DBNull.Value)
                        obj.ENDERDEST_CPAIS = Convert.ToString(Dr["ENDERDEST_CPAIS"]);

                    if (Dr["ENDERDEST_XPAIS"] != DBNull.Value)
                        obj.ENDERDEST_XPAIS = Convert.ToString(Dr["ENDERDEST_XPAIS"]);

                    if (Dr["ENDERDEST_FONE"] != DBNull.Value)
                        obj.ENDERDEST_FONE = Convert.ToDecimal(Dr["ENDERDEST_FONE"]);

                    if (Dr["DEST_IE"] != DBNull.Value)
                        obj.DEST_IE = Convert.ToString(Dr["DEST_IE"]);

                    if (Dr["DEST_ISUF"] != DBNull.Value)
                        obj.DEST_ISUF = Convert.ToString(Dr["DEST_ISUF"]);

                    if (Dr["ICMSTOT_VBC"] != DBNull.Value)
                        obj.ICMSTOT_VBC = Convert.ToDecimal(Dr["ICMSTOT_VBC"]);

                    if (Dr["ICMSTOT_VICMS"] != DBNull.Value)
                        obj.ICMSTOT_VICMS = Convert.ToDecimal(Dr["ICMSTOT_VICMS"]);

                    if (Dr["ICMSTOT_VBCST"] != DBNull.Value)
                        obj.ICMSTOT_VBCST = Convert.ToDecimal(Dr["ICMSTOT_VBCST"]);

                    if (Dr["ICMSTOT_VST"] != DBNull.Value)
                        obj.ICMSTOT_VST = Convert.ToDecimal(Dr["ICMSTOT_VST"]);

                    if (Dr["ICMSTOT_VPROD"] != DBNull.Value)
                        obj.ICMSTOT_VPROD = Convert.ToDecimal(Dr["ICMSTOT_VPROD"]);

                    if (Dr["ICMSTOT_VFRETE"] != DBNull.Value)
                        obj.ICMSTOT_VFRETE = Convert.ToDecimal(Dr["ICMSTOT_VFRETE"]);

                    if (Dr["ICMSTOT_VSEG"] != DBNull.Value)
                        obj.ICMSTOT_VSEG = Convert.ToDecimal(Dr["ICMSTOT_VSEG"]);

                    if (Dr["ICMSTOT_VDESC"] != DBNull.Value)
                        obj.ICMSTOT_VDESC = Convert.ToDecimal(Dr["ICMSTOT_VDESC"]);

                    if (Dr["ICMSTOT_VII"] != DBNull.Value)
                        obj.ICMSTOT_VII = Convert.ToDecimal(Dr["ICMSTOT_VII"]);

                    if (Dr["ICMSTOT_VIPI"] != DBNull.Value)
                        obj.ICMSTOT_VIPI = Convert.ToDecimal(Dr["ICMSTOT_VIPI"]);

                    if (Dr["ICMSTOT_VPIS"] != DBNull.Value)
                        obj.ICMSTOT_VPIS = Convert.ToDecimal(Dr["ICMSTOT_VPIS"]);

                    if (Dr["ICMSTOT_VCOFINS"] != DBNull.Value)
                        obj.ICMSTOT_VCOFINS = Convert.ToDecimal(Dr["ICMSTOT_VCOFINS"]);

                    if (Dr["ICMSTOT_VOUTRO"] != DBNull.Value)
                        obj.ICMSTOT_VOUTRO = Convert.ToDecimal(Dr["ICMSTOT_VOUTRO"]);

                    if (Dr["ICMSTOT_VNF"] != DBNull.Value)
                        obj.ICMSTOT_VNF = Convert.ToDecimal(Dr["ICMSTOT_VNF"]);

                    if (Dr["RETTRIB_VRETPIS"] != DBNull.Value)
                        obj.RETTRIB_VRETPIS = Convert.ToDecimal(Dr["RETTRIB_VRETPIS"]);

                    if (Dr["RETTRIB_VRETCOFINS"] != DBNull.Value)
                        obj.RETTRIB_VRETCOFINS = Convert.ToDecimal(Dr["RETTRIB_VRETCOFINS"]);

                    if (Dr["RETTRIB_VRETCSLL"] != DBNull.Value)
                        obj.RETTRIB_VRETCSLL = Convert.ToDecimal(Dr["RETTRIB_VRETCSLL"]);

                    if (Dr["RETTRIB_VBCIRRF"] != DBNull.Value)
                        obj.RETTRIB_VBCIRRF = Convert.ToDecimal(Dr["RETTRIB_VBCIRRF"]);

                    if (Dr["RETTRIB_VIRRF"] != DBNull.Value)
                        obj.RETTRIB_VIRRF = Convert.ToDecimal(Dr["RETTRIB_VIRRF"]);

                    if (Dr["RETTRIB_VBCRETPREV"] != DBNull.Value)
                        obj.RETTRIB_VBCRETPREV = Convert.ToDecimal(Dr["RETTRIB_VBCRETPREV"]);

                    if (Dr["RETTRIB_VRETPREV"] != DBNull.Value)
                        obj.RETTRIB_VRETPREV = Convert.ToDecimal(Dr["RETTRIB_VRETPREV"]);

                    if (Dr["TRANSP_MODFRETE"] != DBNull.Value)
                        obj.TRANSP_MODFRETE = Convert.ToDecimal(Dr["TRANSP_MODFRETE"]);

                    if (Dr["FAT_NFAT"] != DBNull.Value)
                        obj.FAT_NFAT = Convert.ToString(Dr["FAT_NFAT"]);

                    if (Dr["FAT_VORIG"] != DBNull.Value)
                        obj.FAT_VORIG = Convert.ToDecimal(Dr["FAT_VORIG"]);

                    if (Dr["FAT_VDESC"] != DBNull.Value)
                        obj.FAT_VDESC = Convert.ToDecimal(Dr["FAT_VDESC"]);

                    if (Dr["FAT_VLIQ"] != DBNull.Value)
                        obj.FAT_VLIQ = Convert.ToDecimal(Dr["FAT_VLIQ"]);

                    if (Dr["INFADIC_INFADFISCO"] != DBNull.Value)
                        obj.INFADIC_INFADFISCO = Convert.ToString(Dr["INFADIC_INFADFISCO"]);

                    if (Dr["EXPORTA_UFEMBARQ"] != DBNull.Value)
                        obj.EXPORTA_UFEMBARQ = Convert.ToString(Dr["EXPORTA_UFEMBARQ"]);

                    if (Dr["EXPORTA_XLOCEMBARQ"] != DBNull.Value)
                        obj.EXPORTA_XLOCEMBARQ = Convert.ToString(Dr["EXPORTA_XLOCEMBARQ"]);

                    if (Dr["COMPRA_XNEMP"] != DBNull.Value)
                        obj.COMPRA_XNEMP = Convert.ToString(Dr["COMPRA_XNEMP"]);

                    if (Dr["COMPRA_XPED"] != DBNull.Value)
                        obj.COMPRA_XPED = Convert.ToString(Dr["COMPRA_XPED"]);

                    if (Dr["COMPRA_XCONT"] != DBNull.Value)
                        obj.COMPRA_XCONT = Convert.ToString(Dr["COMPRA_XCONT"]);

                    if (Dr["INFADIC_INFCPL"] != DBNull.Value)
                        obj.INFADIC_INFCPL = Convert.ToString(Dr["INFADIC_INFCPL"]);

                    if (Dr["NOME_RECEBEDOR"] != DBNull.Value)
                        obj.NOME_RECEBEDOR = Convert.ToString(Dr["NOME_RECEBEDOR"]);

                    if (Dr["OBS_IMPRIMIR_NO_CORPO"] != DBNull.Value)
                        obj.OBS_IMPRIMIR_NO_CORPO = Convert.ToString(Dr["OBS_IMPRIMIR_NO_CORPO"]);

                    if (Dr["IDE_DHCONT"] != DBNull.Value)
                        obj.IDE_DHCONT = Convert.ToDateTime(Dr["IDE_DHCONT"]);

                    if (Dr["IDE_XJUST"] != DBNull.Value)
                        obj.IDE_XJUST = Convert.ToString(Dr["IDE_XJUST"]);

                    if (Dr["DEST_EMAIL"] != DBNull.Value)
                        obj.DEST_EMAIL = Convert.ToString(Dr["DEST_EMAIL"]);

                    if (Dr["TRANSP_VAGAO"] != DBNull.Value)
                        obj.TRANSP_VAGAO = Convert.ToString(Dr["TRANSP_VAGAO"]);

                    if (Dr["TRANSP_BALSA"] != DBNull.Value)
                        obj.TRANSP_BALSA = Convert.ToString(Dr["TRANSP_BALSA"]);

                    if (Dr["gera_ideDsaient_automatico"] != DBNull.Value)
                        obj.gera_ideDsaient_automatico = Convert.ToDecimal(Dr["gera_ideDsaient_automatico"]);

                    if (Dr["ide_hcont"] != DBNull.Value)
                        obj.ide_hcont = Convert.ToString(Dr["ide_hcont"]);

                    if (Dr["VTOTTRIB"] != DBNull.Value)
                        obj.VTOTTRIB = Convert.ToDecimal(Dr["VTOTTRIB"]);

                    if (Dr["ISSQNTOT_DCOMPET_OLD"] != DBNull.Value)
                        obj.ISSQNTOT_DCOMPET_OLD = Convert.ToDecimal(Dr["ISSQNTOT_DCOMPET_OLD"]);

                    if (Dr["ISSQNTOT_VDEDUCAO"] != DBNull.Value)
                        obj.ISSQNTOT_VDEDUCAO = Convert.ToDecimal(Dr["ISSQNTOT_VDEDUCAO"]);

                    if (Dr["ISSQNTOT_VOUTRO"] != DBNull.Value)
                        obj.ISSQNTOT_VOUTRO = Convert.ToDecimal(Dr["ISSQNTOT_VOUTRO"]);

                    if (Dr["ISSQNTOT_VDESCINCOND"] != DBNull.Value)
                        obj.ISSQNTOT_VDESCINCOND = Convert.ToDecimal(Dr["ISSQNTOT_VDESCINCOND"]);

                    if (Dr["ISSQNTOT_VDESCCOND"] != DBNull.Value)
                        obj.ISSQNTOT_VDESCCOND = Convert.ToDecimal(Dr["ISSQNTOT_VDESCCOND"]);

                    if (Dr["ISSQNTOT_VISSRET"] != DBNull.Value)
                        obj.ISSQNTOT_VISSRET = Convert.ToDecimal(Dr["ISSQNTOT_VISSRET"]);

                    if (Dr["ISSQNTOT_CREGTRIB"] != DBNull.Value)
                        obj.ISSQNTOT_CREGTRIB = Convert.ToDecimal(Dr["ISSQNTOT_CREGTRIB"]);

                    if (Dr["EXPORTA_UFSAIDAPAIS"] != DBNull.Value)
                        obj.EXPORTA_UFSAIDAPAIS = Convert.ToString(Dr["EXPORTA_UFSAIDAPAIS"]);

                    if (Dr["EXPORTA_XLOCEXPORTA"] != DBNull.Value)
                        obj.EXPORTA_XLOCEXPORTA = Convert.ToString(Dr["EXPORTA_XLOCEXPORTA"]);

                    if (Dr["EXPORTA_XLOCDESPACHO"] != DBNull.Value)
                        obj.EXPORTA_XLOCDESPACHO = Convert.ToString(Dr["EXPORTA_XLOCDESPACHO"]);

                    if (Dr["DEST_INDIE"] != DBNull.Value)
                        obj.DEST_INDIE = Convert.ToDecimal(Dr["DEST_INDIE"]);

                    if (Dr["IDE_DEST"] != DBNull.Value)
                        obj.IDE_DEST = Convert.ToDecimal(Dr["IDE_DEST"]);

                    if (Dr["IDE_INDFINAL"] != DBNull.Value)
                        obj.IDE_INDFINAL = Convert.ToDecimal(Dr["IDE_INDFINAL"]);

                    if (Dr["IDE_INDPRES"] != DBNull.Value)
                        obj.IDE_INDPRES = Convert.ToDecimal(Dr["IDE_INDPRES"]);

                    if (Dr["DEST_IDESTRANG"] != DBNull.Value)
                        obj.DEST_IDESTRANG = Convert.ToString(Dr["DEST_IDESTRANG"]);

                    if (Dr["IDE_HEMI"] != DBNull.Value)
                        obj.IDE_HEMI = Convert.ToString(Dr["IDE_HEMI"]);

                    if (Dr["ICMStot_VICMSDESON"] != DBNull.Value)
                        obj.ICMStot_VICMSDESON = Convert.ToDecimal(Convert.ToDecimal(Dr["ICMStot_VICMSDESON"]).ToString("F"));

                    if (Dr["DEST_IM"] != DBNull.Value)
                        obj.DEST_IM = Convert.ToString(Dr["DEST_IM"]);

                    if (Dr["IDE_HSAIENT"] != DBNull.Value)
                        obj.IDE_HSAIENT = Convert.ToString(Dr["IDE_HSAIENT"]);

                    if (Dr["ICMSTOT_VICMSUFDEST"] != DBNull.Value)
                        obj.ICMSTOT_VICMSUFDEST = Convert.ToDecimal(Dr["ICMSTOT_VICMSUFDEST"]);

                    if (Dr["ICMSTOT_VICMSUFREMET"] != DBNull.Value)
                        obj.ICMSTOT_VICMSUFREMET = Convert.ToDecimal(Dr["ICMSTOT_VICMSUFREMET"]);

                    if (Dr["ICMSTOT_VFCPUFDEST"] != DBNull.Value)
                        obj.ICMSTOT_VFCPUFDEST = Convert.ToDecimal(Dr["ICMSTOT_VFCPUFDEST"]);

                    if (Dr["ICMSTOT_VFCP"] != DBNull.Value)
                        obj.ICMSTOT_VFCP = Convert.ToDecimal(Dr["ICMSTOT_VFCP"]);

                    if (Dr["ICMSTOT_VFCPST"] != DBNull.Value)
                        obj.ICMSTOT_VFCPST = Convert.ToDecimal(Dr["ICMSTOT_VFCPST"]);

                    if (Dr["ICMSTOT_VFCPSTRET"] != DBNull.Value)
                        obj.ICMSTOT_VFCPSTRET = Convert.ToDecimal(Dr["ICMSTOT_VFCPSTRET"]);

                    if (Dr["ICMSTOT_VIPIDEVOL"] != DBNull.Value)
                        obj.ICMSTOT_VIPIDEVOL = Convert.ToDecimal(Dr["ICMSTOT_VIPIDEVOL"]);

                    if (Dr["VTROCO"] != DBNull.Value)
                        obj.VTROCO = Convert.ToDecimal(Dr["VTROCO"]);

                    if (Dr["ISSQNTOT_DCOMPET"] != DBNull.Value)
                        obj.ISSQNTOT_DCOMPET = Convert.ToDateTime(Dr["ISSQNTOT_DCOMPET"]);

                    if (Dr["IDE_INDINTERMED"] != DBNull.Value)
                        obj.IDE_INDINTERMED = Convert.ToDecimal(Dr["IDE_INDINTERMED"]);

                    if (Dr["INFINTERMED_CNPJ"] != DBNull.Value)
                        obj.INFINTERMED_CNPJ = Convert.ToString(Dr["INFINTERMED_CNPJ"]);

                    if (Dr["INFINTERMED_IDCADINTTRAN"] != DBNull.Value)
                        obj.INFINTERMED_IDCADINTTRAN = Convert.ToString(Dr["INFINTERMED_IDCADINTTRAN"]);

                    if (Dr["EMIT_IE"] != DBNull.Value)
                        obj.EMIT_IE = Convert.ToString(Dr["EMIT_IE"]);

                    if (Dr["EMIT_XNOME"] != DBNull.Value)
                        obj.EMIT_XNOME = Convert.ToString(Dr["EMIT_XNOME"]);

                    if (Dr["EMIT_XFANT"] != DBNull.Value)
                        obj.EMIT_XFANT = Convert.ToString(Dr["EMIT_XFANT"]);

                    if (Dr["ENDEREMIT_XLOG"] != DBNull.Value)
                        obj.ENDEREMIT_XLOG = Convert.ToString(Dr["ENDEREMIT_XLOG"]);

                    if (Dr["ENDEREMIT_NRO"] != DBNull.Value)
                        obj.ENDEREMIT_NRO = Convert.ToString(Dr["ENDEREMIT_NRO"]);

                    if (Dr["ENDEREMIT_XCPL"] != DBNull.Value)
                        obj.ENDEREMIT_XCPL = Convert.ToString(Dr["ENDEREMIT_XCPL"]);

                    if (Dr["ENDEREMIT_XBAIRRO"] != DBNull.Value)
                        obj.ENDEREMIT_XBAIRRO = Convert.ToString(Dr["ENDEREMIT_XBAIRRO"]);

                    if (Dr["ENDEREMIT_CMUN"] != DBNull.Value)
                        obj.ENDEREMIT_CMUN = Convert.ToDecimal(Dr["ENDEREMIT_CMUN"]);

                    if (Dr["ENDEREMIT_XMUN"] != DBNull.Value)
                        obj.ENDEREMIT_XMUN = Convert.ToString(Dr["ENDEREMIT_XMUN"]);

                    if (Dr["ENDEREMIT_UF"] != DBNull.Value)
                        obj.ENDEREMIT_UF = Convert.ToString(Dr["ENDEREMIT_UF"]);

                    if (Dr["ENDEREMIT_CEP"] != DBNull.Value)
                        obj.ENDEREMIT_CEP = Convert.ToDecimal(Dr["ENDEREMIT_CEP"]);

                    if (Dr["ENDEREMIT_CPAIS"] != DBNull.Value)
                        obj.ENDEREMIT_CPAIS = Convert.ToString(Dr["ENDEREMIT_CPAIS"]);

                    if (Dr["ENDEREMIT_XPAIS"] != DBNull.Value)
                        obj.ENDEREMIT_XPAIS = Convert.ToString(Dr["ENDEREMIT_XPAIS"]);

                    if (Dr["ENDEREMIT_FONE"] != DBNull.Value)
                        obj.ENDEREMIT_FONE = Convert.ToString(Dr["ENDEREMIT_FONE"]);

                    if (Dr["IDE_TPIMP"] != DBNull.Value)
                        obj.IDE_TPIMP = Convert.ToInt32(Dr["IDE_TPIMP"]);

                    if (Dr["IDE_TPEMIS"] != DBNull.Value)
                        obj.IDE_TPEMIS = Convert.ToInt32(Dr["IDE_TPEMIS"]);

                    if (Dr["IDE_PROCEMI"] != DBNull.Value)
                        obj.IDE_PROCEMI = Convert.ToInt32(Dr["IDE_PROCEMI"]);

                    if (Dr["IDE_CRT"] != DBNull.Value)
                        obj.IDE_CRT = Convert.ToInt32(Dr["IDE_CRT"]);

                    if (Dr["INFTEC_CNPJ"] != DBNull.Value)
                        obj.INFTEC_CNPJ = Convert.ToString(Dr["INFTEC_CNPJ"]);

                    if (Dr["INFTEC_XCONTATO"] != DBNull.Value)
                        obj.INFTEC_XCONTATO = Convert.ToString(Dr["INFTEC_XCONTATO"]);

                    if (Dr["INFTEC_EMAIL"] != DBNull.Value)
                        obj.INFTEC_EMAIL = Convert.ToString(Dr["INFTEC_EMAIL"]);

                    if (Dr["INFTEC_FONE"] != DBNull.Value)
                        obj.INFTEC_FONE = Convert.ToString(Dr["INFTEC_FONE"]);

                    if (Dr["INFTEC_IDCSRT"] != DBNull.Value)
                        obj.INFTEC_IDCSRT = Convert.ToString(Dr["INFTEC_IDCSRT"]);

                    if (Dr["INFTEC_HASHCSRT"] != DBNull.Value)
                        obj.INFTEC_HASHCSRT = Convert.ToString(Dr["INFTEC_HASHCSRT"]);

                }


                return obj;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar TECNO_NF: " + ex.Message.ToString());
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}