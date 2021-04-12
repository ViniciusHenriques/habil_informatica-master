/****** Object:  View [dbo].[VW_LIS_ATENDIMENTO]    Script Date: 17/09/2020 16:20:10 ******/
DROP VIEW [dbo].[VW_LIS_ATENDIMENTO]
GO

/****** Object:  View [dbo].[VW_LIS_ATENDIMENTO]    Script Date: 17/09/2020 16:20:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VW_LIS_ATENDIMENTO]
AS
SELECT        doc.NR_DOCUMENTO, doc.CD_DOCUMENTO, doc.CD_TIPO_DOCUMENTO, doc.CD_EMPRESA, doc.CD_DOCA, doc.CD_SITUACAO, doc.DT_HR_EMISSAO, doc.CD_VENDEDOR, TPC.DS_TIPO_COBRANCA, 
                         ATDOC.CD_LOTE, ATDOC.QT_ATENDIDA, LOC.CD_LOCALIZACAO, SEP.ID AS CD_SEPARACAO, ISNULL(L.NR_LOTE + ' - ' + L.SR_LOTE + ' - F:' + CONVERT(VARCHAR, L.DT_FABRICACAO, 103) 
                         + ' - V:' + CONVERT(VARCHAR, L.DT_VALIDADE, 103), '') AS CPL_LOTE, PDD.QT_SOLICITADA, PDD.CD_PRODUTO, PROD.NM_PRODUTO, UN.SIGLA, M.DS_MARCA, pd.CD_PESSOA AS CD_CLIENTE, 
                         pd.CD_BAIRRO, pd.DS_BAIRRO, pd.RAZ_SOCIAL AS NM_CLIENTE, pd.NR_ENDERECO, pd.COMPLEMENTO, pd.LOGRADOURO, pd.CD_CEP, pd.TELEFONE_1, pd.INSCRICAO, MC.DS_MUNICIPIO, 
                         p.CD_TRANSPORTADOR, d.DS_DOCA, PV.NM_PESSOA AS NM_VENDEDOR, PV.CD_PESSOA, PST.RAZ_SOCIAL AS NM_TRANSPORTE, COUNT(PROD.CD_PRODUTO) AS QT_PROD_COUNT
FROM            dbo.DOCUMENTO AS doc INNER JOIN
                         dbo.PESSOA_DO_DOCUMENTO AS pd ON doc.CD_DOCUMENTO = pd.CD_DOCUMENTO AND pd.TP_PESSOA = 12 INNER JOIN
                         dbo.TIPO_DE_COBRANCA AS TPC ON TPC.CD_TIPO_COBRANCA = doc.CD_TIPO_COBRANCA INNER JOIN
                         dbo.ATENDIMENTO_DO_DOCUMENTO AS ATDOC ON doc.CD_DOCUMENTO = ATDOC.CD_DOCUMENTO INNER JOIN
                         dbo.LOCALIZACAO AS LOC ON ATDOC.CD_LOCALIZACAO = LOC.CD_INDEX INNER JOIN
                         dbo.SEPARACAO AS SEP ON doc.CD_SEPARACAO = SEP.ID INNER JOIN
                         dbo.LOTE AS L ON ATDOC.CD_LOTE = L.CD_INDEX AND ATDOC.CD_PRODUTO = L.CD_PRODUTO INNER JOIN
                         dbo.PESSOA AS p ON pd.CD_PESSOA = p.CD_PESSOA INNER JOIN
                         dbo.MUNICIPIO AS MC ON MC.CD_MUNICIPIO = pd.CD_MUNICIPIO INNER JOIN
                         dbo.DOCA AS d ON d.CD_DOCA = doc.CD_DOCA INNER JOIN
                         dbo.PRODUTO_DO_DOCUMENTO AS PDD ON doc.CD_DOCUMENTO = PDD.CD_DOCUMENTO AND PDD.CD_PROD_DOCUMENTO = ATDOC.CD_PROD_DOCUMENTO INNER JOIN
                         dbo.PRODUTO AS PROD ON PROD.CD_PRODUTO = PDD.CD_PRODUTO INNER JOIN
                         dbo.UNIDADE AS UN ON PROD.CD_UNIDADE = UN.CD_UNIDADE INNER JOIN
                         dbo.MARCA AS M ON PROD.CD_MARCA = M.CD_MARCA INNER JOIN
                         dbo.VENDEDOR AS V ON V.CD_VENDEDOR = doc.CD_VENDEDOR INNER JOIN
                         dbo.PESSOA AS PV ON PV.CD_PESSOA = V.CD_PESSOA INNER JOIN
                         dbo.PESSOA_DO_DOCUMENTO AS PST ON doc.CD_DOCUMENTO = PST.CD_DOCUMENTO AND PST.TP_PESSOA = 13
GROUP BY doc.NR_DOCUMENTO, doc.CD_DOCUMENTO, doc.CD_TIPO_DOCUMENTO, doc.CD_EMPRESA, doc.CD_DOCA, doc.CD_SITUACAO, doc.DT_HR_EMISSAO, doc.CD_VENDEDOR, TPC.DS_TIPO_COBRANCA, 
                         ATDOC.CD_LOTE, ATDOC.QT_ATENDIDA, LOC.CD_LOCALIZACAO, SEP.ID, L.NR_LOTE, L.SR_LOTE, L.DT_FABRICACAO, L.DT_VALIDADE, PDD.QT_SOLICITADA, PROD.NM_PRODUTO, PDD.CD_PRODUTO, 
                         UN.SIGLA, M.DS_MARCA, pd.CD_PESSOA, pd.CD_BAIRRO, pd.DS_BAIRRO, pd.RAZ_SOCIAL, pd.NR_ENDERECO, pd.COMPLEMENTO, pd.LOGRADOURO, pd.CD_CEP, pd.INSCRICAO, MC.DS_MUNICIPIO, 
                         pd.TELEFONE_1, p.CD_TRANSPORTADOR, d.DS_DOCA, PV.NM_PESSOA, PV.CD_PESSOA, PST.RAZ_SOCIAL

GO

