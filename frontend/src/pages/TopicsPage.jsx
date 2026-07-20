import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import api from '../api';
import { FaComments, FaLaptopCode, FaChevronRight } from 'react-icons/fa6';

const getTopicIcon = (name) => {
    switch (name) {
        case 'Giao tiếp hằng ngày':
            return <FaComments className="w-10 h-10 text-[#FB923C]" />;
        case 'Giao tiếp IT':
            return <FaLaptopCode className="w-10 h-10 text-[#60A5FA]" />;
        default:
            return <FaComments className="w-10 h-10 text-gray-400" />;
    }
};

export default function TopicsPage() {
    const [topics, setTopics] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        api.get(`/Practice/topics`)
            .then(res => {
                setTopics(res.data);
                setLoading(false);
            })
            .catch(err => {
                console.error(err);
                setLoading(false);
            });
    }, []);

    return (
        <div className="space-y-6">
            <h2 className="text-2xl font-heading font-bold uppercase tracking-wider text-left text-text-main mb-6">
                Chọn chủ đề luyện tập
            </h2>
            
            <div className="grid gap-6">
                {topics.map(topic => (
                    <Link
                        key={topic.id}
                        to={`/practice/${topic.id}`}
                        className="block p-6 brutalist-card hover:bg-amber-50 dark:hover:bg-[#334155] cursor-pointer"
                    >
                        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                            <div className="flex items-center gap-4">
                                <div className="p-3 brutalist-card bg-bg-card shrink-0 shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] border-2">
                                    {getTopicIcon(topic.name)}
                                </div>
                                <div>
                                    <h3 className="font-heading font-bold text-xl text-text-main mb-1">
                                        {topic.name}
                                    </h3>
                                    <p className="text-sm font-medium text-gray-700 dark:text-gray-300">
                                        {topic.description}
                                    </p>
                                </div>
                            </div>
                            <div className="shrink-0 self-start sm:self-center">
                                <span className="brutalist-btn bg-[#FDE047] text-black text-xs py-1.5 px-3 uppercase tracking-wider font-bold flex items-center gap-1.5">
                                    Luyện tập <FaChevronRight className="w-3 h-3" />
                                </span>
                            </div>
                        </div>
                    </Link>
                ))}
            </div>

            {loading && (
                <div className="p-8 brutalist-card bg-[#FDE047]/10 dark:bg-[#60A5FA]/10 flex flex-col items-center justify-center">
                    <div className="w-8 h-8 border-3 border-border-main border-t-transparent rounded-full animate-spin mb-3"></div>
                    <span className="font-heading font-bold text-sm tracking-wide">
                        ĐANG TẢI CHỦ ĐỀ...
                    </span>
                </div>
            )}

            {!loading && topics.length === 0 && (
                <div className="p-8 brutalist-card bg-[#FFE4E6] dark:bg-[#EF4444]/20 text-center">
                    <span className="font-heading font-bold text-sm">
                        Không tìm thấy chủ đề nào. Vui lòng kiểm tra lại kết nối cơ sở dữ liệu!
                    </span>
                </div>
            )}
        </div>
    );
}