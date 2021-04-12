
/****** Object:  StoredProcedure [dbo].[SP_Atualiza_Estoque_Documento]    Script Date: 17/09/2020 16:25:29 ******/
DROP PROCEDURE [dbo].[SP_Atualiza_Estoque_Documento]
GO

/****** Object:  StoredProcedure [dbo].[SP_Atualiza_Estoque_Documento]    Script Date: 17/09/2020 16:25:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SP_Atualiza_Estoque_Documento]
  @CD_DOCUMENTO as NUMERIC (18,0)

AS

BEGIN

TRANSACTION BEGIN

Declare @DT_LANCAMENTO as datetime 
Declare @Empresa as int  
Declare @Produto as int 
Declare @Localizacao as int
Declare @Lote as int
Declare @CD_INDEX_AUX as NUMERIC (18,0)
Declare @CD_INDEX_MOV as Numeric (18,0)
Declare @Operacao AS NVARCHAR(1)
Declare @CdOperacao AS INT
Declare @QtAtendida as numeric(18,3)
declare @VlUnitario as numeric(18,2)
Declare @NrDocumento as nvarchar(100)
Declare @NrDocumento_Ant as nvarchar(100)
Declare @NrDocumento_Doc as nvarchar(100)
Declare @SerDocumento_Doc as nvarchar(100)
Declare @CdMaquina as INT
Declare @CdUsuario as INT
Declare @InMovEstoque as smallint


Set NoCount ON

/* nao estraga o que tá pronto, se nao vou comer teu gabo*/


/* Busca Informacoes necessarias do Documento */
Select D.DT_HR_EMISSAO, D.NR_DOCUMENTO, D.NR_SR_DOCUMENTO, D.DG_DOCUMENTO, D.DG_SR_DOCUMENTO,D.CD_TIPO_OPERACAO ,D.CD_TIPO_DOCUMENTO 
INTO #TMPATDOC
FROM DOCUMENTO AS D
WHERE D.CD_DOCUMENTO = @CD_DOCUMENTO

