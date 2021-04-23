using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.Persistence;
using DAL.Model;

namespace DAL.TecnoSpeed.Persistence
{
    public class TECNO_NF_DAL : Conexao
    {
        protected string strSQL = "";

        public List<TECNO_NF> ListarTECNO_NF(string strID_NOTA_FISCAL)
        {
            try
            {
                AbrirConexao();
                string comando = "Select * from TECNO_NF Where ID_NOTA_FISCAL = @v1 ";

                Cmd = new SqlCommand(comando, Con);

                Cmd.Parameters.AddWithValue("@v1", strID_NOTA_FISCAL);

                Dr = Cmd.ExecuteReader();
                List<TECNO_NF> tcn_nf = new List<TECNO_NF>();

                while (Dr.Read())
                {
                    TECNO_NF obj = new TECNO_NF();
                    obj.ID_NOTA_FISCAL = Convert.ToDecimal(Dr["ID_NOTA_FISCAL"]);
                    obj.IDE_CUF = Convert.ToDecimal(Dr["IDE_CUF"]);
                    obj.ISSQNTOT_VSERV = Convert.ToDecimal(Dr["ISSQNTOT_VSERV"]);
                    obj.IDE_CNF = Convert.ToDecimal(Dr["IDE_CNF"]);
                    obj.ISSQNTOT_VBC = Convert.ToDecimal(Dr["ISSQNTOT_VBC"]);
                    obj.IDE_NATOP = Convert.ToString(Dr["IDE_NATOP"]);
                    obj.ISSQNTOT_VISS = Convert.ToDecimal(Dr["ISSQNTOT_VISS"]);
                    obj.IDE_INDPAG = Convert.ToDecimal(Dr["IDE_INDPAG"]);
                    obj.ISSQNTOT_VPIS = Convert.ToDecimal(Dr["ISSQNTOT_VPIS"]);
                    obj.IDE_MOD = Convert.ToDecimal(Dr["IDE_MOD"]);
                    obj.ISSQNTOT_VCOFINS = Convert.ToDecimal(Dr["ISSQNTOT_VCOFINS"]);
                    obj.IDE_SERIE = Convert.ToDecimal(Dr["IDE_SERIE"]);
                    obj.IDE_NNF = Convert.ToDecimal(Dr["IDE_NNF"]);
                    obj.IDE_DEMI = Convert.ToDateTime(Dr["IDE_DEMI"]);
                    obj.IDE_DSAIENT = Convert.ToDateTime(Dr["IDE_DSAIENT"]);
                    obj.IDE_TPNF = Convert.ToString(Dr["IDE_TPNF"]);
                    obj.IDE_CMUNFG = Convert.ToDecimal(Dr["IDE_CMUNFG"]);
                    obj.IDE_FINNFE = Convert.ToString(Dr["IDE_FINNFE"]);
                    obj.EMIT_CNPJ = Convert.ToString(Dr["EMIT_CNPJ"]);
                    obj.EMIT_IEST = Convert.ToString(Dr["EMIT_IEST"]);
                    obj.EMIT_IM = Convert.ToString(Dr["EMIT_IM"]);
                    obj.EMIT_CNAE = Convert.ToString(Dr["EMIT_CNAE"]);
                    obj.DEST_CNPJ_CPF = Convert.ToString(Dr["DEST_CNPJ_CPF"]);
                    obj.DEST_XNOME = Convert.ToString(Dr["DEST_XNOME"]);
                    obj.ENDERDEST_XLGR = Convert.ToString(Dr["ENDERDEST_XLGR"]);
                    obj.ENDERDEST_NRO = Convert.ToString(Dr["ENDERDEST_NRO"]);
                    obj.ENDERDEST_XCPL = Convert.ToString(Dr["ENDERDEST_XCPL"]);
                    obj.ENDERDEST_XBAIRRO = Convert.ToString(Dr["ENDERDEST_XBAIRRO"]);
                    obj.ENDERDEST_CMUN = Convert.ToDecimal(Dr["ENDERDEST_CMUN"]);
                    obj.ENDERDEST_XMUN = Convert.ToString(Dr["ENDERDEST_XMUN"]);
                    obj.ENDERDEST_UF = Convert.ToString(Dr["ENDERDEST_UF"]);
                    obj.ENDERDEST_CEP = Convert.ToDecimal(Dr["ENDERDEST_CEP"]);
                    obj.ENDERDEST_CPAIS = Convert.ToString(Dr["ENDERDEST_CPAIS"]);
                    obj.ENDERDEST_XPAIS = Convert.ToString(Dr["ENDERDEST_XPAIS"]);
                    obj.ENDERDEST_FONE = Convert.ToDecimal(Dr["ENDERDEST_FONE"]);
                    obj.DEST_IE = Convert.ToString(Dr["DEST_IE"]);
                    obj.DEST_ISUF = Convert.ToString(Dr["DEST_ISUF"]);
                    obj.ICMSTOT_VBC = Convert.ToDecimal(Dr["ICMSTOT_VBC"]);
                    obj.ICMSTOT_VICMS = Convert.ToDecimal(Dr["ICMSTOT_VICMS"]);
                    obj.ICMSTOT_VBCST = Convert.ToDecimal(Dr["ICMSTOT_VBCST"]);
                    obj.ICMSTOT_VST = Convert.ToDecimal(Dr["ICMSTOT_VST"]);
                    obj.ICMSTOT_VPROD = Convert.ToDecimal(Dr["ICMSTOT_VPROD"]);
                    obj.ICMSTOT_VFRETE = Convert.ToDecimal(Dr["ICMSTOT_VFRETE"]);
                    obj.ICMSTOT_VSEG = Convert.ToDecimal(Dr["ICMSTOT_VSEG"]);
                    obj.ICMSTOT_VDESC = Convert.ToDecimal(Dr["ICMSTOT_VDESC"]);
                    obj.ICMSTOT_VII = Convert.ToDecimal(Dr["ICMSTOT_VII"]);
                    obj.ICMSTOT_VIPI = Convert.ToDecimal(Dr["ICMSTOT_VIPI"]);
                    obj.ICMSTOT_VPIS = Convert.ToDecimal(Dr["ICMSTOT_VPIS"]);
                    obj.ICMSTOT_VCOFINS = Convert.ToDecimal(Dr["ICMSTOT_VCOFINS"]);
                    obj.ICMSTOT_VOUTRO = Convert.ToDecimal(Dr["ICMSTOT_VOUTRO"]);
                    obj.ICMSTOT_VNF = Convert.ToDecimal(Dr["ICMSTOT_VNF"]);
                    obj.RETTRIB_VRETPIS = Convert.ToDecimal(Dr["RETTRIB_VRETPIS"]);
                    obj.RETTRIB_VRETCOFINS = Convert.ToDecimal(Dr["RETTRIB_VRETCOFINS"]);
                    obj.RETTRIB_VRETCSLL = Convert.ToDecimal(Dr["RETTRIB_VRETCSLL"]);
                    obj.RETTRIB_VBCIRRF = Convert.ToDecimal(Dr["RETTRIB_VBCIRRF"]);
                    obj.RETTRIB_VIRRF = Convert.ToDecimal(Dr["RETTRIB_VIRRF"]);
                    obj.RETTRIB_VBCRETPREV = Convert.ToDecimal(Dr["RETTRIB_VBCRETPREV"]);
                    obj.RETTRIB_VRETPREV = Convert.ToDecimal(Dr["RETTRIB_VRETPREV"]);
                    obj.TRANSP_MODFRETE = Convert.ToDecimal(Dr["TRANSP_MODFRETE"]);
                    obj.FAT_NFAT = Convert.ToString(Dr["FAT_NFAT"]);
                    obj.FAT_VORIG = Convert.ToDecimal(Dr["FAT_VORIG"]);
                    obj.FAT_VDESC = Convert.ToDecimal(Dr["FAT_VDESC"]);
                    obj.FAT_VLIQ = Convert.ToDecimal(Dr["FAT_VLIQ"]);
                    obj.INFADIC_INFADFISCO = Convert.ToString(Dr["INFADIC_INFADFISCO"]);
                    obj.EXPORTA_UFEMBARQ = Convert.ToString(Dr["EXPORTA_UFEMBARQ"]);
                    obj.EXPORTA_XLOCEMBARQ = Convert.ToString(Dr["EXPORTA_XLOCEMBARQ"]);
                    obj.COMPRA_XNEMP = Convert.ToString(Dr["COMPRA_XNEMP"]);
                    obj.COMPRA_XPED = Convert.ToString(Dr["COMPRA_XPED"]);
                    obj.COMPRA_XCONT = Convert.ToString(Dr["COMPRA_XCONT"]);
                    obj.INFADIC_INFCPL = Convert.ToString(Dr["INFADIC_INFCPL"]);
                    obj.NOME_RECEBEDOR = Convert.ToString(Dr["NOME_RECEBEDOR"]);
                    obj.OBS_IMPRIMIR_NO_CORPO = Convert.ToString(Dr["OBS_IMPRIMIR_NO_CORPO"]);
                    obj.IDE_DHCONT = Convert.ToDateTime(Dr["IDE_DHCONT"]);
                    obj.IDE_XJUST = Convert.ToString(Dr["IDE_XJUST"]);
                    obj.DEST_EMAIL = Convert.ToString(Dr["DEST_EMAIL"]);
                    obj.TRANSP_VAGAO = Convert.ToString(Dr["TRANSP_VAGAO"]);
                    obj.TRANSP_BALSA = Convert.ToString(Dr["TRANSP_BALSA"]);
                    obj.gera_ideDsaient_automatico = Convert.ToDecimal(Dr["gera_ideDsaient_automatico"]);
                    obj.ide_hcont = Convert.ToString(Dr["ide_hcont"]);
                    obj.VTOTTRIB = Convert.ToDecimal(Dr["VTOTTRIB"]);
                    obj.ISSQNTOT_DCOMPET_OLD = Convert.ToDecimal(Dr["ISSQNTOT_DCOMPET_OLD"]);
                    obj.ISSQNTOT_VDEDUCAO = Convert.ToDecimal(Dr["ISSQNTOT_VDEDUCAO"]);
                    obj.ISSQNTOT_VOUTRO = Convert.ToDecimal(Dr["ISSQNTOT_VOUTRO"]);
                    obj.ISSQNTOT_VDESCINCOND = Convert.ToDecimal(Dr["ISSQNTOT_VDESCINCOND"]);
                    obj.ISSQNTOT_VDESCCOND = Convert.ToDecimal(Dr["ISSQNTOT_VDESCCOND"]);
                    obj.ISSQNTOT_VISSRET = Convert.ToDecimal(Dr["ISSQNTOT_VISSRET"]);
                    obj.ISSQNTOT_CREGTRIB = Convert.ToDecimal(Dr["ISSQNTOT_CREGTRIB"]);
                    obj.EXPORTA_UFSAIDAPAIS = Convert.ToString(Dr["EXPORTA_UFSAIDAPAIS"]);
                    obj.EXPORTA_XLOCEXPORTA = Convert.ToString(Dr["EXPORTA_XLOCEXPORTA"]);
                    obj.EXPORTA_XLOCDESPACHO = Convert.ToString(Dr["EXPORTA_XLOCDESPACHO"]);
                    obj.DEST_INDIE = Convert.ToDecimal(Dr["DEST_INDIE"]);
                    obj.IDE_DEST = Convert.ToDecimal(Dr["IDE_DEST"]);
                    obj.IDE_INDFINAL = Convert.ToDecimal(Dr["IDE_INDFINAL"]);
                    obj.IDE_INDPRES = Convert.ToDecimal(Dr["IDE_INDPRES"]);
                    obj.DEST_IDESTRANG = Convert.ToString(Dr["DEST_IDESTRANG"]);
                    obj.IDE_HEMI = Convert.ToString(Dr["IDE_HEMI"]);
                    obj.ICMStot_VICMSDESON = Convert.ToDecimal(Dr["ICMStot_VICMSDESON"]);
                    obj.DEST_IM = Convert.ToString(Dr["DEST_IM"]);
                    obj.IDE_HSAIENT = Convert.ToString(Dr["IDE_HSAIENT"]);
                    obj.ICMSTOT_VICMSUFDEST = Convert.ToDecimal(Dr["ICMSTOT_VICMSUFDEST"]);
                    obj.ICMSTOT_VICMSUFREMET = Convert.ToDecimal(Dr["ICMSTOT_VICMSUFREMET"]);
                    obj.ICMSTOT_VFCPUFDEST = Convert.ToDecimal(Dr["ICMSTOT_VFCPUFDEST"]);
                    obj.ICMSTOT_VFCP = Convert.ToDecimal(Dr["ICMSTOT_VFCP"]);
                    obj.ICMSTOT_VFCPST = Convert.ToDecimal(Dr["ICMSTOT_VFCPST"]);
                    obj.ICMSTOT_VFCPSTRET = Convert.ToDecimal(Dr["ICMSTOT_VFCPSTRET"]);
                    obj.ICMSTOT_VIPIDEVOL = Convert.ToDecimal(Dr["ICMSTOT_VIPIDEVOL"]);
                    obj.VTROCO = Convert.ToDecimal(Dr["VTROCO"]);
                    obj.ISSQNTOT_DCOMPET = Convert.ToDateTime(Dr["ISSQNTOT_DCOMPET"]);
                    obj.IDE_INDINTERMED = Convert.ToDecimal(Dr["IDE_INDINTERMED"]);
                    obj.INFINTERMED_CNPJ = Convert.ToString(Dr["INFINTERMED_CNPJ"]);
                    obj.INFINTERMED_IDCADINTTRAN = Convert.ToString(Dr["INFINTERMED_IDCADINTTRAN"]);

                    tcn_nf.Add(obj);
                }


                return tcn_nf;

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