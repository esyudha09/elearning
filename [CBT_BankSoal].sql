USE [SCHEMA_ERP]
GO

/****** Object:  Table [dbo].[CBT_BankSoal]    Script Date: 3/1/2023 2:11:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CBT_BankSoal](
	[Kode] [uniqueidentifier] NOT NULL,
	[Rel_Mapel] [varchar](50) NULL,
	[Rel_Guru] [varchar](50) NULL,
	[Soal] [nvarchar](1000) NULL,
	[JwbEssay] [nvarchar](1000) NULL,
	[Rel_JwbGanda] [varchar](50) NULL,
	[Jenis] [varchar](20) NULL,
	[Tanggal_Buat] [datetime] NULL,
	[Tanggal_Update] [datetime] NULL,
	
	
 CONSTRAINT [PK_CBT_Soal] PRIMARY KEY CLUSTERED 
(
	[Kode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


