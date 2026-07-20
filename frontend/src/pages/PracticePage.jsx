import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../api';
import { 
    FaComments, 
    FaFileLines, 
    FaCircleCheck, 
    FaCircleXmark, 
    FaCircleExclamation, 
    FaVolumeHigh, 
    FaUser, 
    FaCheck, 
    FaChevronRight,
    FaRightFromBracket
} from 'react-icons/fa6';

export default function PracticePage() {
    const { topicId } = useParams();
    const navigate = useNavigate();

    const [practiceMode, setPracticeMode] = useState(null);
    const [loadingSentences, setLoadingSentences] = useState(false);
    const [fetchError, setFetchError] = useState(null);
    const [currentPhonetic, setCurrentPhonetic] = useState(null);
    const [chatHistory, setChatHistory] = useState([]);

    const [sentence, setSentence] = useState([]);
    const [currentIndex, setCurrentIndex] = useState(0);
    const [attemptIds, setAttemptIds] = useState([]);

    const [userTranslation, setUserTranslation] = useState("");
    const [feedback, setFeedback] = useState(null);
    const [isloading, setIsLoading] = useState(false);
    const [rateLimitError, setRateLimitError] = useState(null);

    useEffect(() => {
        if (!practiceMode) return;
        setLoadingSentences(true);
        setFetchError(null);
        api.get(`/Practice/sentences/${topicId}?type=${practiceMode}&count=20`) // Lấy 20 câu theo yêu cầu
            .then(res => {
                setSentence(res.data);
                setLoadingSentences(false);
            })
            .catch(err => {
                console.error(err);
                setFetchError(err.response?.data || "Không thể tải câu hỏi. Vui lòng thử lại!");
                setLoadingSentences(false);
            });
    }, [topicId, practiceMode]);

    const handleSpeak = (text) => {
        if ('speechSynthesis' in window) {
            window.speechSynthesis.cancel();
            const utterance = new SpeechSynthesisUtterance(text);
            utterance.lang = 'en-US';
            const voices = window.speechSynthesis.getVoices();
            const englishVoice = voices.find(v => v.lang.startsWith('en'));
            if (englishVoice) {
                utterance.voice = englishVoice;
            }
            window.speechSynthesis.speak(utterance);
        } else {
            alert("Trình duyệt không hỗ trợ Text-To-Speech.");
        }
    };

    const handleSubmit = async () => {
        if (!userTranslation.trim()) return;
        setIsLoading(true);
        setFeedback(null);
        setRateLimitError(null);
        setCurrentPhonetic(null);

        try {
            const res = await api.post('/Practice/submit', {
                sentenceId: sentence[currentIndex].id,
                userTranslation: userTranslation
            });

            setFeedback(res.data.feedback);
            setAttemptIds(prev => [...prev, res.data.attemptId]);
            setCurrentPhonetic(res.data.phonetic);
        } catch (err) {
            console.error(err);
            if (err.response && err.response.status === 429) {
                setRateLimitError(err.response.data.error || "Bạn đã vượt quá giới hạn gọi API Gemini.");
            } else {
                alert("Có lỗi xảy ra khi gửi câu trả lời.");
            }
        } finally {
            setIsLoading(false);
        }
    };

    const handleNext = () => {
        if (practiceMode === 'Conversation' && feedback) {
            setChatHistory(prev => [...prev, {
                vietnameseText: sentence[currentIndex].vietnameseText,
                correctedSentence: feedback.corrected_sentence,
                phonetic: currentPhonetic,
                userTranslation: userTranslation,
                isCorrect: feedback.is_correct
            }]);
        }

        if (currentIndex < sentence.length - 1) {
            setCurrentIndex(prev => prev + 1);
            setUserTranslation("");
            setFeedback(null);
            setCurrentPhonetic(null);
        } else {
            // Hoàn thành set học
            navigate("/feedback", { state: { attemptIds: attemptIds } });
        }
    };

    const handleFinishEarly = () => {
        if (attemptIds.length > 0) {
            if (window.confirm("Bạn muốn kết thúc buổi học sớm và xem đánh giá AI cho các câu đã hoàn thành chứ?")) {
                navigate("/feedback", { state: { attemptIds: attemptIds } });
            }
        } else {
            navigate("/");
        }
    };

    if (!practiceMode) {
        return (
            <div className="space-y-6 max-w-md mx-auto py-8">
                <h2 className="text-2xl font-heading font-bold uppercase tracking-wider text-center text-text-main mb-2">
                    Chọn chế độ luyện tập
                </h2>
                <div className="grid gap-6">
                    <button
                        onClick={() => setPracticeMode("Single")}
                        className="p-6 brutalist-card bg-[#FDE047] hover:bg-amber-300 dark:bg-[#FDE047]/80 dark:hover:bg-amber-500 text-left transition-colors cursor-pointer"
                    >
                        <h3 className="font-heading font-bold text-xl text-black mb-2 flex items-center gap-2">
                            <FaFileLines className="w-6 h-6" /> LUYỆN CÂU ĐƠN
                        </h3>
                        <p className="text-sm font-medium text-gray-800 dark:text-gray-900">
                            Luyện dịch ngẫu nhiên các câu đơn lẻ trong chủ đề để tích lũy phản xạ và từ vựng nhanh.
                        </p>
                    </button>

                    <button
                        onClick={() => setPracticeMode("Conversation")}
                        className="p-6 brutalist-card bg-[#60A5FA] hover:bg-blue-400 dark:bg-[#60A5FA]/80 dark:hover:bg-blue-500 text-left transition-colors cursor-pointer"
                    >
                        <h3 className="font-heading font-bold text-xl text-black mb-2 flex items-center gap-2">
                            <FaComments className="w-6 h-6" /> LUYỆN HỘI THOẠI
                        </h3>
                        <p className="text-sm font-medium text-gray-800 dark:text-gray-900">
                            Dịch một đoạn hội thoại hoàn chỉnh theo ngữ cảnh để luyện tư duy mạch lạc và giao tiếp thực tế.
                        </p>
                    </button>
                </div>
            </div>
        );
    }

    if (fetchError) {
        return (
            <div className="p-8 brutalist-card bg-[#FFE4E6] dark:bg-[#EF4444]/20 text-center space-y-4">
                <span className="font-heading font-bold text-sm block text-red-600 dark:text-red-400">
                    {fetchError}
                </span>
                <div className="flex gap-4 justify-center">
                    <button
                        onClick={() => setPracticeMode(null)}
                        className="brutalist-btn bg-[#60A5FA] text-black text-xs py-1.5 px-3 uppercase tracking-wider font-bold cursor-pointer flex items-center gap-1"
                    >
                        Quay lại chọn chế độ
                    </button>
                    <button
                        onClick={() => navigate("/")}
                        className="brutalist-btn bg-gray-400 text-black text-xs py-1.5 px-3 uppercase tracking-wider font-bold cursor-pointer"
                    >
                        Về trang chủ
                    </button>
                </div>
            </div>
        );
    }

    if (loadingSentences || sentence.length === 0) {
        return (
            <div className="p-8 brutalist-card bg-[#FDE047]/10 dark:bg-[#60A5FA]/10 flex flex-col items-center justify-center">
                <div className="w-8 h-8 border-3 border-border-main border-t-transparent rounded-full animate-spin mb-3"></div>
                <span className="font-heading font-bold text-sm tracking-wide">
                    ĐANG TẢI CÂU HỎI...
                </span>
            </div>
        );
    }

    const currentSentence = sentence[currentIndex];

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center gap-4 flex-wrap">
                <span className="inline-block px-3 py-1.5 brutalist-card bg-[#FB923C] text-black text-xs font-heading font-bold uppercase tracking-wider">
                    {practiceMode === 'Conversation' ? 'Hội thoại' : 'Câu hỏi'} {currentIndex + 1} / {sentence.length}
                </span>

                <button
                    onClick={handleFinishEarly}
                    className="brutalist-btn bg-[#FFE4E6] dark:bg-red-950/20 text-text-main text-xs py-1.5 px-3 uppercase tracking-wider font-bold cursor-pointer flex items-center gap-1.5"
                    title="Kết thúc buổi học sớm và xem kết quả"
                >
                    <FaRightFromBracket className="w-3.5 h-3.5 text-red-600 dark:text-red-400" />
                    Kết thúc sớm
                </button>
            </div>

            {/* Chat History block for conversation mode */}
            {practiceMode === 'Conversation' && chatHistory.length > 0 && (
                <div className="p-4 brutalist-card bg-amber-50/50 dark:bg-slate-800/30 max-h-[250px] overflow-y-auto space-y-4 mb-2 flex flex-col gap-2">
                    <div className="text-xs text-gray-500 dark:text-gray-400 font-bold uppercase tracking-wider sticky top-0 bg-transparent mb-1">
                        Hội thoại đã dịch:
                    </div>
                    {chatHistory.map((chat, idx) => (
                        <div key={idx} className="flex flex-col items-start max-w-[90%] brutalist-card p-3 bg-bg-card text-sm border-2">
                            <div className="flex items-center gap-1.5 font-bold text-gray-800 dark:text-gray-200">
                                <FaUser className="w-3.5 h-3.5 text-blue-500" />
                                <span>{chat.vietnameseText}</span>
                            </div>
                            <div className="mt-1 pt-1.5 border-t border-dashed border-border-main w-full text-xs font-medium text-gray-600 dark:text-gray-300">
                                <div className="flex items-center gap-1.5 font-semibold text-green-700 dark:text-green-400">
                                    <FaCheck className="w-3 h-3 text-green-600" />
                                    <span>{chat.correctedSentence}</span>
                                </div>
                                {chat.phonetic && (
                                    <p className="text-amber-600 dark:text-amber-500 font-bold mt-0.5 ml-4">{chat.phonetic}</p>
                                )}
                            </div>
                        </div>
                    ))}
                </div>
            )}

            <div className="p-6 brutalist-card">
                <span className="text-xs text-gray-500 dark:text-gray-400 font-bold uppercase tracking-wider block mb-2">
                    Hãy dịch câu sau sang Tiếng Anh:
                </span>
                <p className="text-xl font-heading font-bold text-text-main leading-relaxed">
                    {currentSentence.vietnameseText}
                </p>
            </div>

            <textarea
                className="w-full p-4 brutalist-input font-body text-base outline-none min-h-[120px]"
                placeholder="Nhập câu trả lời của bạn..."
                value={userTranslation}
                onChange={(e) => setUserTranslation(e.target.value)}
                disabled={isloading || feedback || rateLimitError}
            />

            {rateLimitError && (
                <div className="p-4 brutalist-card bg-[#FFE4E6] dark:bg-red-950/20 border-2 border-red-500 text-text-main text-left flex flex-col gap-2">
                    <h4 className="font-heading font-bold uppercase text-sm text-red-600 dark:text-red-400 flex items-center gap-1.5">
                        <FaCircleExclamation className="w-4 h-4" />
                        Đã đạt giới hạn gọi API
                    </h4>
                    <p className="text-xs font-bold">{rateLimitError}</p>
                    <button
                        onClick={() => { setRateLimitError(null); }}
                        className="brutalist-btn bg-[#FDE047] text-black text-xs py-1 px-3 mt-1 uppercase font-bold self-start cursor-pointer"
                    >
                        Đã hiểu & Thử lại
                    </button>
                </div>
            )}

            {!feedback ? (
                <button
                    onClick={handleSubmit}
                    disabled={isloading || !userTranslation.trim() || rateLimitError}
                    className="w-full brutalist-btn bg-[#4ADE80] text-black text-lg uppercase tracking-wider font-heading font-bold py-3.5 cursor-pointer"
                >
                    {isloading ? (
                        <div className="flex items-center justify-center gap-2">
                            <div className="w-5 h-5 border-2 border-black border-t-transparent rounded-full animate-spin"></div>
                            <span>AI đang chấm điểm...</span>
                        </div>
                    ) : (
                        'Nộp bài'
                    )}
                </button>
            ) : (
                <div className="space-y-6">
                    {/* Hiển thị FeedBack */}
                    <div className={`p-6 brutalist-card ${feedback.is_correct ? 'bg-[#DCFCE7] dark:bg-[#064E3B]/40' : 'bg-[#FFE4E6] dark:bg-[#9F1239]/40'}`}>
                        <h3 className={`font-heading font-bold text-xl uppercase tracking-wider mb-4 flex items-center gap-2 ${feedback.is_correct ? 'text-green-800 dark:text-green-300' : 'text-red-800 dark:text-red-300'}`}>
                            {feedback.is_correct ? (
                                <>
                                    <FaCircleCheck className="w-6 h-6 text-green-700 dark:text-green-400" />
                                    <span>Chính xác!</span>
                                </>
                            ) : (
                                <>
                                    <FaCircleXmark className="w-6 h-6 text-red-700 dark:text-red-400" />
                                    <span>Chưa chính xác</span>
                                </>
                            )}
                        </h3>
                        
                        <div className="space-y-4">
                            <div className="p-4 brutalist-card bg-bg-card border-2 flex items-center justify-between gap-4">
                                <div className="min-w-0 flex-1">
                                    <span className="text-xs text-gray-500 dark:text-gray-400 font-bold uppercase tracking-wide block mb-1">
                                        Câu sửa / Dịch đúng nhất:
                                    </span>
                                    <p className="font-heading font-bold text-lg text-text-main break-words">
                                        {feedback.corrected_sentence}
                                    </p>
                                    {currentPhonetic && (
                                        <p className="text-sm font-bold text-amber-600 dark:text-amber-500 mt-1 select-all font-body">
                                            {currentPhonetic}
                                        </p>
                                    )}
                                </div>
                                <button
                                    onClick={() => handleSpeak(feedback.corrected_sentence)}
                                    className="p-2.5 brutalist-btn bg-[#FDE047] hover:bg-[#FACC15] shrink-0 cursor-pointer flex items-center justify-center"
                                    title="Nghe phát âm"
                                >
                                    <FaVolumeHigh className="w-5 h-5 text-black" />
                                </button>
                            </div>

                            {feedback.mistake_explanation && (
                                <div>
                                    <span className="text-xs text-gray-500 dark:text-gray-400 font-bold uppercase tracking-wide block mb-1">
                                        Giải thích lỗi:
                                    </span>
                                    <p className="text-sm font-medium text-text-main leading-relaxed">
                                        {feedback.mistake_explanation}
                                    </p>
                                </div>
                            )}

                            {feedback.natural_alternatives && feedback.natural_alternatives.length > 0 && (
                                <div className="pt-2 border-t border-dashed border-border-main">
                                    <span className="text-xs text-gray-500 dark:text-gray-400 font-bold uppercase tracking-wide block mb-2">
                                        Cách nói tự nhiên khác:
                                    </span>
                                    <ul className="space-y-3">
                                        {feedback.natural_alternatives.map((alt, i) => (
                                            <li key={i} className="text-sm font-medium flex items-center justify-between gap-4 p-2 brutalist-card bg-bg-card border border-border-main">
                                                <div className="flex items-start gap-2 min-w-0">
                                                    <span className="text-[#FB923C] font-bold shrink-0">•</span>
                                                    <span className="break-words">{alt}</span>
                                                </div>
                                                <button
                                                    onClick={() => handleSpeak(alt)}
                                                    className="p-1.5 brutalist-btn bg-[#60A5FA] hover:bg-[#3B82F6] shrink-0 cursor-pointer flex items-center justify-center"
                                                    title="Nghe phát âm"
                                                >
                                                    <FaVolumeHigh className="w-4 h-4 text-black" />
                                                </button>
                                            </li>
                                        ))}
                                    </ul>
                                </div>
                            )}
                        </div>
                    </div>

                    {/* Nút Next */}
                    <button
                        onClick={handleNext}
                        className="w-full brutalist-btn bg-[#60A5FA] text-black text-lg uppercase tracking-wider font-heading font-bold py-3.5 cursor-pointer flex items-center justify-center gap-2"
                    >
                        {currentIndex < sentence.length - 1 ? (
                            <>
                                <span>Câu tiếp theo</span>
                                <FaChevronRight className="w-4 h-4" />
                            </>
                        ) : (
                            'Xem tổng kết'
                        )}
                    </button>
                </div>
            )}
        </div>
    );
}