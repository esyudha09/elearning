USE [SCHEMA_ERP]
GO

/****** Object:  Table [dbo].[CBT_RumahSoal]    Script Date: 2/21/2023 3:48:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CBT_DesignSoal](
	[Kode] [uniqueidentifier] NOT NULL,
	[Rel_RumahSoal] varchar(50) null,
	[Rel_BankSoal] [varchar](50) NULL,
	[Jenis] [varchar](20) NULL,
	[Skor] [int] NULL,	
	[CreatedDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_CBT_DesignSoal] PRIMARY KEY CLUSTERED 
(
	[Kode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


