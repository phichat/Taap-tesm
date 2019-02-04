/*
Navicat SQL Server Data Transfer

Source Server         : AmpeliteCloud
Source Server Version : 140000
Source Host           : 203.154.45.40:1433
Source Database       : db_Taap
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 140000
File Encoding         : 65001

Date: 2019-02-05 02:25:08
*/


-- ----------------------------
-- Table structure for Permissions
-- ----------------------------
DROP TABLE [dbo].[Permissions]
GO
CREATE TABLE [dbo].[Permissions] (
[PermissionID] int NOT NULL IDENTITY(1,1) ,
[RoleID] int NOT NULL ,
[Form] varchar(50) NOT NULL ,
[Description] varchar(250) NULL ,
[Viewer] bit NULL ,
[Creater] bit NULL ,
[Editer] bit NULL ,
[Deleter] bit NULL ,
[Printer] bit NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[Permissions]', RESEED, 28)
GO

-- ----------------------------
-- Records of Permissions
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Permissions] ON
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'1', N'1', N'parts-receive', null, N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'3', N'1', N'rpt-parts-receive', null, N'1', N'0', N'0', N'0', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'4', N'1', N'rpt-parts-movement', null, N'1', N'0', N'0', N'0', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'5', N'1', N'rpt-stock-available', null, N'1', N'0', N'0', N'0', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'6', N'1', N'rpt-stock-material', null, N'1', N'0', N'0', N'0', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'7', N'1', N'rpt-cars-movement', null, N'1', N'0', N'0', N'0', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'8', N'2', N'parts-receive', null, N'1', N'0', N'0', N'0', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'11', N'1', N'users', N'จัดการข้อมูล "ผู้ใช้งาน"', N'1', N'1', N'1', N'1', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'12', N'1', N'roles', N'จัดการข้อมูล "หน้าที่การทำงาน"', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'13', N'1', N'permissions', N'จัดการข้อมูล "สิทธิผู้ใช้งาน"', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'14', N'1', N'buy-off', N'หน้าสำหรับอัพโหลด Buy off File และ Reference File', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'15', N'1', N'master-types', N'จัดการข้อมูล "รุ่นรถ/ประเภท"', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'16', N'1', N'master-shops', N'จัดการข้อมูล "แผนก"', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'17', N'1', N'master-models', N'จัดการข้อมูล "โมเดลรถ"', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'18', N'1', N'master-type-items', N'จัดการผูกข้อมูล "รุ่นรถ/ประเภท" กับ แผนก ', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'19', N'1', N'transfer', N'จัดการข้อมูลใบโอน', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'20', N'1', N'wms-imp-cpl', N'import cpl', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'21', N'1', N'wms-imp-fpl', N'import fpl', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'22', N'1', N'wms-imp-rec', N'import receipt', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'23', N'1', N'wms-tf-move', N'Transfer movement', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'24', N'1', N'wms-out-bound', N'Out bound', N'1', N'1', N'1', N'1', N'0')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'25', N'1', N'rpt-wms-stock-available', null, N'1', N'1', N'1', N'1', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'26', N'1', N'rpt-wms-part-receipt', null, N'1', N'1', N'1', N'1', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'27', N'1', N'rpt-wms-part-movement', null, N'1', N'1', N'1', N'1', N'1')
GO
GO
INSERT INTO [dbo].[Permissions] ([PermissionID], [RoleID], [Form], [Description], [Viewer], [Creater], [Editer], [Deleter], [Printer]) VALUES (N'28', N'1', N'rpt-wms-movement', null, N'1', N'1', N'1', N'1', N'1')
GO
GO
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO

-- ----------------------------
-- Indexes structure for table Permissions
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table Permissions
-- ----------------------------
ALTER TABLE [dbo].[Permissions] ADD PRIMARY KEY ([PermissionID])
GO
