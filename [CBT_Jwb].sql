USE [SCHEMA_ERP]
GO

/****** Object:  Table [dbo].[CBT_BankSoal]    Script Date: 04-Mar-23 5:35:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CBT_Jwb](
	[Kode] [uniqueidentifier] NOT NULL,
	[Rel_DesignSoal] [varchar](50) NULL,
	[Rel_Siswa] [varchar](50) NULL,
	[Rel_JwbGanda] [nvarchar](50) NULL,
	[JwbEssay] [nvarchar](1000) NULL,	
	[Tanggal_Buat] [datetime] NULL,
	
 CONSTRAINT [PK_CBT_Jwb] PRIMARY KEY CLUSTERED 
(
	[Kode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


