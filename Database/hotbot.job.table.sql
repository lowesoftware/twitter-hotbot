USE [HotBot]
GO

/****** Object:  Table [dbo].[Job]    Script Date: 05/23/2014 01:51:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Job](
	[JobId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Query] [nvarchar](128) NOT NULL,
	[IsLearning] [bit] NOT NULL,
	[MinMatchScore] [float] NOT NULL,
	[MaxNonMatchScore] [float] NOT NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[JobId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Job] ADD  CONSTRAINT [DF_Job_IsLearning]  DEFAULT ((1)) FOR [IsLearning]
GO

ALTER TABLE [dbo].[Job] ADD  CONSTRAINT [DF_Job_MinMatchScore]  DEFAULT ((0.8)) FOR [MinMatchScore]
GO

ALTER TABLE [dbo].[Job] ADD  CONSTRAINT [DF_Job_MaxNonMatchScore]  DEFAULT ((0.2)) FOR [MaxNonMatchScore]
GO

