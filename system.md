# Language Learning App - Agent Memory (system.md)

## 📌 Project Overview
Ứng dụng cá nhân giúp luyện phản xạ giao tiếp Tiếng Anh (mức độ A2-B1) bằng phương pháp dịch câu/đoạn hội thoại từ Tiếng Việt sang Tiếng Anh.
- **Đối tượng người dùng:** Lập trình viên IT.
- **Mục tiêu:** Giao tiếp hàng ngày và giao tiếp trong môi trường công việc IT.

---

## 🛠️ Tech Stack
- **Frontend:** React (Deploy trên Vercel)
- **Backend:** .NET 8 Web API (Deploy trên Render) *(Hiện tại cấu hình TargetFramework là net10.0 tương thích với môi trường hiện tại)*
- **Database:** PostgreSQL (Host trên Supabase)
- **ORM:** Entity Framework Core (Code-First)
- **AI Integration:** Google Gemini 3.1 Flash Lite API (Free Tier)

---

## 🗄️ Database Schema & Architecture

### 1. `Topics` (Chủ đề học)
- `Id` (int, PK)
- `Name` (string, max length 100, Required) - Tên chủ đề (VD: "Giao tiếp IT", "Đi siêu thị")
- `Description` (string, optional) - Mô tả chủ đề
- *Navigation:* `Sentences` (List<Sentence>)

### 2. `Sentences` (Câu học)
- `Id` (int, PK)
- `TopicId` (int, FK) - Liên kết tới `Topics`
- `VietnameseText` (string, max length 500, Required) - Câu hỏi tiếng Việt
- `EnglishText` (string, max length 500, Required) - Câu tham khảo tiếng Anh
- *Navigation:* `Topic`, `UserAttempts` (List<UserAttempt>)

### 3. `UserAttempts` (Lịch sử làm bài)
- `Id` (int, PK)
- `SentenceId` (int, FK) - Liên kết tới `Sentences`
- `UserTranslation` (string, Required) - Câu dịch của người dùng
- `AiFeedback` (string, Required) - Phản hồi từ AI (JSON string chứa chi tiết lỗi sai và gợi ý)
- `MistakeType` (string, Required) - Loại lỗi sai (VD: "Grammar", "Vocabulary", "Structure", "Naturalness", "All")
- `CreatedAt` (DateTime, UTC) - Thời gian thực hiện
- *Navigation:* `Sentence`

---

## 🚀 Core Features
1. **Lấy câu hỏi:** API lấy ngẫu nhiên hoặc theo chủ đề câu Tiếng Việt từ Database để hiển thị cho người dùng dịch.
2. **Chấm điểm & Feedback bằng AI:** Gọi Gemini API để phân tích câu dịch của người dùng, chấm điểm, chỉ ra lỗi sai và đề xuất phương án dịch tự nhiên hơn.
3. **Lưu lịch sử:** Lưu lại toàn bộ kết quả dịch (`UserTranslation`, `AiFeedback`, `MistakeType`) vào bảng `UserAttempts`.
4. **Tổng hợp cuối bài học:** Sau khi hoàn thành một set câu hỏi, hệ thống tổng hợp toàn bộ lỗi sai đã mắc phải trong set đó, gọi AI phân tích điểm yếu và đề xuất giải pháp cải thiện cụ thể.
5. **Đọc thành tiếng (Text-to-Speech):** Đọc to các phương án dịch đúng chuẩn tiếng Anh bằng giọng đọc bản xứ thông qua Web Speech API.
6. **Trang cá nhân & Lịch sử học tập:** Thống kê số câu đã học, tỷ lệ dịch chính xác, phân loại lỗi sai thường gặp và lịch sử chi tiết tất cả các lượt làm bài, hiển thị trực quan câu dịch đề xuất từ AI kèm giọng đọc phát âm chuẩn.
7. **Hiện hạn mức sử dụng (Rate Limit) API Google Gemini:**
    - Hiển thị rõ ràng thông tin giới hạn sử dụng của Google Gemini API (Free Tier) ngay trên giao diện ứng dụng.
    - Thông tin bao gồm: Số lượng yêu cầu tối đa mỗi phút (RPM), mỗi ngày (RPD) và thời gian reset tự động.
    - Cập nhật realtime khi người dùng thực hiện các thao tác gọi API, hiển thị trạng thái giới hạn (Normal/Warning/Limit) và tính năng tự động tạm dừng hoạt động khi đạt đến giới hạn.
    - Giúp người dùng nắm bắt được mức độ sử dụng và tránh vượt quá giới hạn cho phép.
8. **Hiển thị điểm yếu và các lỗi thường gặp:**
    - Sử dụng AI để phân tích tổng hợp các lỗi sai mà người dùng hay gặp phải trong quá trình luyện tập.
    - Hiển thị kết quả phân tích dưới dạng các nhãn (badge) với màu sắc khác nhau, trực quan hóa các điểm cần cải thiện.
    - Giúp người dùng nhận biết nhanh các vấn đề ngữ pháp, từ vựng hoặc cấu trúc cần khắc phục.
    - Kết hợp với phần gợi ý từ AI, cung cấp giải pháp và phương pháp cải thiện cụ thể cho từng loại lỗi.
9. **Kết thúc buổi học sớm (Finish Early):** Tích hợp nút kết thúc sớm trên màn hình luyện tập. Nếu người dùng chọn dừng giữa chừng khi đã hoàn thành ít nhất 1 câu, hệ thống sẽ gửi danh sách các câu đã làm tới AI để tiến hành đánh giá tổng kết ngay lập tức thay vì bắt buộc hoàn thành tất cả câu.

---

## 📈 Current Project Progress & Status
- **Database Connection & Migration:** Đã sửa lỗi tên cột `CreateAt` thành `CreatedAt` trong migration, thực hiện đồng bộ cấu hình C# Model và Database thành công trên PostgreSQL Supabase.
- **Frontend Development:** Đã xây dựng hoàn tất dự án React + Vite (sử dụng Tailwind CSS v4 và React Router DOM v7) tại thư mục `frontend/`.
- **Giao diện Neo-Brutalism:** Thiết kế giao diện theo phong cách Neo-Brutalism cao cấp, tối ưu hóa hiển thị và hỗ trợ đầy đủ chế độ Light/Dark Theme (đồng bộ lưu trữ qua localStorage).
- **AI Integration:** Khắc phục lỗi tích hợp Gemini API, điều chỉnh gọi đúng model tương thích `gemini-3.1-flash-lite`.
- **Phân tích kỹ năng từ AI:** Triển khai tính năng tự động phân tích và chỉ ra Điểm còn yếu & Cách khắc phục dựa trên lịch sử lỗi sai của người dùng, hiển thị ngắn gọn trực quan dưới dạng nhãn dán màu sắc (Badges).
- **Giới hạn API Gemini:** Hiển thị chi tiết giới hạn API key (Free Tier: 15 RPM, 1500 RPD) và thời gian tự động reset (14:00 theo giờ Việt Nam) trực tiếp trên trang cá nhân. Đã hoàn thiện logic đếm số lượt gọi API động ở backend, tích hợp middleware xử lý lỗi HTTP 429 và cơ chế cảnh báo/thử lại ở giao diện người dùng.
- **Ponytail Integration:** Đã cài đặt các rule và file steering của Ponytail vào dự án để áp dụng triết lý "lazy senior developer".


