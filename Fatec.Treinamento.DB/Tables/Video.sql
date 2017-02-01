CREATE TABLE [dbo].[Video] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Nome]       VARCHAR (50) NOT NULL,
    [Duracao]    INT          NOT NULL,
    [IdCapitulo] INT          NOT NULL,
    [CodigoYoutube] VARCHAR(100) NULL, 
    CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Video_Capitulo] FOREIGN KEY ([IdCapitulo]) REFERENCES [dbo].[Capitulo] ([Id])
);

