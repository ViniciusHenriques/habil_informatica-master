
/****** Object:  StoredProcedure [dbo].[SP_Gera_Atendimento_Documento]    Script Date: 17/09/2020 16:24:16 ******/
DROP PROCEDURE [dbo].[SP_Gera_Atendimento_Documento]
GO

/****** Object:  StoredProcedure [dbo].[SP_Gera_Atendimento_Documento]    Script Date: 17/09/2020 16:24:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SP_Gera_Atendimento_Documento]
  @CD_DOCUMENTO as numeric(18,0)

AS

BEGIN

--TRANSACTION BEGIN
  Declare @CD_INDEX_AUX as NUMERIC (18,0)
  Declare @CD_INDEX_EST as NUMERIC (18,0)
  Declare @CD_Empresa as int  
  Declare @CD_Produto as int 
  Declare @QtSolicitada as numeric(18,3)
  Declare @QtAtendida as numeric(18,3)
  DECLARE @QT_DISPONIVEL as numeric(18,3)
  DECLARE @CD_PROD_PRODUTO AS INT 
  Declare @CD_Localizacao as int
  Declare @CD_Lote as int

Set NoCount ON

/* Busca Informacoes necessarias do Produto do Documento */
Select ROW_NUMBER() OVER(ORDER BY PD.CD_DOCUMENTO ASC) AS ID, PD.CD_DOCUMENTO, CD_EMPRESA, CD_PROD_DOCUMENTO, CD_PRODUTO, QT_SOLICITADA 
INTO #TMPPRODDOC
FROM PRODUTO_DO_DOCUMENTO AS PD
   INNER JOIN DOCUMENTO AS D
     ON D.CD_DOCUMENTO = PD.CD_DOCUMENTO
WHERE PD.CD_DOCUMENTO = @CD_DOCUMENTO

DELETE FROM ATENDIMENTO_DO_DOCUMENTO WHERE CD_DOCUMENTO = @CD_DOCUMENTO

Declare @TmpAtdtable table(CD_INDEX_CHAVE numeric(18,0))

  Select TOP 0 CD_INDEX AS ID, CD_EMPRESA, CD_PRODUTO, CD_INDEX_LOCALIZACAO , CD_LOTE, QT_DISPONIVEL   
  INTO #TMPESTOQUE
  from VW_Estoque 
  Where CD_EMPRESA = @CD_EMPRESA 
  And CD_PRODUTO = @CD_PRODUTO
  AND QT_DISPONIVEL > 0

/* Atribui Atendimentos */
Insert Into @TmpAtdtable 
  Select ID from #TMPPRODDOC Order By ID

While (select count(*) from @TmpAtdtable) > 0
Begin
  Select Top 1 @CD_INDEX_AUX = T.CD_INDEX_CHAVE From @TmpAtdtable T

  SET @CD_EMPRESA       = (SELECT CD_EMPRESA FROM #TMPPRODDOC WHERE ID = @CD_INDEX_AUX)
  SET @CD_PROD_PRODUTO  = (SELECT CD_PROD_DOCUMENTO  FROM #TMPPRODDOC WHERE ID = @CD_INDEX_AUX)
  SET @CD_PRODUTO       = (SELECT CD_PRODUTO FROM #TMPPRODDOC WHERE ID = @CD_INDEX_AUX)
  SET @QtSolicitada     = (SELECT QT_SOLICITADA FROM #TMPPRODDOC WHERE ID = @CD_INDEX_AUX)
  Set @QtAtendida      = 0
  
  INSERT INTO #TMPESTOQUE
  Select CD_INDEX AS ID, CD_EMPRESA, CD_PRODUTO, CD_INDEX_LOCALIZACAO , CD_LOTE, QT_DISPONIVEL   
  from VW_Estoque 
  Where CD_EMPRESA = @CD_EMPRESA 
  And CD_PRODUTO = @CD_PRODUTO
  AND QT_DISPONIVEL > 0

  Declare @TmpAtdtableE table(CD_INDEX_CHAVE numeric(18,0))

  Insert Into @TmpAtdtableE 
    Select ID from #TMPESTOQUE Order By ID

  While (select count(*) from @TmpAtdtableE) > 0
  Begin

    Select Top 1 @CD_INDEX_EST = T.CD_INDEX_CHAVE From @TmpAtdtableE T

    if @QtSolicitada > 0
	begin 
      SET @CD_LOCALIZACAO   = (SELECT CD_INDEX_LOCALIZACAO FROM #TMPESTOQUE WHERE ID = @CD_INDEX_EST)
      SET @CD_LOTE          = (SELECT CD_LOTE        FROM #TMPESTOQUE WHERE ID = @CD_INDEX_EST)
      SET @QT_DISPONIVEL    = (SELECT QT_DISPONIVEL FROM #TMPESTOQUE WHERE ID = @CD_INDEX_EST)

      if @QT_DISPONIVEL >= @QtSolicitada 
	  Begin
  	    Set @QtAtendida = @QtSolicitada
	    Set @QtSolicitada = @QtSolicitada - @QtAtendida
	  end
	  else begin
	    Set @QtAtendida = @QT_DISPONIVEL
	    Set @QtSolicitada = @QtSolicitada - @QT_DISPONIVEL
	  end

	  insert into ATENDIMENTO_DO_DOCUMENTO values (@CD_DOCUMENTO, @CD_Empresa, @CD_Produto, @CD_PROD_PRODUTO, @CD_LOCALIZACAO, @CD_LOTE, 0,  @QtAtendida)
    end
    Delete From @TmpAtdtableE Where CD_INDEX_CHAVE = @CD_INDEX_EST
    Delete From #TMPESTOQUE Where ID = @CD_INDEX_EST

  End
  Delete From @TmpAtdtable Where CD_INDEX_CHAVE = @CD_INDEX_AUX
  Delete From #TMPPRODDOC Where ID = @CD_INDEX_AUX

End

--select 'Atende - Executado com sucesso!' as MsgRetorno, 0 as CodRetorno
return(0)



END



GO


