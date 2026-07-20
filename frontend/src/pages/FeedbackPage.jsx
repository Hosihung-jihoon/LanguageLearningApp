import { useState, useEffect } from 'react';
import { useLocation, Link } from 'react-router-dom';
import ReactMarkdown from 'react-markdown';
import api from '../api';
import { FaMedal, FaCircleExclamation, FaHouse } from 'react-icons/fa6';

export default function FeedbackPage() {
    const location = useLocation();
    const { attemptIds } = location.state || {};

    const [recommendation, setRecommendation] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        if (attemptIds && attemptIds.length > 0) {
            api.post('/Practice/session-feedback', { attemptIds })
                .then(res => {
                    setRecommendation(res.data.recommendations);
                    setIsLoading(false);
                })
                .catch(err => {
                    console.error(err);
                    if (err.response && err.response.status === 429) {
                        setError(err.response.data.error || "Bạn đã vượt quá giới hạn gọi API Gemini.");
                    } else {
                        setError("Không thể tải đánh giá từ AI lúc này.");
                    }
                    setIsLoading(false);
                });
        } else {
            setIsLoading(false);
        }
    }, [attemptIds]);

    const handleRetry = () => {
        setIsLoading(true);
        setError(null);
        api.post('/Practice/session-feedback', { attemptIds })
            .then(res => {
                setRecommendation(res.data.recommendations);
                setIsLoading(false);
            })
            .catch(err => {
                console.error(err);
                if (err.response && err.response.status === 429) {
                    setError(err.response.data.error || "Bạn đã vượt quá giới hạn gọi API Gemini.");
                } else {
                    setError("Không thể tải đánh giá từ AI lúc này.");
                }
                setIsLoading(false);
            });
    };

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-heading font-bold uppercase tracking-wider text-left text-text-main mb-6 flex items-center gap-2.5">
                <FaMedal className="w-7 h-7 text-[#FB923C]" />
                Tổng kết buổi học
            </h2>

            {isLoading ? (
                <div className="p-8 brutalist-card bg-[#FDE047]/10 dark:bg-[#60A5FA]/10 flex flex-col items-center justify-center">
                    <div className="w-8 h-8 border-3 border-border-main border-t-transparent rounded-full animate-spin mb-3"></div>
                    <span className="font-heading font-bold text-sm tracking-wide uppercase">
                        AI đang phân tích lỗi sai của bạn...
                    </span>
                </div>
            ) : error ? (
                <div className="p-8 brutalist-card bg-[#FFE4E6] dark:bg-red-950/20 text-text-main text-left flex flex-col gap-2">
                    <h4 className="font-heading font-bold uppercase text-lg text-red-600 dark:text-red-400 flex items-center gap-1.5 m-0">
                        <FaCircleExclamation className="w-5 h-5 text-red-600 dark:text-red-400" />
                        Không thể phân tích kết quả
                    </h4>
                    <p className="text-sm font-bold">{error}</p>
                    <button
                        onClick={handleRetry}
                        className="brutalist-btn bg-[#FB923C] text-xs font-bold uppercase py-1.5 px-3 mt-2 self-start text-black cursor-pointer"
                    >
                        Thử lại
                    </button>
                </div>
            ) : (
                <div className="p-8 brutalist-card bg-bg-card text-text-main shadow-[4px_4px_0px_0px_rgba(0,0,0,1)] border-2">
                    <div className="prose dark:prose-invert max-w-none text-left font-body text-base leading-relaxed space-y-4">
                        <ReactMarkdown>{recommendation}</ReactMarkdown>
                    </div>
                </div>
            )}

            <div className="pt-4 flex gap-4">
                <Link
                    to="/"
                    className="brutalist-btn bg-[#FDE047] text-black text-base uppercase tracking-wider font-heading font-bold flex items-center gap-2"
                >
                    <FaHouse className="w-4 h-4" />
                    Về trang chủ
                </Link>
            </div>
        </div>
    );
}