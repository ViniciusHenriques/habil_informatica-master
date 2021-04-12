
/****** Object:  StoredProcedure [dbo].[SP_Atualiza_Estoque]    Script Date: 17/09/2020 16:26:07 ******/
DROP PROCEDURE [dbo].[SP_Atualiza_Estoque]
GO

/****** Object:  StoredProcedure [dbo].[SP_Atualiza_Estoque]    Script Date: 17/09/2020 16:26:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SP_Atualiza_Estoque]
  @CD_INDEX as NUMERIC (18,0),
  @DT_REFERENCIA as DATETIME

AS

BEGIN
Declare @SaldoAnterior as Numeric(18,3) 
Declare @Empresa as int  
Declare @Produto as int 
Declare @Localizacao as int
Declare @Lote as int
Declare @CD_INDEX_AUX as NUMERIC (18,0)
Declare @Operacao AS NVARCHAR(1)
Declare @QtdMovimentada as numeric(18,3)
Declare @QtdAjuste as numeric(18,3)
Declare @SaldoFinal as numeric(18,3)


Set NoCount ON

Select CD_EMPRESA, CD_PRODUTO, CD_LOCALIZACAO, CD_LOTE 
INTO #TMPMOVESTOQUE
FROM MOVIMENTACAO_DE_ESTOQUE
WHERE CD_INDEX = @CD_INDEX

SET @Empresa = ISNULL ((SELECT  ISNULL (CD_EMPRESA, 0) FROM #TMPMOVESTOQUE),0 )
SET @Produto = ISNULL ((SELECT  ISNULL (CD_PRODUTO, 0) FROM #TMPMOVESTOQUE),0 )
SET @Localizacao = ISNULL ((SELECT  ISNULL (CD_LOCALIZACAO, 0) FROM #TMPMOVESTOQUE),0 )
SET @Lote = ISNULL ((SELECT  ISNULL (CD_LOTE, 0) FROM #TMPMOVESTOQUE),0 )

If exists (select Top 1 1 from tempdb.dbo.sysobjects where id = object_id(N'tempdb.dbo.#TMPMOVESTOQUE'))
Begin
  DROP TABLE #TMPMOVESTOQUE
End

SELECT top 1 ISNULL(VL_SALDO_FINAL, 0) AS VL_SALDO_FINAL
INTO #TMPMOVESTOQUE2
FROM MOVIMENTACAO_DE_ESTOQUE
WHERE CD_EMPRESA = @Empresa
AND CD_PRODUTO = @Produto
AND CD_LOCALIZACAO = @Localizacao
AND CD_LOTE = @Lote
AND DT_LANCAMENTO < @DT_REFERENCIA
ORDER BY CD_INDEX DESC

SET @SaldoAnterior = ISNULL ((SELECT  ISNULL (VL_SALDO_FINAL, 0) FROM #TMPMOVESTOQUE2),0 )

If exists (select Top 1 1 from tempdb.dbo.sysobjects where id = object_id(N'tempdb.dbo.#TMPMOVESTOQUE2'))
Begin
  DROP TABLE #TMPMOVESTOQUE2
End

SELECT * 
INTO #TMPMOVESTOQUE3
FROM MOVIMENTACAO_DE_ESTOQUE
WHERE CD_EMPRESA = @Empresa
AND CD_PRODUTO = @Produto
AND CD_LOCALIZACAO = @Localizacao
AND CD_LOTE = @Lote
AND DT_LANCAMENTO >= @DT_REFERENCIA
ORDER BY CD_INDEX 

Declare @TmpMovEstChave table (CD_INDEX_CHAVE numeric(18,0))
Insert Into @TmpMovEstChave Select CD_INDEX from #TMPMOVESTOQUE3 Order By CD_INDEX

While (select count(*) from @TmpMovEstChave) > 0
Begin
  Select Top 1 @CD_INDEX_AUX = T.CD_INDEX_CHAVE From @TmpMovEstChave T

  Set @Operacao = Isnull((Select Top 1 TP_OPER From #TMPMOVESTOQUE3 Where CD_INDEX = @CD_INDEX_AUX),'')

  if(@Operacao = '')
  begin
    goto ProximaMovimentacao 
  end

  Set @QtdMovimentada = Isnull((Select Top 1 QT_MOVIMENTADA From #TMPMOVESTOQUE3 Where CD_INDEX = @CD_INDEX_AUX),0)

  if(@Operacao = 'S')
  begin
	set @SaldoFinal = @SaldoAnterior - @QtdMovimentada
  end
  
  if(@Operacao = 'E')
  begin
	set @SaldoFinal = @SaldoAnterior + @QtdMovimentada
  end

  if(@Operacao = 'A')
  begin
    Set @QtdAjuste = Isnull((Select Top 1 VL_AJUSTE From #TMPMOVESTOQUE3 Where CD_INDEX = @CD_INDEX_AUX),0)
    
	Set @SaldoFinal = @QtdAjuste
    Set @QtdMovimentada = ABS(@QtdAjuste - @SaldoAnterior)


    Update MOVIMENTACAO_DE_ESTOQUE 
    Set VL_SALDO_ANTERIOR = @SaldoAnterior, VL_SALDO_FINAL = @SaldoFinal, QT_MOVIMENTADA =  @QtdMovimentada
    Where CD_INDEX = @CD_INDEX_AUX 

  
  end
  else begin 
    Update MOVIMENTACAO_DE_ESTOQUE 
    Set VL_SALDO_ANTERIOR = @SaldoAnterior, VL_SALDO_FINAL = @SaldoFinal 
    Where CD_INDEX = @CD_INDEX_AUX 
  end

 SET @SaldoAnterior = @SaldoFinal


If Exists(Select 1 From ESTOQUE WITH (NOLOCK) 
Where CD_EMPRESA = @Empresa
And CD_PRODUTO = @Produto 
And CD_LOCALIZACAO = @Localizacao
And CD_LOTE = @Lote)
Begin
  Update ESTOQUE 
  Set QUANTIDADE = @SaldoFinal 
  Where CD_EMPRESA = @Empresa 
  And   CD_PRODUTO = @Produto 
  And CD_LOCALIZACAO = @Localizacao 
  And CD_LOTE = @Lote
End
Else Begin
  Insert Into Estoque (CD_PRODUTO,CD_EMPRESA,CD_LOCALIZACAO, CD_LOTE, QUANTIDADE, CD_SITUACAO)
  Values (@Produto,@Empresa,@Localizacao, @Lote, @SaldoFinal, 128)
End

ProximaMovimentacao: 

  Delete From @TmpMovEstChave Where CD_INDEX_CHAVE = @CD_INDEX_AUX
  Delete From #TMPMOVESTOQUE3 Where CD_INDEX = @CD_INDEX_AUX
End

END



GO


