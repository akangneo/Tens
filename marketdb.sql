USE [marketdb]
GO
/****** Object:  Table [dbo].[addresse_types]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[addresse_types](
	[address_type_code] [char](4) NOT NULL,
	[address_type_description] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[address_type_code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[suppliers]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[suppliers](
	[id_supplier] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[email] [varchar](50) NULL,
	[phone] [varchar](50) NULL,
	[cellphone] [varchar](50) NULL,
	[other_detils] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_supplier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[roles]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[roles](
	[id_role] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_role] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[countries]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[countries](
	[id_country] [int] IDENTITY(1,1) NOT NULL,
	[country_name] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_country] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[brands]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[brands](
	[id_brand] [int] IDENTITY(1,1) NOT NULL,
	[brand_name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_brand] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[item_categories]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[item_categories](
	[item_category_code] [char](4) NOT NULL,
	[category_description] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[item_category_code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[inventrory_items]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[inventrory_items](
	[id_item] [int] IDENTITY(1,1) NOT NULL,
	[brand_id] [int] NULL,
	[item_category_code] [char](4) NULL,
	[item_description] [text] NULL,
	[avarage_montly_usage] [int] NULL,
	[recorder_level] [int] NULL,
	[recorder_quantity] [int] NULL,
	[other_item_details] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_item] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[addresses]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[addresses](
	[id_address] [int] IDENTITY(1,1) NOT NULL,
	[description] [text] NULL,
	[city] [varchar](50) NULL,
	[postal_code] [varchar](50) NULL,
	[country_id] [int] NULL,
	[other_address_details] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_address] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[persons]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[persons](
	[id_person] [int] IDENTITY(1,1) NOT NULL,
	[person_name] [varchar](50) NULL,
	[gender] [int] NULL,
	[phone] [varchar](50) NULL,
	[cellphone] [varchar](50) NULL,
	[email] [varchar](50) NULL,
	[address_id] [int] NULL,
	[image] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_person] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[item_suppliers]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[item_suppliers](
	[item_id] [int] NULL,
	[supplier_id] [int] NULL,
	[value_supplied_to_date] [int] NULL,
	[total_quantity_supplied_to_date] [int] NULL,
	[first_item_supplied_to_date] [int] NULL,
	[last_item_supplied_to_date] [int] NULL,
	[delivery_lead_time] [time](7) NULL,
	[standard_price] [money] NULL,
	[percentage_discount] [decimal](18, 0) NULL,
	[minimum_order_quantity] [int] NULL,
	[maximum_order_quantity] [int] NULL,
	[other_item_supplier_details] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[item_stock_levels]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[item_stock_levels](
	[item_id] [int] NULL,
	[stocking_taking_date] [int] NULL,
	[quantity_in_stock] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[suppliers_addresses]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[suppliers_addresses](
	[id_suppliers_address] [int] IDENTITY(1,1) NOT NULL,
	[address_id] [int] NULL,
	[supplier_id] [int] NULL,
	[address_type_code] [char](4) NULL,
	[date_address_from] [date] NULL,
	[date_address_to] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_suppliers_address] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[users]    Script Date: 07/23/2016 22:32:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[users](
	[id_user] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[role_id] [int] NULL,
	[person_id] [int] NULL,
	[status] [int] NULL,
	[date_expired] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[id_user] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK__addresses__count__300424B4]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[addresses]  WITH CHECK ADD FOREIGN KEY([country_id])
REFERENCES [dbo].[countries] ([id_country])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__inventror__brand__30F848ED]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[inventrory_items]  WITH CHECK ADD FOREIGN KEY([brand_id])
REFERENCES [dbo].[brands] ([id_brand])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__inventror__item___31EC6D26]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[inventrory_items]  WITH CHECK ADD FOREIGN KEY([item_category_code])
REFERENCES [dbo].[item_categories] ([item_category_code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__item_stoc__item___32E0915F]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[item_stock_levels]  WITH CHECK ADD FOREIGN KEY([item_id])
REFERENCES [dbo].[inventrory_items] ([id_item])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__item_supp__item___33D4B598]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[item_suppliers]  WITH CHECK ADD FOREIGN KEY([item_id])
REFERENCES [dbo].[inventrory_items] ([id_item])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__item_supp__suppl__34C8D9D1]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[item_suppliers]  WITH CHECK ADD FOREIGN KEY([supplier_id])
REFERENCES [dbo].[suppliers] ([id_supplier])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__persons__address__35BCFE0A]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[persons]  WITH CHECK ADD FOREIGN KEY([address_id])
REFERENCES [dbo].[addresses] ([id_address])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__suppliers__addre__36B12243]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[suppliers_addresses]  WITH CHECK ADD FOREIGN KEY([address_type_code])
REFERENCES [dbo].[addresse_types] ([address_type_code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__suppliers__addre__37A5467C]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[suppliers_addresses]  WITH CHECK ADD FOREIGN KEY([address_id])
REFERENCES [dbo].[addresses] ([id_address])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__suppliers__suppl__38996AB5]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[suppliers_addresses]  WITH CHECK ADD FOREIGN KEY([supplier_id])
REFERENCES [dbo].[suppliers] ([id_supplier])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__users__person_id__398D8EEE]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[users]  WITH CHECK ADD FOREIGN KEY([person_id])
REFERENCES [dbo].[persons] ([id_person])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
/****** Object:  ForeignKey [FK__users__role_id__3A81B327]    Script Date: 07/23/2016 22:32:34 ******/
ALTER TABLE [dbo].[users]  WITH CHECK ADD FOREIGN KEY([role_id])
REFERENCES [dbo].[roles] ([id_role])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
