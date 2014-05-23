USE [HotBot]
GO

/****** Object:  Table [dbo].[Friend]    Script Date: 05/23/2014 01:51:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Friend](
	[FriendId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[IsFollowed] [bit] NOT NULL,
 CONSTRAINT [PK_Friend] PRIMARY KEY CLUSTERED 
(
	[FriendId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Friend] ADD  CONSTRAINT [DF_Friend_IsFollowed]  DEFAULT ((0)) FOR [IsFollowed]
GO

