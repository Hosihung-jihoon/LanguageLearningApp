using LanguageLearningApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningApp.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // ==========================================
            // TOPIC 1: GIAO TIẾP HẰNG NGÀY
            // ==========================================
            var topicDaily = await context.Topics.FirstOrDefaultAsync(t => t.Name == "Giao tiếp hằng ngày");
            if (topicDaily == null)
            {
                topicDaily = new Topic
                {
                    Name = "Giao tiếp hằng ngày",
                    Description = "Luyện tập các mẫu câu và hội thoại giao tiếp cơ bản hàng ngày."
                };
                context.Topics.Add(topicDaily);
                await context.SaveChangesAsync();
            }

            var dailySentences = new List<Sentence>
            {
                // --- CÂU ĐƠN (20 câu) ---
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Chào buổi sáng, hôm nay bạn thế nào?", EnglishText = "Good morning, how are you doing today?", Phonetic = "/ɡʊd ˈmɔːrnɪŋ, haʊ ɑːr juː ˈduːɪŋ təˈdeɪ/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Rất vui được gặp bạn.", EnglishText = "Nice to meet you.", Phonetic = "/naɪs tuː miːt juː/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Cảm ơn vì đã giúp đỡ tôi nhé.", EnglishText = "Thank you so much for your help.", Phonetic = "/θæŋk juː soʊ mʌtʃ fɔːr jɔːr help/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Không sao đâu, đừng lo lắng quá.", EnglishText = "No problem, don't worry about it too much.", Phonetic = "/noʊ ˈprɑːbləm, doʊnt ˈwɜːri əˈbaʊt ɪt tuː mʌtʃ/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Làm ơn cho tôi hỏi đường đến ga tàu gần nhất.", EnglishText = "Excuse me, can you show me the way to the nearest train station?", Phonetic = "/ɪkˈskjuːz mi, kæn juː ʃoʊ mi ðə weɪ tuː ðə ˈnɪrəst treɪn ˈsteɪʃn/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Bạn có muốn uống chút trà hoặc cà phê không?", EnglishText = "Would you like some tea or coffee?", Phonetic = "/wʊd juː laɪk sʌm tiː ɔːr ˈkɔːfi/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Hôm nay thời tiết đẹp quá nhỉ!", EnglishText = "The weather is really nice today, isn't it?", Phonetic = "/ðə ˈweðər ɪz ˈriːəli naɪs təˈdeɪ, ˈɪznt ɪt/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Cái này giá bao nhiêu tiền vậy?", EnglishText = "How much does this cost?", Phonetic = "/haʊ mʌtʃ dʌz ðɪs kɔːst/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Xin lỗi, tôi đang vội nên phải đi bây giờ.", EnglishText = "Sorry, I am in a hurry so I must go now.", Phonetic = "/ˈsɔːri, aɪ æm ɪn ə ˈhɜːri soʊ aɪ mʌst ɡoʊ naʊ/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Chúc bạn cuối tuần vui vẻ nhé!", EnglishText = "Have a great weekend!", Phonetic = "/hæv ə ɡreɪt ˈwiːk.ɛnd/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Bạn có thời gian rảnh vào tối nay không?", EnglishText = "Do you have any free time tonight?", Phonetic = "/duː juː hæv ˈɛni friː taɪm təˈnaɪt/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Tôi đồng ý với ý kiến của bạn.", EnglishText = "I agree with your opinion.", Phonetic = "/aɪ əˈɡriː wɪð jɔːr əˈpɪnjən/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Bạn có thể nói chậm hơn một chút được không?", EnglishText = "Could you please speak a little slower?", Phonetic = "/kʊd juː pliːz spiːk ə ˈlɪtl ˈsloʊər/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Chúc mừng sinh nhật bạn nhé!", EnglishText = "Happy birthday to you!", Phonetic = "/ˈhæpi ˈbɜːrθdeɪ tuː juː/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Tôi thực sự xin lỗi vì sự bất tiện này.", EnglishText = "I am truly sorry for this inconvenience.", Phonetic = "/aɪ æm ˈtruːli ˈsɔːri fɔːr ðɪs ˌɪnkənˈviːniəns/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Hẹn gặp lại bạn vào ngày mai nhé.", EnglishText = "See you tomorrow.", Phonetic = "/siː juː təˈmɔːroʊ/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Món ăn này thực sự rất ngon!", EnglishText = "This dish is absolutely delicious!", Phonetic = "/ðɪs dɪʃ ɪz ˌæbsəˈluːtli dɪˈlɪʃəs/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Bạn có thể giúp tôi chụp một bức ảnh không?", EnglishText = "Could you please take a photo for me?", Phonetic = "/kʊd juː pliːz teɪk ə ˈfoʊtoʊ fɔːr mi/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Tôi cảm thấy hơi mệt hôm nay.", EnglishText = "I feel a bit tired today.", Phonetic = "/aɪ fiːl ə bɪt ˈtaɪərd təˈdeɪ/", Type = "Single" },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Chúc bạn một ngày tốt lành!", EnglishText = "Have a nice day!", Phonetic = "/hæv ə naɪs deɪ/", Type = "Single" },

                // --- HỘI THOẠI (20 câu) ---
                // Hội thoại 1: Đi cà phê (ConversationId = 1 - 6 câu)
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Chào Lan, cuối tuần này cậu có kế hoạch gì chưa?", EnglishText = "Hi Lan, do you have any plans for this weekend?", Phonetic = "/haɪ læn, duː juː hæv ˈɛni plænz fɔːr ðɪs ˈwiːk.ɛnd/", Type = "Conversation", ConversationId = 1 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Tớ chưa. Có chuyện gì thế?", EnglishText = "Not yet. What's up?", Phonetic = "/nɑːt jet. wʌts ʌp/", Type = "Conversation", ConversationId = 1 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Chúng ta đi uống cà phê nhé.", EnglishText = "Let's go grab a coffee.", Phonetic = "/lets ɡoʊ ɡræb ə ˈkɔːfi/", Type = "Conversation", ConversationId = 1 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Ý kiến hay đó! Khi nào chúng ta đi?", EnglishText = "Great idea! When should we go?", Phonetic = "/ɡreɪt aɪˈdiːə! wen ʃʊd wiː ɡoʊ/", Type = "Conversation", ConversationId = 1 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Sáng Chủ Nhật lúc 9 giờ được không?", EnglishText = "Is Sunday morning at 9 AM okay?", Phonetic = "/ɪz ˈsʌndeɪ ˈmɔːrnɪŋ æt naɪn eɪ-ɛm oʊˈkeɪ/", Type = "Conversation", ConversationId = 1 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Được chứ, gặp lại cậu sau nhé!", EnglishText = "Sure, see you then!", Phonetic = "/ʃʊr, siː juː ðen/", Type = "Conversation", ConversationId = 1 },

                // Hội thoại 2: Hỏi thăm sức khỏe (ConversationId = 2 - 7 câu)
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Dạo này bạn thế nào? Trông hơi mệt mỏi đấy.", EnglishText = "How have you been lately? You look a bit tired.", Phonetic = "/haʊ hæv juː biːn ˈleɪtli? juː lʊk ə bɪt ˈtaɪərd/", Type = "Conversation", ConversationId = 2 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Dạo này tớ bận quá, ngủ không đủ giấc.", EnglishText = "I've been so busy recently, I don't get enough sleep.", Phonetic = "/aɪv biːn soʊ ˈbɪzi ˈriːsntli, aɪ doʊnt ɡet ɪˈnʌf sliːp/", Type = "Conversation", ConversationId = 2 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Cố gắng nghỉ ngơi nhiều hơn nhé. Gia đình bạn vẫn khỏe chứ?", EnglishText = "Try to rest more. Is your family doing well?", Phonetic = "/traɪ tuː rest mɔːr. ɪz jɔːr ˈfæməli ˈduːɪŋ wel/", Type = "Conversation", ConversationId = 2 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Mọi người vẫn khỏe, cảm ơn cậu đã hỏi thăm.", EnglishText = "Everyone is doing great, thanks for asking.", Phonetic = "/ˈevriwʌn ɪz ˈduːɪŋ ɡreɪt, θæŋks fɔːr ˈæskɪŋ/", Type = "Conversation", ConversationId = 2 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Bố mẹ tớ vừa đi du lịch về hôm qua.", EnglishText = "My parents just returned from a trip yesterday.", Phonetic = "/maɪ ˈperənts dʒʌst rɪˈtɜːrnd frʌm ə trɪp ˈjestərdeɪ/", Type = "Conversation", ConversationId = 2 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Tuyệt quá! Khi nào rảnh tớ sẽ qua thăm họ.", EnglishText = "That's great! When I'm free, I will visit them.", Phonetic = "/ðæts ɡreɪt! wen aɪm friː, aɪ wɪl ˈvɪzɪt ðem/", Type = "Conversation", ConversationId = 2 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Họ chắc chắn sẽ rất vui khi gặp cậu.", EnglishText = "They will definitely be very happy to see you.", Phonetic = "/ðeɪ wɪl ˈdefɪnətli biː ˈveri ˈhæpi tuː siː juː/", Type = "Conversation", ConversationId = 2 },

                // Hội thoại 3: Hẹn ăn tối (ConversationId = 3 - 7 câu)
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Tối nay cậu muốn ăn gì không?", EnglishText = "Would you like to eat something tonight?", Phonetic = "/wʊd juː laɪk tuː iːt ˈsʌmθɪŋ təˈnaɪt/", Type = "Conversation", ConversationId = 3 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Tớ thèm ăn đồ Nhật quá.", EnglishText = "I really crave Japanese food.", Phonetic = "/aɪ ˈriːəli kreɪv ˌdʒæpəˈniːz fuːd/", Type = "Conversation", ConversationId = 3 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Có một nhà hàng sushi mới mở ở gần đây đấy.", EnglishText = "There is a new sushi restaurant opening nearby.", Phonetic = "/ðer ɪz ə nuː ˈsuːʃi ˈrestrɑːnt ˈoʊpnɪŋ ˌnɪrˈbaɪ/", Type = "Conversation", ConversationId = 3 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Thật sao? Đồ ăn ở đó có ngon không?", EnglishText = "Really? Is the food there good?", Phonetic = "/ˈriːəli? ɪz ðə fuːd ðer ɡʊd/", Type = "Conversation", ConversationId = 3 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Nghe nói cá ở đó rất tươi và giá cả phải chăng.", EnglishText = "I heard the fish there is very fresh and the price is reasonable.", Phonetic = "/aɪ hɜːrd ðə fɪʃ ðer ɪz ˈveri freʃ ænd ðə praɪs ɪz ˈriːznəbl/", Type = "Conversation", ConversationId = 3 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Vậy đặt bàn trước đi kẻo hết chỗ.", EnglishText = "Then let's book a table in advance before it gets crowded.", Phonetic = "/ðen lets bʊk ə ˈteɪbl ɪn ədˈvæns bɪˈfɔːr ɪt ɡets ˈkraʊdɪd/", Type = "Conversation", ConversationId = 3 },
                new Sentence { TopicId = topicDaily.Id, VietnameseText = "Đồng ý, tớ sẽ gọi điện đặt chỗ ngay bây giờ.", EnglishText = "Agreed, I will call to make a reservation right now.", Phonetic = "/əˈɡriːd, aɪ wɪl kɔːl tuː meɪk ə ˌrezərˈveɪʃn raɪt naʊ/", Type = "Conversation", ConversationId = 3 }
            };

            await SeedSentencesSafelyAsync(context, dailySentences);

            // ==========================================
            // TOPIC 2: GIAO TIẾP IT (CÔNG NGHỆ THÔNG TIN)
            // ==========================================
            var topicIt = await context.Topics.FirstOrDefaultAsync(t => t.Name == "Giao tiếp IT");
            if (topicIt == null)
            {
                topicIt = new Topic
                {
                    Name = "Giao tiếp IT",
                    Description = "Luyện tập tiếng Anh chuyên ngành công nghệ thông tin, họp dự án và báo cáo lỗi."
                };
                context.Topics.Add(topicIt);
                await context.SaveChangesAsync();
            }

            var itSentences = new List<Sentence>
            {
                // --- CÂU ĐƠN (20 câu) ---
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Mã nguồn này cần được tối ưu hóa hiệu năng.", EnglishText = "This source code needs to be optimized for performance.", Phonetic = "/ðɪs sɔːrs koʊd niːdz tuː biː ˈɑːptɪmaɪzd fɔːr pərˈfɔːrməns/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Tôi đã phát hiện một lỗi bảo mật nghiêm trọng.", EnglishText = "I detected a critical security vulnerability.", Phonetic = "/aɪ dɪˈtektɪd ə ˈkrɪtɪkl sɪˈkjʊrəti ˌvʌlnərəˈbɪləti/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Vui lòng đẩy các thay đổi của bạn lên nhánh chính.", EnglishText = "Please push your changes to the main branch.", Phonetic = "/pliːz pʊʃ jɔːr ˈtʃeɪndʒɪz tuː ðə meɪn bræntʃ/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Máy chủ cơ sở dữ liệu hiện tại không phản hồi.", EnglishText = "The database server is currently unresponsive.", Phonetic = "/ðə ˈdeɪtəbeɪs ˈsɜːrvər ɪz ˈkɜːrəntli ˌʌnrɪˈspɑːnsɪv/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Chúng ta cần tích hợp API này trước ngày mai.", EnglishText = "We need to integrate this API before tomorrow.", Phonetic = "/wiː niːd tuː ˈɪntɪɡreɪt ðɪs eɪ-pi-aɪ bɪˈfɔːr təˈmɔːroʊ/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Hệ thống CI/CD bị lỗi khi build.", EnglishText = "The CI/CD pipeline failed during the build process.", Phonetic = "/ðə siː-aɪ siː-diː ˈpaɪplaɪn feɪld ˈdʊrɪŋ ðə bɪld ˈprɑːses/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Bạn đã xem qua tài liệu hướng dẫn API chưa?", EnglishText = "Have you read through the API documentation yet?", Phonetic = "/hæv juː red θruː ðə eɪ-pi-aɪ ˌdɑːkjumənˈteɪʃn jet/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Lỗi này xảy ra do rò rỉ bộ nhớ ở frontend.", EnglishText = "This bug is caused by a memory leak in the frontend.", Phonetic = "/ðɪs bʌɡ ɪz kɔːzd baɪ ə ˈmeməri liːk ɪn ðə ˈfrʌntend/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Chúng ta cần refactor đoạn code phức tạp này.", EnglishText = "We need to refactor this complex piece of code.", Phonetic = "/wiː niːd tuː ˌriːˈfæktər ðɪs ˈkɑːmpleks piːs ɒv koʊd/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Môi trường staging đã sẵn sàng để kiểm thử.", EnglishText = "The staging environment is ready for testing.", Phonetic = "/ðə ˈsteɪdʒɪŋ ɪnˈvaɪrənmənt ɪz ˈredi fɔːr ˈtestɪŋ/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Vui lòng viết unit test cho hàm mới này.", EnglishText = "Please write unit tests for this new function.", Phonetic = "/pliːz raɪt ˈjuːnɪt tests fɔːr ðɪs nuː ˈfʌŋkʃn/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Tôi đang giải quyết xung đột khi merge.", EnglishText = "I am resolving a merge conflict right now.", Phonetic = "/aɪ æm rɪˈzɑːlvɪŋ ə mɜːrdʒ ˈkɑːnflɪkt raɪt naʊ/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Hệ thống tải trang rất chậm do truy vấn SQL nặng.", EnglishText = "The page loading is very slow due to heavy SQL queries.", Phonetic = "/ðə peɪdʒ ˈloʊdɪŋ ɪz ˈveri sloʊ duː tuː ˈhevi ˌes-kjuː-ˈel ˈkwɪriz/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Bạn có thể giải thích logic nghiệp vụ của chức năng này không?", EnglishText = "Could you explain the business logic of this feature?", Phonetic = "/kʊd juː ɪkˈspleɪn ðə ˈbɪznəs ˈlɑːdʒɪk ɒv ðɪs ˈfiːtʃər/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Ứng dụng cần hỗ trợ cả chế độ sáng và tối.", EnglishText = "The application needs to support both light and dark modes.", Phonetic = "/ðiː ˌæplɪˈkeɪʃn niːdz tuː səˈpɔːrt boʊθ laɪt ænd dɑːrk moʊdz/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Chúng ta nên sử dụng caching để giảm tải cho DB.", EnglishText = "We should use caching to reduce the database load.", Phonetic = "/wiː ʃʊd juːz ˈkæʃɪŋ tuː rɪˈduːs ðə ˈdeɪtəbeɪs loʊd/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Hãy cấu hình biến môi trường trong file env.", EnglishText = "Please configure the environmental variables in the env file.", Phonetic = "/pliːz kənˈfɪɡjər ðiː ɪnˌvaɪrənˈmentl ˈweriəblz ɪn ðə env faɪl/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Chức năng này không hoạt động trên trình duyệt Safari.", EnglishText = "This feature does not work on the Safari browser.", Phonetic = "/ðɪs ˈfiːtʃər dʌz nɑːt wɜːrk ɑːn ðə səˈfɑːri ˈbraʊzər/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Tôi sẽ tạo pull request và gán bạn làm reviewer.", EnglishText = "I will create a pull request and assign you as a reviewer.", Phonetic = "/aɪ wɪl kriˈeɪt ə pʊl rɪˈkwest ænd əˈsaɪn juː æz ə rɪˈvjuːər/", Type = "Single" },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Chúng ta cần nâng cấp các thư viện phụ thuộc lên phiên bản mới nhất.", EnglishText = "We need to upgrade the dependencies to their latest versions.", Phonetic = "/wiː niːd tuː ʌpˈɡreɪd ðə dɪˈpendənsiz tuː ðer ˈleɪtɪst ˈvɜːrʒnz/", Type = "Single" },

                // --- HỘI THOẠI (20 câu) ---
                // Hội thoại 4: Daily Standup (ConversationId = 4 - 7 câu)
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Chào mọi người, hôm qua tôi đã hoàn thành xong giao diện trang chủ.", EnglishText = "Hi everyone, yesterday I finished implementing the homepage UI.", Phonetic = "/haɪ ˈevriwʌn, ˈjestərdeɪ aɪ ˈfɪnɪʃt ˈɪmpləmentɪŋ ðə ˈhoʊmpeɪdʒ juː-aɪ/", Type = "Conversation", ConversationId = 4 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Tuyệt vời. Hôm nay cậu dự định làm gì?", EnglishText = "Great. What are you planning to work on today?", Phonetic = "/ɡreɪt. wʌt ɑːr juː ˈplænɪŋ tuː wɜːrk ɒn təˈdeɪ/", Type = "Conversation", ConversationId = 4 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Hôm nay tôi sẽ kết nối API đăng nhập và xử lý state.", EnglishText = "Today I will integrate the login API and handle state management.", Phonetic = "/təˈdeɪ aɪ wɪl ˈɪntɪɡreɪt ðə ˈlɔːɡɪn eɪ-pi-aɪ ænd ˈhændl steɪt ˈmænɪdʒmənt/", Type = "Conversation", ConversationId = 4 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Cậu có gặp khó khăn hay vướng mắc gì không?", EnglishText = "Do you have any blockers or difficulties?", Phonetic = "/duː juː hæv ˈɛni ˈblɑːkərz ɔːr ˈdɪfɪkəltiz/", Type = "Conversation", ConversationId = 4 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Hiện tại thì không, mọi thứ vẫn đang đúng tiến độ.", EnglishText = "Not at the moment, everything is on track.", Phonetic = "/nɑːt æt ðə ˈmoʊmənt, ˈevriwʌn ɪz ɒn træk/", Type = "Conversation", ConversationId = 4 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Nếu cần trợ giúp về API, cứ nhắn tôi nhé.", EnglishText = "If you need help with the API, just text me.", Phonetic = "/ɪf juː niːd help wɪð ðə eɪ-pi-aɪ, dʒʌst tekst mi/", Type = "Conversation", ConversationId = 4 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Cảm ơn cậu, tôi sẽ nhắn nếu có vấn đề gì.", EnglishText = "Thanks, I will let you know if anything comes up.", Phonetic = "/θæŋks, aɪ wɪl let juː noʊ ɪf ˈɛniθɪŋ kʌmz ʌp/", Type = "Conversation", ConversationId = 4 },

                // Hội thoại 5: Báo lỗi Production Bug (ConversationId = 5 - 7 câu)
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Hình như có lỗi nghiêm trọng trên production.", EnglishText = "It seems there is a critical bug on production.", Phonetic = "/ɪt siːmz ðer ɪz ə ˈkrɪtɪkl bʌɡ ɑːn prəˈdʌkʃn/", Type = "Conversation", ConversationId = 5 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Lỗi gì thế? Người dùng không thanh toán được à?", EnglishText = "What bug is it? Can't users make payments?", Phonetic = "/wʌt bʌɡ ɪz ɪt? kænt ˈjuːzərz meɪk ˈpeɪmənts/", Type = "Conversation", ConversationId = 5 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Đúng vậy, cổng thanh toán trả về lỗi 500 liên tục.", EnglishText = "Yes, the payment gateway is returning 500 errors repeatedly.", Phonetic = "/jes, ðə ˈpeɪmənt ˈɡeɪtweɪ ɪz rɪˈtɜːrnɪŋ faɪv-ˈhʌndrəd ˈerərz rɪˈpiːtɪdli/", Type = "Conversation", ConversationId = 5 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Hãy kiểm tra log của server ngay lập tức.", EnglishText = "Check the server logs immediately.", Phonetic = "/tʃek ðə ˈsɜːrvər lɔːɡz ɪˈmiːdiətli/", Type = "Conversation", ConversationId = 5 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Tôi thấy rồi, lỗi kết nối cơ sở dữ liệu do quá tải.", EnglishText = "I see it, a database connection error due to overload.", Phonetic = "/aɪ siː ɪt, ə ˈdeɪtəbeɪs kəˈnekʃn ˈerər duː tuː ˈoʊvərloʊd/", Type = "Conversation", ConversationId = 5 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Cậu có thể tăng dung lượng kết nối trong file config không?", EnglishText = "Can you increase the connection pool size in the config file.", Phonetic = "/kæn juː ɪnˈkriːs ðə kəˈnekʃn puːl saɪz ɪn ðə kənˈfɪɡ faɪl/", Type = "Conversation", ConversationId = 5 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Được rồi, để tôi deploy bản hotfix này lên ngay.", EnglishText = "Alright, let me deploy this hotfix right away.", Phonetic = "/ɔːlˈraɪt, let mi dɪˈplɔɪ ðɪs ˈhɑːtfɪks raɪt əˈweɪ/", Type = "Conversation", ConversationId = 5 },

                // Hội thoại 6: Refactor Code (ConversationId = 6 - 6 câu)
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Đoạn code này nhìn rối rắm và khó bảo trì quá.", EnglishText = "This piece of code looks messy and hard to maintain.", Phonetic = "/ðɪs piːs ɒv koʊd lʊks ˈmesi ænd hɑːrd tuː meɪnˈteɪn/", Type = "Conversation", ConversationId = 6 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Đúng thế, có quá nhiều câu lệnh if-else lồng nhau.", EnglishText = "Indeed, there are too many nested if-else statements.", Phonetic = "/ɪnˈdiːd, ðer ɑːr tuː ˈmʌni ˈnestɪd ɪf-ˈels ˈsteɪtmənts/", Type = "Conversation", ConversationId = 6 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Chúng ta có nên tách nó ra thành các hàm nhỏ hơn không?", EnglishText = "Should we extract it into smaller functions?", Phonetic = "/ʃʊd wiː ɪkˈstrækt ɪt ˈɪntuː ˈsmɔːlər ˈfʌŋkʃnz/", Type = "Conversation", ConversationId = 6 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Ý kiến hay đó, và áp dụng pattern thay thế nữa.", EnglishText = "Good idea, and apply design patterns to replace them.", Phonetic = "/ɡʊd aɪˈdiːə, ænd əˈplaɪ dɪˈzaɪn ˈpætərnz tuː rɪˈpleɪs ðem/", Type = "Conversation", ConversationId = 6 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Cậu có muốn thực hiện việc refactor này cùng tôi không?", EnglishText = "Do you want to pair program this refactoring with me?", Phonetic = "/duː juː wɑːnt tuː per ˈproʊɡræm ðɪs ˌriːˈfæktərɪŋ wɪð mi/", Type = "Conversation", ConversationId = 6 },
                new Sentence { TopicId = topicIt.Id, VietnameseText = "Rất sẵn lòng, chúng ta bắt đầu chiều nay nhé.", EnglishText = "Gladly, let's start this afternoon.", Phonetic = "/ˈɡlædli, lets stɑːrt ðɪs ˌæftərˈnuːn/", Type = "Conversation", ConversationId = 6 }
            };

            await SeedSentencesSafelyAsync(context, itSentences);
        }

        private static async Task SeedSentencesSafelyAsync(AppDbContext context, List<Sentence> sentences)
        {
            foreach (var sentence in sentences)
            {
                // Kiểm tra xem câu hỏi này đã tồn tại trong DB chưa để tránh trùng lặp
                var exists = await context.Sentences.AnyAsync(s => s.VietnameseText == sentence.VietnameseText && s.TopicId == sentence.TopicId);
                if (!exists)
                {
                    context.Sentences.Add(sentence);
                }
                else
                {
                    // Nếu đã tồn tại thì cập nhật lại các thuộc tính mới (Phonetic, Type, ConversationId) để đồng bộ dữ liệu
                    var existing = await context.Sentences.FirstOrDefaultAsync(s => s.VietnameseText == sentence.VietnameseText && s.TopicId == sentence.TopicId);
                    if (existing != null)
                    {
                        existing.Phonetic = sentence.Phonetic;
                        existing.Type = sentence.Type;
                        existing.ConversationId = sentence.ConversationId;
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
