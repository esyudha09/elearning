USE [SCHEMA_ERP]
GO

/****** Object:  Table [dbo].[CBT_BankSoalJwbGanda]    Script Date: 3/1/2023 4:12:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CBT_BankSoalJwbGanda](
	[Kode] [uniqueidentifier] NOT NULL,
	[Rel_BankSoal] [varchar](50) NULL,
	[Jawaban] [nvarchar](1000) NULL,
 CONSTRAINT [PK_CBT_JawabanSoalGanda] PRIMARY KEY CLUSTERED 
(
	[Kode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


