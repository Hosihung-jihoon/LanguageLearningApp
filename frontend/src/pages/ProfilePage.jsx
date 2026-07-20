import { useState, useEffect } from 'react';
import ReactMarkdown from 'react-markdown';
import api from '../api';
import { 
    FaClock, 
    FaPenToSquare, 
    FaRegCircleCheck, 
    FaPercent, 
    FaChartSimple, 
    FaBrain, 
    FaClockRotateLeft, 
    FaLightbulb, 
    FaVolumeHigh, 
    FaRotateRight 
} from 'react-icons/fa6';

export default function ProfilePage() {
    const [stats, setStats] = useState(null);
    const [loading, setLoading] = useState(true);
    const [recommendations, setRecommendations] = useState('');
    const [recLoading, setRecLoading] = useState(true);
    const [recError, setRecError] = useState(null);

    const loadRecommendations = () => {
        setRecLoading(true);
        setRecError(null);
        api.get('/Practice/improvement-recommendations')
            .then(res => {
                setRecommendations(res.data.recommendations);
                setRecLoading(false);
            })
            .catch(err => {
                console.error(err);
                if (err.response && err.response.status === 429) {
                    setRecError(err.response.data.error || "Bạn đã vượt quá giới hạn gọi API Gemini.");
                } else {
                    setRecError("Không thể tải đề xuất phân tích kỹ năng từ AI lúc này.");
                }
                setRecLoading(false);
            });
    };

    useEffect(() => {
        api.get('/Practice/stats')
            .then(res => {
                setStats(res.data);
                setLoading(false);
            })
            .catch(err => {
                console.error(err);
                setLoading(false);
            });

        loadRecommendations();
    }, []);

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

    if (loading) {
        return (
            <div className="p-8 brutalist-card bg-[#FDE047]/10 dark:bg-[#60A5FA]/10 flex flex-col items-center justify-center">
                <div className="w-8 h-8 border-3 border-border-main border-t-transparent rounded-full animate-spin mb-3"></div>
                <span className="font-heading font-bold text-sm tracking-wide">
                    ĐANG TẢI THỐNG KÊ...
                </span>
            </div>
        );
    }

    if (!stats || stats.totalAttempts === 0) {
        return (
            <div className="space-y-6">
                <h2 className="text-2xl font-heading font-bold uppercase tracking-wider text-left text-text-main mb-6">
                    Trang cá nhân
                </h2>
                <div className="p-8 brutalist-card bg-[#FDE047]/10 text-center">
                    <p className="font-heading font-bold text-lg mb-2">Chưa có dữ liệu học tập!</p>
                    <p className="text-sm font-medium">Hãy tham gia luyện tập dịch câu để bắt đầu lưu lại tiến trình học của bạn.</p>
                </div>
            </div>
        );
    }

    // Tính toán để hiển thị biểu đồ phân tích lỗi
    const breakdown = Object.entries(stats.mistakeBreakdown).map(([type, count]) => ({
        type,
        count,
        percentage: Math.round((count / stats.totalAttempts) * 100)
    })).sort((a, b) => b.count - a.count);

    const usage = stats?.geminiUsage || { currentRpm: 0, currentRpd: 0, maxRpm: 15, maxRpd: 1500 };
    
    let statusText = "Bình thường";
    let statusColor = "bg-[#4ADE80] text-black"; // green
    if (usage.currentRpm >= usage.maxRpm || usage.currentRpd >= usage.maxRpd) {
        statusText = "Đạt giới hạn";
        statusColor = "bg-[#F472B6] text-black"; // red/pink
    } else if (usage.currentRpm >= usage.maxRpm * 0.8 || usage.currentRpd >= usage.maxRpd * 0.8) {
        statusText = "Sắp quá tải";
        statusColor = "bg-[#FB923C] text-black"; // orange
    }

    return (
        <div className="space-y-8">
            <h2 className="text-2xl font-heading font-bold uppercase tracking-wider text-left text-text-main">
                Trang cá nhân
            </h2>

            {/* Giới hạn API Gemini (Free Tier) */}
            <div className="p-6 brutalist-card bg-[#FDE047] text-black text-left shadow-[4px_4px_0px_0px_rgba(0,0,0,1)] border-2">
                <h3 className="text-lg font-heading font-bold uppercase tracking-wider mb-4 flex items-center justify-between gap-2 m-0 text-black flex-wrap">
                    <span className="flex items-center gap-2">
                        <FaClock className="w-5 h-5 text-black" />
                        Giới hạn API Gemini (Free Tier)
                    </span>
                    <span className={`px-2.5 py-0.5 border-2 border-black rounded-md text-[10px] uppercase font-black shadow-[1.5px_1.5px_0px_0px_rgba(0,0,0,1)] ${statusColor}`}>
                        Trạng thái: {statusText}
                    </span>
                </h3>
                <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 text-sm font-bold">
                    <div className="p-3 bg-white border-2 border-black rounded-lg shadow-[2px_2px_0px_0px_rgba(0,0,0,1)]">
                        <span className="block text-xs uppercase text-gray-500 mb-1">Mỗi phút (RPM)</span>
                        <span className="text-base text-black">{usage.currentRpm} / {usage.maxRpm} requests</span>
                    </div>
                    <div className="p-3 bg-white border-2 border-black rounded-lg shadow-[2px_2px_0px_0px_rgba(0,0,0,1)]">
                        <span className="block text-xs uppercase text-gray-500 mb-1">Mỗi ngày (RPD)</span>
                        <span className="text-base text-black">{usage.currentRpd} / {usage.maxRpd} requests</span>
                    </div>
                    <div className="p-3 bg-white border-2 border-black rounded-lg shadow-[2px_2px_0px_0px_rgba(0,0,0,1)]">
                        <span className="block text-xs uppercase text-gray-500 mb-1">Thời gian reset</span>
                        <span className="text-base text-black">14:00 (Giờ Việt Nam)</span>
                    </div>
                </div>
            </div>

            {/* Thống kê chung */}
            <div className="grid grid-cols-1 sm:grid-cols-3 gap-6">
                <div className="p-6 brutalist-card bg-[#60A5FA] text-black shadow-[4px_4px_0px_0px_rgba(0,0,0,1)] border-2">
                    <span className="text-xs uppercase font-bold tracking-wider mb-2 flex items-center gap-1.5">
                        <FaPenToSquare className="w-4 h-4" /> Tổng câu đã làm
                    </span>
                    <span className="text-4xl font-heading font-bold">{stats.totalAttempts}</span>
                </div>
                <div className="p-6 brutalist-card bg-[#4ADE80] text-black shadow-[4px_4px_0px_0px_rgba(0,0,0,1)] border-2">
                    <span className="text-xs uppercase font-bold tracking-wider mb-2 flex items-center gap-1.5">
                        <FaRegCircleCheck className="w-4 h-4" /> Dịch chính xác
                    </span>
                    <span className="text-4xl font-heading font-bold">{stats.correctCount}</span>
                </div>
                <div className="p-6 brutalist-card bg-[#FDE047] text-black shadow-[4px_4px_0px_0px_rgba(0,0,0,1)] border-2">
                    <span className="text-xs uppercase font-bold tracking-wider mb-2 flex items-center gap-1.5">
                        <FaPercent className="w-4 h-4" /> Tỷ lệ chính xác
                    </span>
                    <span className="text-4xl font-heading font-bold">{stats.accuracyRate}%</span>
                </div>
            </div>

            {/* Phân tích loại lỗi */}
            <div className="p-6 brutalist-card bg-bg-card shadow-[4px_4px_0px_0px_rgba(0,0,0,1)] border-2">
                <h3 className="text-lg font-heading font-bold uppercase tracking-wider mb-6 text-text-main flex items-center gap-2">
                    <FaChartSimple className="w-5 h-5 text-[#FB923C]" />
                    Phân tích loại lỗi mắc phải
                </h3>
                <div className="space-y-4">
                    {breakdown.map((item, index) => (
                        <div key={index} className="space-y-1">
                            <div className="flex justify-between text-sm font-bold">
                                <span className="uppercase text-text-main">{item.type === 'None' ? 'Không lỗi (Chính xác)' : item.type}</span>
                                <span className="text-text-main">{item.count} lần ({item.percentage}%)</span>
                            </div>
                            <div className="w-full bg-gray-200 dark:bg-[#334155] border-2 border-border-main rounded-full h-5 overflow-hidden">
                                <div
                                    className="h-full border-r-2 border-border-main transition-all duration-500"
                                    style={{
                                        width: `${item.percentage}%`,
                                        backgroundColor: item.type === 'None' ? '#4ADE80' : item.type === 'Grammar' ? '#FB923C' : item.type === 'Vocabulary' ? '#F472B6' : '#60A5FA'
                                    }}
                                ></div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>

            {/* Kỹ năng cần cải thiện (AI Phân tích chi tiết) */}
            <div className="p-6 brutalist-card bg-bg-card shadow-[4px_4px_0px_0px_rgba(0,0,0,1)] border-2">
                <h3 className="text-lg font-heading font-bold uppercase tracking-wider mb-4 text-text-main flex items-center gap-2">
                    <FaBrain className="w-5 h-5 text-[#FB923C]" />
                    Kỹ năng cần cải thiện (AI Phân tích)
                </h3>
                {recLoading ? (
                    <div className="p-6 bg-[#FB923C]/10 dark:bg-[#FB923C]/5 border-2 border-dashed border-border-main rounded-xl flex flex-col items-center justify-center animate-pulse">
                        <div className="w-6 h-6 border-2 border-border-main border-t-transparent rounded-full animate-spin mb-2"></div>
                        <span className="text-xs font-bold uppercase tracking-wider">AI đang phân tích điểm yếu của bạn...</span>
                    </div>
                ) : recError ? (
                    <div className="p-6 bg-[#FFE4E6] dark:bg-red-950/20 border-2 border-dashed border-red-500 rounded-xl text-left flex flex-col gap-2">
                        <span className="text-sm font-bold text-red-600 dark:text-red-400">Không thể tải phân tích kĩ năng</span>
                        <p className="text-xs font-semibold text-text-main">{recError}</p>
                        <button
                            onClick={loadRecommendations}
                            className="brutalist-btn bg-[#FB923C] text-xs font-bold uppercase py-1 px-3 self-start text-black cursor-pointer flex items-center gap-1"
                        >
                            <FaRotateRight className="w-3 h-3" />
                            Thử lại
                        </button>
                    </div>
                ) : (
                    <div className="ai-recommendations prose dark:prose-invert max-w-none text-left font-body text-base leading-relaxed space-y-2">
                        <ReactMarkdown>{recommendations}</ReactMarkdown>
                    </div>
                )}
            </div>

            {/* Lịch sử làm bài */}
            <div className="space-y-4">
                <h3 className="text-xl font-heading font-bold uppercase tracking-wider text-left text-text-main flex items-center gap-2">
                    <FaClockRotateLeft className="w-5 h-5 text-[#FB923C]" />
                    Lịch sử làm bài gần đây
                </h3>
                <div className="space-y-4">
                    {stats.history.map((attempt) => (
                        <div key={attempt.id} className="p-4 brutalist-card bg-bg-card flex flex-col sm:flex-row sm:items-center justify-between gap-4 shadow-[3px_3px_0px_0px_rgba(0,0,0,1)] border-2">
                            <div className="space-y-1 text-left min-w-0 flex-1">
                                <div className="flex items-center gap-2 flex-wrap">
                                    <span className="text-xs font-bold text-gray-500 dark:text-gray-400">
                                        {new Date(attempt.createdAt).toLocaleDateString('vi-VN', {
                                            hour: '2-digit',
                                            minute: '2-digit',
                                            day: '2-digit',
                                            month: '2-digit'
                                        })}
                                    </span>
                                    <span className={`inline-block px-2 py-0.5 border border-border-main rounded-md text-[10px] uppercase font-bold ${attempt.isCorrect
                                            ? 'bg-[#DCFCE7] text-green-800 dark:bg-green-900/30 dark:text-green-300'
                                            : 'bg-[#FFE4E6] text-red-800 dark:bg-red-900/30 dark:text-red-300'
                                        }`}>
                                        {attempt.isCorrect ? 'Đúng' : `Sai: ${attempt.mistakeType}`}
                                    </span>
                                </div>
                                <p className="font-heading font-bold text-base text-text-main break-words">
                                    {attempt.vietnameseText}
                                </p>
                                <p className="font-body text-sm text-gray-700 dark:text-gray-300 italic break-words mb-2">
                                    Dịch của bạn: "{attempt.userTranslation}"
                                </p>
                                {attempt.correctedSentence && (
                                    <div className="mt-2 p-2.5 border-2 border-dashed border-[#4ADE80] bg-[#4ADE80]/15 dark:bg-[#4ADE80]/5 rounded-lg text-xs font-semibold text-text-main flex flex-col sm:flex-row sm:items-center justify-between gap-2">
                                        <span className="break-words flex-1 leading-relaxed flex items-center gap-1.5 flex-wrap">
                                            <FaLightbulb className="w-3.5 h-3.5 text-yellow-500 shrink-0" />
                                            <span>Gợi ý dịch: <strong className="font-bold text-black dark:text-white font-heading">"{attempt.correctedSentence}"</strong></span>
                                        </span>
                                        <button
                                            onClick={() => handleSpeak(attempt.correctedSentence)}
                                            className="p-1 brutalist-btn bg-[#4ADE80] hover:bg-[#22C55E] shrink-0 self-start sm:self-center cursor-pointer flex items-center justify-center"
                                            title="Nghe câu dịch đề xuất"
                                        >
                                            <FaVolumeHigh className="w-3.5 h-3.5 text-black" />
                                        </button>
                                    </div>
                                )}
                            </div>

                            <div className="flex items-center gap-2 self-end sm:self-center shrink-0">
                                <button
                                    onClick={() => handleSpeak(attempt.userTranslation)}
                                    className="p-2.5 brutalist-btn bg-[#60A5FA] hover:bg-[#3B82F6] cursor-pointer flex items-center justify-center"
                                    title="Nghe câu bạn dịch"
                                >
                                    <FaVolumeHigh className="w-4 h-4 text-black" />
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}
