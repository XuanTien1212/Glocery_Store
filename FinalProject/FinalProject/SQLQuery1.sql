CREATE TABLE [dbo].[Product] (
    [ProductNum]    INT IDENTITY(1,1) NOT NULL,
    [ProductID]		char(6) NOT NULL constraint PK_Product Primary key,
    [NamePro]       NVARCHAR (50)  NULL,
    [CateID]        char(2)  NULL,
    [BrandID]		char(4)      NULL,
    [Price]         FLOAT NOT NULL,
    [ImagePro]      NVARCHAR (50)  NULL,
    [DescriptionPro] NVARCHAR (50)  NULL,
    [Quantity]      INT NOT NULL,
);

CREATE TABLE [dbo].[Category] (
    [IDCate]   char(2) NOT NULL constraint PK_Category Primary key,
    [NameCate] NVARCHAR (50) NULL,
);

CREATE TABLE [dbo].[ProductBrand] (
    [CategoryID] char(2) NULL,
    [IDBrand] char(4) NOT NULL constraint PK_ProductBrand Primary key,
    [NameBrand] NVARCHAR (50) NULL
);

-- Bang AdminUser
CREATE TABLE [dbo].[AdminUser] (
    [ID]           INT            NOT NULL,
    [NameUser]     NVARCHAR (50) NOT NULL,
    [RoleUser]     NVARCHAR (50) NULL,
    [PasswordUser] NCHAR (50)     NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

--Bang Customer
CREATE TABLE [dbo].[Customer] (
    [IDCus]    INT            IDENTITY (1, 1) NOT NULL,
    [NameCus]  NVARCHAR (50) NOT NULL,
    [PhoneCus] NVARCHAR (15)  NOT NULL,
    [EmailCus] NVARCHAR (50) NULL,
    [AddressDelivery] NVARCHAR (50) NOT NULL,
    [UserName] VARCHAR (50) NOT NULL,
    [Password] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([IDCus] ASC)
);


--Bang OrderPro
CREATE TABLE [dbo].[OrderPro] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [DateOrder]        DATE           NULL,
    [IDCus]            INT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDCus]) REFERENCES [dbo].[Customer] ([IDCus])
);

--Bang OrderDetail
CREATE TABLE [dbo].[OrderDetail] (
    [ID]        INT        IDENTITY (1, 1) NOT NULL,
    [IDPro] char(6)       NULL,
    [IDOrder]   INT        NULL,
    [Quantity]  INT       NOT NULL,
    [UnitPrice] FLOAT NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDOrder]) REFERENCES [dbo].[OrderPro] ([ID])
);

ALTER TABLE [dbo].[OrderPro] add constraint FK_OrderPro_Customer foreign key(IDCus) references [dbo].[Customer](IDCus)
ALTER TABLE [dbo].[OrderDetail] add constraint FK_OrderDetail_Product foreign key(IDPro) references [dbo].[Product](ProductID)

ALTER TABLE [dbo].[Product] add constraint FK_Product_Category foreign key(CateID) references [dbo].[Category](IDCate)
ALTER TABLE [dbo].[Product] add constraint FK_Product_Brand foreign key(BrandID) references [dbo].[ProductBrand](IDBrand)
ALTER TABLE [dbo].[ProductBrand] add constraint FK_Brand_Category foreign key(CategoryID) references [dbo].[Category](IDCate)


Insert into AdminUser values (1,'Admin1','Admin','123456789')