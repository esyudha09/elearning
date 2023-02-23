USE [SCHEMA_ERP]
GO

/****** Object:  Table [dbo].[CBT_BankSoal]    Script Date: 2/21/2023 3:00:54 PM ******/
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
	[JwbGanda1] [nvarchar](1000) NULL,
	[JwbGanda2] [nvarchar](1000) NULL,
	[JwbGanda3] [nvarchar](1000) NULL,
	[JwbGanda4] [nvarchar](1000) NULL,
	[JwbGanda5] [nvarchar](1000) NULL,
	[Tanggal_Buat] [datetime] NULL,
	[Tanggal_Update] [datetime] NULL,
	[JwbGanda] [varchar](1) NULL,
 CONSTRAINT [PK_CBT_Soal] PRIMARY KEY CLUSTERED 
(
	[Kode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


