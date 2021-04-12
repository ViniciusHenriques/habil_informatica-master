
/****** Object:  StoredProcedure [dbo].[SP_Separacao_Documento]    Script Date: 17/09/2020 16:22:30 ******/
DROP PROCEDURE [dbo].[SP_Separacao_Documento]
GO

/****** Object:  StoredProcedure [dbo].[SP_Separacao_Documento]    Script Date: 17/09/2020 16:22:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SP_Separacao_Documento]
  @CD_MAQUINA as int,
  @CD_DOCA as int,
  @lst_cd_documento as nvarchar(max)

AS

BEGIN


  select space(1000) as MsgRetorno, 1000 as CodRetorno into #TmpMensErro
  truncate table #TmpMensErro

  BEGIN TRANSACTION 

  Declare @CD_DOCUMENTO_AUX as NUMERIC(18,0)
  Declare @CD_SEPARACAO_AUX as INT
  DECLARE @return_status AS INT  
  
  Set NoCount ON



/*ATE AQUI DE BOAS*/
  insert into SEPARACAO (CD_MAQUINA, DS_CD_DOCUMENTO) values (@CD_MAQUINA, @lst_cd_documento)
  Set @CD_SEPARACAO_AUX = (SELECT scope_identity())

  DECLARE @STR NVARCHAR(max) = @lst_cd_documento
  SET @STR  = 'update DOCUMENTO set CD_SEPARACAO = ' + Convert(varchar, @CD_SEPARACAO_AUX) + ' where CD_DOCUMENTO in (' + @STR + ')'
  --SET @STR  = 'SELECT * FROM documento WHERE cd_documento in (' + @STR + ')'
  EXEC SP_EXECUTESQL @STR

  /*ATE AQUI DE BOAS*/
  select CD_DOCUMENTO, CD_SITUACAO, CD_SEPARACAO
  into #TmpPedAgrupados
  from DOCUMENTO 
  where CD_SEPARACAO = @CD_SEPARACAO_AUX 

  declare @TmpSeptable table(CD_INDEX_CHAVE numeric(18,0))

  /*ATE AQUI DE BOAS*/
  Insert Into @TmpSeptable 
  Select CD_DOCUMENTO from #TmpPedAgrupados Order By CD_DOCUMENTO

  While (select count(*) from @TmpSeptable) > 0
  begin
    SET @CD_DOCUMENTO_AUX = ISNULL ((select top 1 CD_DOCUMENTO from #TmpPedAgrupados WHERE CD_SEPARACAO = @CD_SEPARACAO_AUX ),0 )

    EXEC  @return_status = [dbo].[SP_Gera_Atendimento_Documento] @CD_DOCUMENTO_AUX
    /*ATE AQUI DE BOAS*/
    --SELECT  @return_status
      if @return_status = 0
      begin
	    if @CD_DOCA <> 0
		begin
  	      update DOCUMENTO set CD_SITUACAO = 155, CD_DOCA = @CD_DOCA where CD_DOCUMENTO = @CD_DOCUMENTO_AUX 
	    end
		else begin
		 update DOCUMENTO set CD_SITUACAO = 155 where CD_DOCUMENTO = @CD_DOCUMENTO_AUX 
		end

		update EVENTO_DO_DOCUMENTO set CD_SITUACAO = 157 where CD_DOCUMENTO = @CD_DOCUMENTO_AUX AND CD_SITUACAO = 154
      end
      else begin 
        rollback transaction
        insert into #TmpMensErro 
  	    select 'Erro ao Atender Documento: ' + convert(varchar,@CD_DOCUMENTO_AUX) as MsgRetorno, 10 as CodRetorno

        Select top 1 * from #TmpMensErro order by codretorno desc

  	    return(10)

		goto Sair
      end
     Delete From @TmpSeptable Where CD_INDEX_CHAVE = @CD_DOCUMENTO_AUX
     Delete From #TmpPedAgrupados Where CD_DOCUMENTO = @CD_DOCUMENTO_AUX

    END
  commit 
  insert into #TmpMensErro
  select 'Executado com sucesso!' as MsgRetorno, 0 as CodRetorno

  Select top 1 * from #TmpMensErro order by codretorno desc
  return(0)

Sair:

end
GO


