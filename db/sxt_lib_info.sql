CREATE TABLE [dbo].[sxt_lib_info](
	[lib_id] 		[int] 			 NOT NULL,		-- ID，与图书馆一致
	[lib_title] 	[nvarchar](200)	 NOT NULL,		-- 标题
	[lib_author] 	[nvarchar](100)	 NOT NULL,		-- 作者
	[lib_press] 	[nvarchar](200)	 NOT NULL,		-- 出版社
	[lib_subject] 	[nvarchar](200)	 NOT NULL,		-- 主题
	[lib_type] 		[nvarchar](200)	 NOT NULL,		-- 类型编码
	[lib_page] 		[int] 			 NULL,			-- 页数
	[lib_price] 	[decimal](10, 3) NULL,			-- 价格
	[lib_currency] 	[nvarchar](20)	 NULL,			-- 币种
	[lib_isbn] 		[varchar](50)	 NOT NULL,		-- ISBN
	[lib_callno] 	[varchar](50)	 NOT NULL,		-- 索取码
	[lib_desc] 		[nvarchar](max)	 NOT NULL,		-- 描述
)


