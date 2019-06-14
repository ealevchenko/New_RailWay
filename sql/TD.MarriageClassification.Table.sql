USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [TD].[MarriageClassification]    Script Date: 14.06.2019 17:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TD].[MarriageClassification](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[classification] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_MarriageClassification] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [TD].[MarriageClassification] ON 

INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (1, N'взрез стрелочного перевода')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (2, N'наезд на тупик')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (3, N'перевод стрелки под составом')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (4, N'повреждение')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (5, N'проезд запрещающего показания светофора')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (6, N'просыпь агломерата')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (7, N'самораскрытие')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (8, N'самораскрытие, сход с рельс')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (9, N'саморасцеп')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (10, N'столкновение')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (11, N'столкновение со сходом с рельс')
INSERT [TD].[MarriageClassification] ([id], [classification]) VALUES (12, N'сход с рельс')
SET IDENTITY_INSERT [TD].[MarriageClassification] OFF