/* Atribui */
SET @DT_LANCAMENTO = (SELECT DT_HR_EMISSAO FROM #TMPATDOC)
SET @NrDocumento_Doc = Isnull(Convert(varchar,(SELECT  ISNULL (NR_DOCUMENTO, 0) FROM #TMPATDOC) ),'0')
SET @SerDocumento_Doc = Isnull(Convert(varchar,(SELECT  ISNULL (NR_SR_DOCUMENTO, 0) FROM #TMPATDOC)),'0')
Set @CdOperacao = Isnull((Select CD_TIPO_OPERACAO From #TMPATDOC), 0)
Set @Operacao = Isnull((Select CASE CD_TP_MOVIMENTACAO WHEN 60 THEN 'S' WHEN 61 THEN 'E' WHEN 62 THEN 'A' ELSE '' END From TIPO_DE_OPERACAO Where CD_TIPO_OPERACAO = @CdOperacao ),'')
set @InMovEstoque = Isnull((Select IN_MOV_ESTOQUE From TIPO_DE_OPERACAO Where CD_TIPO_OPERACAO = @CdOperacao ), 0)
Set @NrDocumento_Ant = Isnull((Select CASE CD_TIPO_DOCUMENTO WHEN 1 THEN 'NFS' WHEN 2 THEN 'OS' WHEN 3 THEN 'CR' WHEN 4 THEN 'PG' WHEN 5 THEN 'ORC' WHEN 6 THEN 'SOA' WHEN 7 THEN 'CT' WHEN 8 THEN 'PED' ELSE '' END  From #TMPATDOC),'')
/* Vê se atualiza estoque */
if @InMovEstoque = 0 
begin
	select 'Documento Sem permissão de Atualizar Estoque!' as MsgRetorno, 1 as CodRetorno
	RollBack transaction
	return(1)
end

/* Exclui Duplicacao */
IF @CD_DOCUMENTO > 0
  DELETE FROM MOVIMENTACAO_DE_ESTOQUE WHERE CD_DOCUMENTO = @CD_DOCUMENTO AND CD_TIPO_OPERACAO = @CdOperacao 

IF @NrDocumento_Ant <> ''
  Set @NrDocumento = @NrDocumento_Ant
else
  Set @NrDocumento = ''

/* Monta Numero do Documento */
if @NrDocumento_Doc <> '0'
begin
  if @SerDocumento_Doc <> '0'
    Set @NrDocumento =  @NrDocumento + '/' + @SerDocumento_Doc + '/' + @NrDocumento_Doc   
  else
    Set @NrDocumento =  @NrDocumento + '/' + @NrDocumento_Doc


end
else begin
  SET @NrDocumento_Doc = (SELECT  ISNULL (DG_DOCUMENTO, '') FROM #TMPATDOC)
  SET @SerDocumento_Doc = (SELECT  ISNULL (NR_SR_DOCUMENTO, '') FROM #TMPATDOC)
  if @NrDocumento_Doc <> ''
  begin
    if @SerDocumento_Doc <> ''
      Set @NrDocumento =  @NrDocumento + '/' + @SerDocumento_Doc + '/' + @NrDocumento_Doc   
    else
      Set @NrDocumento =  @NrDocumento + '/' + @NrDocumento_Doc
  end
end

DROP TABLE #TMPATDOC

/* pega a maquina e usuario */
Select TOP 1 CD_MAQUINA, CD_USUARIO
INTO #TMPEV
FROM EVENTO_DO_DOCUMENTO AS ED
WHERE ED.CD_DOCUMENTO = @CD_DOCUMENTO
ORDER BY ED.CD_EVENTO DESC

/* Atribui */
Set @CdMaquina = Isnull((Select CD_MAQUINA From #TMPEV), 0)
Set @CdUsuario = Isnull((Select CD_USUARIO From #TMPEV), 0)
/* drop tmp */
DROP TABLE #TMPEV
/* Monta Atendimentos */
SELECT CD_INDEX INTO #TMPATDOC3 FROM ATENDIMENTO_DO_DOCUMENTO WHERE CD_DOCUMENTO = @CD_DOCUMENTO

Declare @TmpAtdtable table(CD_INDEX_CHAVE numeric(18,0))

/* Atribui Atendimentos */
Insert Into @TmpAtdtable 
  Select CD_INDEX from #TMPATDOC3 Order By CD_INDEX

While (select count(*) from @TmpAtdtable) > 0
Begin

  Select Top 1 @CD_INDEX_AUX = T.CD_INDEX_CHAVE From @TmpAtdtable T

  /* Dados necessarios atendimento */
  Select A.CD_EMPRESA, A.CD_PRODUTO, A.CD_LOCALIZACAO, A.CD_LOTE, ISNULL(A.QT_ATENDIDA, 0) AS QT_ATENDIDA, ISNULL(P.VL_ITEM, 0) AS VL_ITEM 
  INTO #TMPATDOC2
  FROM ATENDIMENTO_DO_DOCUMENTO AS A
    INNER JOIN PRODUTO_DO_DOCUMENTO AS P
      ON P.CD_DOCUMENTO = A.CD_DOCUMENTO
	  AND P.CD_PROD_DOCUMENTO= A.CD_PROD_DOCUMENTO
  WHERE A.CD_INDEX = @CD_INDEX_AUX
  
  /* atribui */
  SET @Empresa = ISNULL ((SELECT  ISNULL (CD_EMPRESA, 0) FROM #TMPATDOC2),0 )
  SET @Produto = ISNULL ((SELECT  ISNULL (CD_PRODUTO, 0) FROM #TMPATDOC2),0 )
  SET @Localizacao = ISNULL ((SELECT  ISNULL (CD_LOCALIZACAO, 0) FROM #TMPATDOC2),0 )
  SET @Lote = ISNULL ((SELECT  ISNULL (CD_LOTE, 0) FROM #TMPATDOC2),0 )
  SET @QtAtendida = ISNULL ((SELECT  ISNULL (QT_ATENDIDA, 0) FROM #TMPATDOC2),0 )
  SET @VlUnitario = ISNULL ((SELECT  ISNULL (VL_ITEM, 0) FROM #TMPATDOC2),0 )

  DROP TABLE #TMPATDOC2

  /* movimenta */
  INSERT INTO MOVIMENTACAO_DE_ESTOQUE (DT_LANCAMENTO, CD_EMPRESA, CD_LOCALIZACAO, CD_LOTE,
    CD_PRODUTO, CD_TIPO_OPERACAO, TP_OPER, CD_USUARIO, CD_MAQUINA, CD_DOCUMENTO, VL_AJUSTE,
    NR_DOCUMENTO, VL_UNITARIO, VL_SALDO_ANTERIOR, QT_MOVIMENTADA, VL_SALDO_FINAL)
  Values (@DT_LANCAMENTO, @Empresa, @Localizacao, @Lote, 
    @Produto, @CdOperacao, @Operacao, @CdUsuario, @CdMaquina, @CD_DOCUMENTO, 0,
	@NrDocumento, @VlUnitario, 0, @QtAtendida, 0)

  Set @CD_INDEX_MOV = (SELECT scope_identity())

  /* Calcula Saldo */
  EXEC [dbo].[SP_Atualiza_Estoque] @CD_INDEX_MOV, @DT_LANCAMENTO

ProximaMovimentacao: 
  /* Atualiza Atendimento */

  UPDATE ATENDIMENTO_DO_DOCUMENTO SET CD_INDEX_MOV_ESTOQUE = @CD_INDEX_MOV WHERE CD_INDEX = @CD_INDEX_AUX

  Delete From @TmpAtdtable Where CD_INDEX_CHAVE = @CD_INDEX_AUX
  Delete From #TMPATDOC3 Where CD_INDEX = @CD_INDEX_AUX

End

COMMIT 

select 'Executado com sucesso!' as MsgRetorno, 0 as CodRetorno
return(0)

END


GO


