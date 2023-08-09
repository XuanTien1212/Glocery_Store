USE DBEcommerceWeb; -- Thay thế YourDatabaseName bằng tên của cơ sở dữ liệu

-- Đặt giá trị khởi tạo cho cột Identity của bảng Product
DBCC CHECKIDENT ('Product', RESEED, 1);
-- Đặt giá trị khởi tạo cho cột Identity của bảng OrderPro
DBCC CHECKIDENT ('OrderPro', RESEED, 1);

-- Đặt giá trị khởi tạo cho cột Identity của bảng OrderDetail
DBCC CHECKIDENT ('OrderDetail', RESEED, 1);

