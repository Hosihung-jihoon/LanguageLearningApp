import { useState, useEffect } from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import TopicsPage from './pages/TopicsPage';
import PracticePage from './pages/PracticePage';
import FeedbackPage from './pages/FeedbackPage';
import ProfilePage from './pages/ProfilePage';
import { LuBookOpen, LuUser, LuSun, LuMoon } from 'react-icons/lu';

function App() {
  const [theme, setTheme] = useState(() => {
    return localStorage.getItem('theme') || 'light';
  });

  useEffect(() => {
    if (theme === 'dark') {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
    localStorage.setItem('theme', theme);
  }, [theme]);

  const toggleTheme = () => {
    setTheme(prev => (prev === 'light' ? 'dark' : 'light'));
  };

  return (
    <div className="min-h-screen pb-12 transition-colors duration-200">
      <BrowserRouter>
        <header className="sticky top-0 z-50 bg-bg-card border-b-3 border-border-main transition-colors duration-200 mb-8">
          <div className="max-w-2xl mx-auto px-4 py-4 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
            <Link to="/" className="text-2xl font-heading font-bold uppercase tracking-wider text-text-main m-0 no-underline hover:opacity-80">
              English Practice
            </Link>

            <div className="flex items-center justify-end gap-3 flex-wrap">
              <Link
                to="/"
                className="brutalist-btn bg-[#60A5FA] text-black text-xs py-1.5 px-3 uppercase tracking-wider font-bold no-underline flex items-center gap-1.5"
              >
                <LuBookOpen className="w-3.5 h-3.5" />
                Chủ đề
              </Link>
              <Link
                to="/profile"
                className="brutalist-btn bg-[#FB923C] text-black text-xs py-1.5 px-3 uppercase tracking-wider font-bold no-underline flex items-center gap-1.5"
              >
                <LuUser className="w-3.5 h-3.5" />
                Trang cá nhân
              </Link>

              <button
                onClick={toggleTheme}
                className="p-1.5 brutalist-btn bg-[#FDE047] dark:bg-[#A7F3D0] cursor-pointer flex items-center justify-center"
                aria-label="Toggle theme"
              >
                {theme === 'light' ? (
                  <LuMoon className="w-4 h-4 text-black" />
                ) : (
                  <LuSun className="w-4 h-4 text-black" />
                )}
              </button>
            </div>
          </div>
        </header>

        <main className="max-w-2xl mx-auto px-4">
          <Routes>
            <Route path="/" element={<TopicsPage />} />
            <Route path="/practice/:topicId" element={<PracticePage />} />
            <Route path="/feedback" element={<FeedbackPage />} />
            <Route path="/profile" element={<ProfilePage />} />
          </Routes>
        </main>
      </BrowserRouter>
    </div>
  );
}

export default App;