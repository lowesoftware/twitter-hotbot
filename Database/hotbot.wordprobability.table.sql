USE [HotBot]
GO

/****** Object:  Table [dbo].[WordProbability]    Script Date: 05/23/2014 01:52:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WordProbability](
	[WordProbabilityId] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[Word] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Matches] [int] NOT NULL,
	[NonMatches] [int] NOT NULL,
 CONSTRAINT [PK_WordProbability] PRIMARY KEY CLUSTERED 
(
	[WordProbabilityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[WordProbability]  WITH CHECK ADD  CONSTRAINT [FK_WordProbability_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WordProbability] CHECK CONSTRAINT [FK_WordProbability_Job]
GO

ALTER TABLE [dbo].[WordProbability] ADD  CONSTRAINT [DF_WordProbability_Matches]  DEFAULT ((0)) FOR [Matches]
GO

ALTER TABLE [dbo].[WordProbability] ADD  CONSTRAINT [DF_WordProbability_NonMatches]  DEFAULT ((0)) FOR [NonMatches]
GO

