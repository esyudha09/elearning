USE [SCHEMA_ERP]
GO

/****** Object:  Table [dbo].[CBT_StatusSiswa]    Script Date: 3/8/2023 12:02:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CBT_StatusSiswa](
	[Kode] [uniqueidentifier] NOT NULL,
	[Rel_RumahSoal] [varchar](50) NULL,
	[Rel_Siswa] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_CBT_StatusSiswa] PRIMARY KEY CLUSTERED 
(
	[Kode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


